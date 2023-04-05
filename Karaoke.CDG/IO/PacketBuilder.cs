using Karaoke.CDG.Packets;
using Karaoke.CDG.Validation;
using Karaoke.Core.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karaoke.CDG.IO
{

    /// <summary>
    /// Responsible for reading <see cref="Packet"/> objects from a <see cref="CdgFileStream"/>.
    /// </summary>
    public class PacketBuilder : IPacketBuilder
    {

        #region PROTECTED PROPERTIES


        /// <summary>
        /// Gets the graphic utility.
        /// </summary>
        protected IGraphicUtility GraphicUtility { get; private set; }


        #endregion PROTECTED PROPERTIES

        #region CONSTRUCTORS


        /// <summary>
        /// Initializes a new instance of the <see cref="PacketBuilder"/> class.
        /// </summary>
        /// <param name="graphicUtility">The graphic utility.</param>
        public PacketBuilder(IGraphicUtility graphicUtility)
        {
            GraphicUtility = graphicUtility;
        }


        #endregion CONSTRUCTORS

        #region PUBLIC METHODS


        /// <summary>
        /// Builds the color table from bitmap.
        /// </summary>
        /// <param name="bmp">The BMP.</param>
        /// <returns></returns>
        public LoadColorTablePacket[] BuildColorTableFromBitmap(Bitmap bmp)
        {
            var packets = new List<LoadColorTablePacket>();
            var low = new LoadColorTablePacket(ColorTableSet.Low);
            var high = new LoadColorTablePacket(ColorTableSet.High);

            var colors = GraphicUtility.GetDistinctColorsFromBitmap(bmp);

            // MAKE SURE WE DONT HAVE MORE THAT 16 COLORS.
            if (colors.Count > 16)
                colors = GraphicUtility.QuantizeColors(colors, 16);

            // MAKE SURE WE HAVE EXACTLY 16
            if (colors.Count < 16)
            {
                var diff = 16 - colors.Count;
                for (var i = 0; i < diff; i++)
                    colors.Add(Color.FromName("Black"));
            }

            // ADD THE LOW COLORS
            for (var i = 0; i < 8; i++)
                low.Colors[i] = colors[i];

            // ADD THE HIGH COLORS
            for (var i = 8; i < 16; i++)
                high.Colors[i - 8] = colors[i];

            packets.Add(low);
            packets.Add(high);
            return packets.ToArray();
        }

        /// <summary>
        /// Converts the specified bitmap to an array of CDG packets.
        /// </summary>
        /// <param name="bmp">The bitmap to convert to CDG packets.</param>
        /// <returns></returns>
        public List<Packet> ConvertBitmapToCDG(Bitmap bmp)
        {
            var colorTable = BuildColorTableFromBitmap(bmp);
            var packets = colorTable.Cast<Packet>().ToList();

            for (Int16 i = 0; i < 10; i++)
                packets.Add(new MemoryPresetPacket() { ColorIndex = 0, Repeat = i });

            //packets.Add(new DefineTransparencyPacket() { ColorIndex = 3 });

            var widthInTiles = CdgBitmap.FullWidth / TileBlockPacket.TileWidth;
            var heightInTiles = CdgBitmap.FullHeight / TileBlockPacket.TileHeight;

            for (var y = 0; y < heightInTiles; y++)
            {
                for (var x = 0; x < widthInTiles; x++)
                {
                    // LIST OF TileBlockPackets THAT MAKE UP THIS SECTION
                    var batch = new List<TileBlockPacket>();

                    for (var tileY = 0; tileY < TileBlockPacket.TileHeight; tileY++)
                    {

                        // 1.) GET ALL THE COLORS IN THIS TILE IN THE SUPPLIED BITMAP
                        // 2.) CREATE A NEW TILE PACKET FOR EACH TILE WITH OFF COLOR SET TO
                        //     TRANSPARENT COLOR AND ON COLOR SET TO THAT COLORS INDEX
                        //     TURNING ON ONLY THE BITS WHERE THAT PIXEL EXISTS


                        for (var tileX = 0; tileX < TileBlockPacket.TileWidth; tileX++)
                        {
                            var absoluteX = ((x * TileBlockPacket.TileWidth) + tileX);
                            var absoluteY = ((y * TileBlockPacket.TileHeight) + tileY);

                            var pixel = bmp.GetPixel(absoluteX, absoluteY);

                            var colorIndex = 0;
                            for (var i = 0; i < colorTable[0].Colors.Length; i++)
                            {
                                if (colorTable[0].Colors[i] == pixel)
                                {
                                    colorIndex = i;
                                    break;
                                }
                            }

                            if (colorIndex == 0)
                            {
                                for (var i = 0; i < colorTable[1].Colors.Length; i++)
                                {
                                    if (colorTable[1].Colors[i] == pixel)
                                    {
                                        colorIndex = i;
                                        break;
                                    }
                                }
                            }

                            // CHECK IF THERE IS ALREADY A TILE THAT HAS THIS COLOR IN IT
                            var tileBlock = batch.FirstOrDefault(block => block.OnColorIndex == colorIndex);
                            if (tileBlock == null)
                            {
                                tileBlock = new TileBlockPacket(TileBlockType.XOR) { OnColorIndex = (Int16)colorIndex, Row = (Int16)y, Column = (Int16)x };
                                batch.Add(tileBlock);
                            }
                            tileBlock.Pixels[tileY, tileX] = true;

                        }
                    }
                    packets.AddRange(batch);
                }
            }




            return packets;
        }

        /// <summary>
        /// Reads the next packet in the file stream.
        /// </summary>
        /// <param name="stream">The stream being read from.</param>
        /// <returns>
        ///   <c>true</c> if a packet was read successfully; otherwise <c>false</c>
        /// </returns>
        public Packet ReadPacket(ICdgFileStream stream)
        {
            if (stream == null || stream.Eof())
                return null;

            var buffer = new Byte[Packet.PacketSize];
            var bytesRead = 0;

            bytesRead = stream.Read(buffer, buffer.Length);
            var instruction = (Instruction)(buffer[Packet.InstructionOffset] & 0x3F);

            Int32? index = null;
            index = (Int32)((stream.Position / Packet.PacketSize) - 1);

            switch (instruction)
            {
                case Instruction.BorderPreset: return ReadBorderPreset(buffer, index);
                case Instruction.DefineTransparentColor: return ReadTransparencyPacket(buffer);
                case Instruction.LoadColorTableHigh: return ReadColorTablePacket(buffer, ColorTableSet.High, index);
                case Instruction.LoadColorTableLow: return ReadColorTablePacket(buffer, ColorTableSet.Low, index);
                case Instruction.MemoryPreset: return ReadMemoryPresetPacket(buffer, index);
                case Instruction.ScrollCopy: return ReadScrollCopyPacket(buffer, index);
                case Instruction.ScrollPreset: return ReadScrollPresetPacket(buffer, true, index);
                case Instruction.TileBlock: return ReadTileBlockPacket(buffer, TileBlockType.Normal, index);
                case Instruction.TileBlockXOR: return ReadTileBlockPacket(buffer, TileBlockType.XOR, index);
                default: return ReadUnknownPacket(buffer);
            }
        }

        /// <summary>
        /// Writes the specified packet to the specified file stream.
        /// </summary>
        /// <param name="stream">The stream to write the packet to.</param>
        /// <param name="packet">The packet to write to the stream.</param>
        /// <returns>The number of bytes writen to the stream.</returns>
        public Int32 WritePacket(ICdgFileStream stream, Packet packet)
        {
            var buffer = packet.ToByteArray();
            return stream.Write(buffer, buffer.Length);
        }

        /// <summary>
        /// Reads over the collection of packets and attempts to fix the broken ones.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <remarks>
        /// The two types of
        /// </remarks>
        public List<ValidationResult> RepairCdg(CdgFile file)
        {
            var results = new List<ValidationResult>();
            var rules = new Rules(file);
            foreach (var packet in file.Packets)
            {
                if (packet is TileBlockPacket)
                    results.AddRange(packet.Validate(rules));
            }
            return results;
        }


        #endregion PUBLIC METHODS

        #region PROTECTED METHODS


        /// <summary>
        /// Reads the unknown packet.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        protected virtual TimingPacket ReadUnknownPacket(Byte[] data)
        {
            var packet = new TimingPacket();
            Array.Copy(data, packet.Data, data.Length);
            return packet;
        }

        /// <summary>
        /// Reads a border preset packet from the stream.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        protected virtual BorderPresetPacket ReadBorderPreset(Byte[] data, Int32? index = null)
        {
            var packet = new BorderPresetPacket() { Index = index };
            Array.Copy(data, packet.Data, data.Length);
            packet.ColorIndex = (Int16)((data[Packet.PacketDataOffset + BorderPresetPacket.ColorIndexOffset] & BorderPresetPacket.ColorIndexBitMask) & 0xF);
            OnBorderPresetPacketRead(packet);
            return packet;
        }

        /// <summary>
        /// Reads the tile block packet.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="type">The type of tile block to read.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        protected virtual TileBlockPacket ReadTileBlockPacket(Byte[] data, TileBlockType type, Int32? index = null)
        {
            var packet = new TileBlockPacket(type) { Index = index };
            Array.Copy(data, packet.Data, data.Length);

            packet.OffColorIndex = (Int16)((data[Packet.PacketDataOffset + TileBlockPacket.OffColorIndexOffset] & 0x3F) & 0x0F);
            packet.OnColorIndex = (Int16)((data[Packet.PacketDataOffset + TileBlockPacket.OnColorIndexOffset] & 0x3F) & 0x0F);
            packet.Row = (Int16)(data[Packet.PacketDataOffset + TileBlockPacket.RowOffset] & 0x3F);
            packet.Column = (Int16)(data[Packet.PacketDataOffset + TileBlockPacket.ColumnOffset] & 0x3F);

            //if (packet.Row > (CdgBitmap.FullHeight - TileBlockPacket.TileHeight))
            //	return null;

            //if (packet.Column > (CdgBitmap.FullWidth - TileBlockPacket.TileWidth))
            //	return null;

            for (var y = 0; y < TileBlockPacket.TileHeight; y++)
            {
                var d = (data[(Packet.PacketDataOffset + (4 + y)) & 0x3F]);
                for (var x = 0; x < TileBlockPacket.TileWidth; x++)
                {
                    var on = (((d >> (5 - x)) & 0x1) != 0);
                    packet.Pixels[y, x] = on;
                }
            }
            OnTileBlockPacketRead(packet);
            return packet;
        }

        /// <summary>
        /// Reads the transparency packet.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        protected virtual DefineTransparencyPacket ReadTransparencyPacket(Byte[] data, Int32? index = null)
        {
            var packet = new DefineTransparencyPacket() { Index = index };
            Array.Copy(data, packet.Data, data.Length);
            packet.ColorIndex = (Int16)(data[Packet.PacketDataOffset + DefineTransparencyPacket.ColorIndexOffset] & 0xF);
            OnDefineTransparencyPacketRead(packet);
            return packet;
        }

        /// <summary>
        /// Reads a color table packet from the file stream.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="set">The color table set to read.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        protected virtual LoadColorTablePacket ReadColorTablePacket(Byte[] data, ColorTableSet set, Int32? index = null)
        {
            var packet = new LoadColorTablePacket(set) { Index = index };
            Array.Copy(data, packet.Data, data.Length);
            for (var i = 0; i < 8; i++)
            {
                var byte0 = data[Packet.PacketDataOffset + (2 * i)] & 0x3F;
                var byte1 = data[Packet.PacketDataOffset + (2 * i + 1)] & 0x3F;
                var r = (byte0 & 0x3F) >> 2;
                var g = ((byte0 & 0x3) << 2) | ((byte1 & 0x3F) >> 4);
                var b = (byte1 & 0xF);
                r *= 17;
                g *= 17;
                b *= 17;
                packet.Colors[i] = Color.FromArgb(r, g, b);

            }
            OnLoadColorTablePacketRead(packet);
            return packet;
        }

        /// <summary>
        /// Reads the memory preset packet.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        protected virtual MemoryPresetPacket ReadMemoryPresetPacket(Byte[] data, Int32? index = null)
        {
            var packet = new MemoryPresetPacket() { Index = index };
            Array.Copy(data, packet.Data, data.Length);

            packet.ColorIndex = (Int16)(data[Packet.PacketDataOffset + MemoryPresetPacket.ColorIndexOffset] & 0xF);
            packet.Repeat = (Int16)(data[Packet.PacketDataOffset + MemoryPresetPacket.RepeatOffset] & 0xF);
            OnMemoryPresetPacketRead(packet);
            return packet;
        }

        /// <summary>
        /// Reads the scrol copy packet.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        protected virtual ScrollCopyPacket ReadScrollCopyPacket(Byte[] data, Int32? index = null)
        {
            var spp = ReadScrollPresetPacket(data, false, index);

            // COPY EVERYTHING FROM THE ScrollPresetPacket EXCEPT THE FillColorIndex
            var packet = new ScrollCopyPacket
            {
                HorizontalScrollDirection = spp.HorizontalScrollDirection,
                VerticalScrollDirection = spp.VerticalScrollDirection,
                HorizontalScrollOffset = spp.HorizontalScrollOffset,
                VerticalScrollOffset = spp.VerticalScrollOffset,
                Index = spp.Index,
            };

            OnScrollCopyPacketRead(packet);
            return packet;
        }

        /// <summary>
        /// Reads the scroll preset packet.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="fireEvent">if set to <c>true</c> fire the <see cref="ScrollPresetPacketRead" /> event.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        protected virtual ScrollPresetPacket ReadScrollPresetPacket(Byte[] data, Boolean fireEvent = true, Int32? index = null)
        {
            var packet = new ScrollPresetPacket() { Index = index };
            Array.Copy(data, packet.Data, data.Length);
            packet.FillColorIndex = (Int16)(data[Packet.PacketDataOffset + 0] & 0xF);
            var hScroll = data[Packet.PacketDataOffset + 1] & 0x3f;
            var vScroll = data[Packet.PacketDataOffset + 2] & 0x3f;

            packet.HorizontalScrollDirection = (HorizontalScrollDirection)((hScroll & 0x30) >> 4);
            packet.VerticalScrollDirection = (VerticalScrollDirection)((vScroll & 0x30) >> 4);

            var hOffset = (hScroll & 0x7);
            var vOffset = (vScroll & 0xf);


            packet.HorizontalScrollOffset = (Int16)(hOffset < 5 ? hOffset : 5);
            packet.VerticalScrollOffset = (Int16)(vOffset < 11 ? vOffset : 11);

            if (fireEvent)
                OnScrollPresetPacketRead(packet);

            return packet;
        }


        #endregion PROTECTED METHODS

        #region PUBLIC EVENTS


        /// <summary>
        /// Called when a <see cref="ScrollCopyPacket"/> is read from the <see cref="ICdgFileStream"/>.
        /// </summary>
        /// <param name="packet">The packet.</param>
        protected void OnScrollCopyPacketRead(ScrollCopyPacket packet) => ScrollCopyPacketRead?.Invoke(this, packet);

        /// <summary>
        /// Called when a <see cref="ScrollPresetPacket"/> is read from the <see cref="ICdgFileStream"/>.
        /// </summary>
        /// <param name="packet">The packet.</param>
        protected void OnScrollPresetPacketRead(ScrollPresetPacket packet) => ScrollPresetPacketRead?.Invoke(this, packet);

        /// <summary>
        /// Called when a <see cref="MemoryPresetPacket"/> is read from the <see cref="ICdgFileStream"/>.
        /// </summary>
        /// <param name="packet">The packet.</param>
        protected void OnMemoryPresetPacketRead(MemoryPresetPacket packet) => MemoryPresetPacketRead?.Invoke(this, packet);

        /// <summary>
        /// Called when a <see cref="DefineTransparencyPacket"/> is read from the <see cref="ICdgFileStream"/>.
        /// </summary>
        /// <param name="packet">The packet.</param>
        protected void OnDefineTransparencyPacketRead(DefineTransparencyPacket packet) => DefineTransparencyPacketRead?.Invoke(this, packet);

        /// <summary>
        /// Called when a <see cref="LoadColorTablePacket"/> is read from the <see cref="ICdgFileStream"/>.
        /// </summary>
        /// <param name="packet">The packet.</param>
        protected void OnLoadColorTablePacketRead(LoadColorTablePacket packet) => LoadColorTablePacketRead?.Invoke(this, packet);

        /// <summary>
        /// Called when a <see cref="BorderPresetPacket"/> is read from the <see cref="ICdgFileStream"/>.
        /// </summary>
        /// <param name="packet">The packet.</param>
        protected void OnBorderPresetPacketRead(BorderPresetPacket packet) => BorderPresetPacketRead?.Invoke(this, packet);

        /// <summary>
        /// Called when a <see cref="TileBlockPacket"/> is read from the <see cref="ICdgFileStream"/>.
        /// </summary>
        /// <param name="packet">The packet.</param>
        protected void OnTileBlockPacketRead(TileBlockPacket packet) => TileBlockPacketRead?.Invoke(this, packet);


        /// <summary>
        /// Occurs when a <see cref="ScrollCopyPacket" /> is read from a <see cref="CdgFileStream" />.
        /// </summary>
        public event EventHandler<ScrollCopyPacket>? ScrollCopyPacketRead;

        /// <summary>
        /// Occurs when a <see cref="ScrollPresetPacket" /> is read from a <see cref="CdgFileStream" />.
        /// </summary>
        public event EventHandler<ScrollPresetPacket>? ScrollPresetPacketRead;

        /// <summary>
        /// Occurs when a <see cref="LoadColorTablePacket" /> is read from a <see cref="CdgFileStream" />.
        /// </summary>
        public event EventHandler<TileBlockPacket>? TileBlockPacketRead;

        /// <summary>
        /// Occurs when a <see cref="MemoryPresetPacket" /> is read from a <see cref="CdgFileStream" />.
        /// </summary>
        public event EventHandler<MemoryPresetPacket>? MemoryPresetPacketRead;

        /// <summary>
        /// Occurs when a <see cref="BorderPresetPacket" /> is read from a <see cref="CdgFileStream" />.
        /// </summary>
        public event EventHandler<BorderPresetPacket>? BorderPresetPacketRead;

        /// <summary>
        /// Occurs when a <see cref="DefineTransparencyPacket" /> is read from a <see cref="CdgFileStream" />.
        /// </summary>
        public event EventHandler<DefineTransparencyPacket>? DefineTransparencyPacketRead;

        /// <summary>
        /// Occurs when a <see cref="LoadColorTablePacket" /> is read from a <see cref="CdgFileStream" />.
        /// </summary>
        public event EventHandler<LoadColorTablePacket>? LoadColorTablePacketRead;


        #endregion PUBLIC EVENTS

    }

    /// <summary>
    /// Defines an interface that packet builders must implement. Responsible for 
    /// reading <see cref="Packet"/> objects from a <see cref="CdgFileStream"/>.
    /// </summary>
    public interface IPacketBuilder
    {

        #region METHODS


        /// <summary>
        /// Reads the next packet in the file stream.
        /// </summary>
        /// <param name="stream">The stream being read from.</param>
        /// <returns>
        ///   <c>true</c> if a packet was read successfully; otherwise <c>false</c>
        /// </returns>
        Packet ReadPacket(ICdgFileStream stream);

        /// <summary>
        /// Builds the color table from bitmap.
        /// </summary>
        /// <param name="bmp">The BMP.</param>
        /// <returns></returns>
        LoadColorTablePacket[] BuildColorTableFromBitmap(Bitmap bmp);

        /// <summary>
        /// Converts the specified bitmap to an array of CDG packets.
        /// </summary>
        /// <param name="bmp">The bitmap to convert to CDG packets.</param>
        /// <returns></returns>
        List<Packet> ConvertBitmapToCDG(Bitmap bmp);

        /// <summary>
        /// Reads over the collection of packets and attempts to fix the broken ones.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <remarks>
        /// The two types of
        /// </remarks>
        List<ValidationResult> RepairCdg(CdgFile file);


        #endregion METHODS

        #region EVENTS


        /// <summary>
        /// Occurs when a <see cref="ScrollCopyPacket"/> is read from a <see cref="CdgFileStream"/>.
        /// </summary>
        event EventHandler<ScrollCopyPacket>? ScrollCopyPacketRead;

        /// <summary>
        /// Occurs when a <see cref="ScrollPresetPacket"/> is read from a <see cref="CdgFileStream"/>.
        /// </summary>
        event EventHandler<ScrollPresetPacket>? ScrollPresetPacketRead;

        /// <summary>
        /// Occurs when a <see cref="LoadColorTablePacket"/> is read from a <see cref="CdgFileStream"/>.
        /// </summary>
        event EventHandler<TileBlockPacket>? TileBlockPacketRead;

        /// <summary>
        /// Occurs when a <see cref="MemoryPresetPacket"/> is read from a <see cref="CdgFileStream"/>.
        /// </summary>
        event EventHandler<MemoryPresetPacket>? MemoryPresetPacketRead;

        /// <summary>
        /// Occurs when a <see cref="BorderPresetPacket"/> is read from a <see cref="CdgFileStream"/>.
        /// </summary>
        event EventHandler<BorderPresetPacket>? BorderPresetPacketRead;

        /// <summary>
        /// Occurs when a <see cref="DefineTransparencyPacket"/> is read from a <see cref="CdgFileStream"/>.
        /// </summary>
        event EventHandler<DefineTransparencyPacket>? DefineTransparencyPacketRead;

        /// <summary>
        /// Occurs when a <see cref="LoadColorTablePacket"/> is read from a <see cref="CdgFileStream"/>.
        /// </summary>
        event EventHandler<LoadColorTablePacket>? LoadColorTablePacketRead;


        #endregion EVENTS

    }


}
