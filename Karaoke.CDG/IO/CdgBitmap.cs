using Karaoke.CDG.Packets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Karaoke.CDG.IO
{

    /// <summary>
    /// Represents a bitmap that can be altered by CDG <see cref="Packet"/> objects.
    /// </summary>
    public class CdgBitmap : IDisposable
    {

        #region CONSTANTS


        public const Int32 FullWidth = 300;
        public const Int32 FullHeight = 216;
        public const Int32 ColorTableSize = 16;


        #endregion CONSTANTS

        #region PRIVATE PROPERTIES


        /// <summary>
        /// Gets the pixel colors. Contains which color index should be applied to which pixel. Kind of like a paint by numbers.
        /// </summary>
        protected Byte[,] PixelColors { get; private set; }

        /// <summary>
        /// Gets the current color table to use.
        /// </summary>
        protected Int32[] ColorTable { get; private set; }

        /// <summary>
        /// Gets the RGB data. This is a y by x matrix of RGB colors.
        /// </summary>
        protected Int64[,] RgbData { get; private set; }

        /// <summary>
        /// Gets the color index to use for a transparent color.
        /// </summary>
        protected Int32 TransparentColor { get; private set; }

        /// <summary>
        /// Gets the index of the preset color.
        /// </summary>
        protected Int32 PresetColorIndex { get; private set; }

        /// <summary>
        /// Gets the color index to use for the border color.
        /// </summary>
        protected Int32 BorderColorIndex { get; private set; }

        /// <summary>
        /// Gets the vertical offset.
        /// </summary>
        protected Int32 VerticalOffset { get; private set; }

        /// <summary>
        /// Gets the horizontal offset.
        /// </summary>
        protected Int32 HorizontalOffset { get; private set; }


        #endregion PRIVATE PROPERTIES

        #region PUBLIC ACCESSORS


        /// <summary>
        /// Gets the image.
        /// </summary>
        public Bitmap Image
        {
            get
            {
                var stream = new MemoryStream();
                try
                {
                    for (var row = 0; row < FullHeight; row++)
                    {
                        for (var col = 0; col < FullWidth; col++)
                        {
                            var color = Convert.ToInt32(RgbData[row, col]);
                            var data = BitConverter.GetBytes(color);
                            stream.Write(data, 0, 4);
                        }
                    }
                }
                catch (Exception)
                {
                    // RETURN AN EMPTY BITMAP IF THIS FAILS
                }
                var bitmap = ConvertStreamToBitmap(stream);
                return bitmap;
            }
        }


        #endregion PUBLIC ACCESSORS

        #region CONSTRUCTORS


        /// <summary>
        /// Initializes a new instance of the <see cref="CdgBitmap"/> class.
        /// </summary>
        public CdgBitmap()
        {
            PixelColors = new Byte[FullHeight, FullWidth];
            RgbData = new Int64[FullHeight, FullWidth];
            ColorTable = new Int32[ColorTableSize];
        }


        #endregion CONSTRUCTORS

        #region PRIVATE METHODS


        /// <summary>
        /// Applies the memory preset.
        /// </summary>
        /// <param name="packet">The packet.</param>
        protected virtual void ApplyMemoryPreset(MemoryPresetPacket packet)
        {
            // IGNORE REPEAT MEMORY PRESET MESSAGES
            // TODO: Should this be configurable?
            if ((packet.Repeat != 0))
                return;

            PresetColorIndex = packet.ColorIndex;
            BorderColorIndex = packet.ColorIndex;

            // LOOP ALL ROWS AND COLUMNS SETTING THE COLOR IN EACH ARRAY POSITION
            for (var row = 0; row <= FullHeight - 1; row++)
            {
                for (var col = 0; col <= FullWidth - 1; col++)
                {
                    PixelColors[row, col] = (Byte)packet.ColorIndex;
                }
            }
        }

        /// <summary>
        /// Applies the border preset packet.
        /// </summary>
        /// <param name="packet">The packet.</param>
        protected virtual void ApplyBorderPresetPacket(BorderPresetPacket packet)
        {
            // THE BORDER IS THE AREA AROUND THE OUTSIDE MARKED BY THE RECTANGLE (6,12,294,204)
            BorderColorIndex = packet.ColorIndex;
            for (var row = 0; row <= FullHeight - 1; row++)
            {
                for (var col = 0; col <= 5; col++)
                    PixelColors[row, col] = (Byte)packet.ColorIndex;

                for (var col = FullWidth - 6; col <= FullWidth - 1; col++)
                    PixelColors[row, col] = (Byte)packet.ColorIndex;
            }

            for (var col = 6; col <= FullWidth - 7; col++)
            {
                for (var row = 0; row <= 11; row++)
                    PixelColors[row, col] = (Byte)packet.ColorIndex;

                for (var row = FullHeight - 12; row <= FullHeight - 1; row++)
                    PixelColors[row, col] = (Byte)packet.ColorIndex;
            }
        }

        /// <summary>
        /// Applies the color table packet.
        /// </summary>
        /// <param name="packet">The packet.</param>
        protected virtual void ApplyColorTablePacket(LoadColorTablePacket packet)
        {
            for (var i = 0; i < LoadColorTablePacket.ColorTableSize; i++)
            {
                var color = packet.Colors[i].ToArgb();
                ColorTable[i + (packet.Set == ColorTableSet.High ? 8 : 0)] = color;
            }
        }

        /// <summary>
        /// Applies the scroll copy packet.
        /// </summary>
        /// <param name="packet">The packet.</param>
        protected virtual void ApplyScrollCopyPacket(ScrollCopyPacket packet)
        {
            //var movedPixels = new Byte[FullHeight, FullWidth];
            //for (var y = 0; y < FullHeight; y++)
            //{
            //	for (var x = 0; x < FullWidth; x++)
            //	{
            //		var moveToX = x;
            //		var moveToY = y;

            //		moveToY -= packet.VerticalScrollOffset;

            //		switch (packet.HorizontalScrollDirection)
            //		{
            //			case HorizontalScrollDirection.Left: moveToX -= packet.HorizontalScrollOffset; break;
            //			case HorizontalScrollDirection.Right: moveToX += packet.HorizontalScrollOffset; break;
            //			case HorizontalScrollDirection.None:
            //			default: moveToX = x; break;
            //		}
            //		switch (packet.VerticalScrollDirection)
            //		{
            //			case VerticalScrollDirection.Up: moveToY -= packet.VerticalScrollOffset; break;
            //			case VerticalScrollDirection.Down: moveToY += packet.VerticalScrollOffset; break;
            //			case VerticalScrollDirection.None:
            //			default: moveToY = y; break;
            //		}

            //		var currentColorIndex = PixelColors[y, x];

            //		// IF PIXELS ARE SCROLLED OFFSCREEN, IGNORE THEM
            //		if (moveToX < 0 || moveToX >= FullWidth || moveToY < 0 || moveToY >= FullHeight)
            //			continue;

            //		movedPixels[moveToY, moveToX] = currentColorIndex;
            //		movedPixels[y, x] = currentColorIndex;
            //	}
            //}
            //PixelColors = movedPixels;
        }

        /// <summary>
        /// Applies the scroll preset packet.
        /// </summary>
        /// <param name="packet">The packet.</param>
        protected virtual void ApplyScrollPresetPacket(ScrollPresetPacket packet)
        {
            //var movedPixels = new Byte[FullHeight, FullWidth];
            //for (var y = 0; y < FullHeight; y++)
            //{
            //	for (var x = 0; x < FullWidth; x++)
            //	{
            //		var moveToX = x;
            //		var moveToY = y;

            //		moveToY -= packet.VerticalScrollOffset;
            //		switch (packet.HorizontalScrollDirection)
            //		{
            //			case HorizontalScrollDirection.Left: moveToX -= packet.HorizontalScrollOffset; break;
            //			case HorizontalScrollDirection.Right: moveToX += packet.HorizontalScrollOffset; break;
            //			case HorizontalScrollDirection.None:
            //			default: moveToX = x; break;
            //		}
            //		//switch(packet.VerticalScrollDirection)
            //		//{
            //		//	case VerticalScrollDirection.Up: moveToY -= packet.VerticalScrollOffset; break;
            //		//	case VerticalScrollDirection.Down: moveToY += packet.VerticalScrollOffset; break;
            //		//	case VerticalScrollDirection.None:
            //		//	default: moveToY = y; break;
            //		//}

            //		var currentColorIndex = PixelColors[y, x];

            //		// IF PIXELS ARE SCROLLED OFFSCREEN, IGNORE THEM
            //		if (moveToX < 0 || moveToX >= FullWidth || moveToY < 0 || moveToY >= FullHeight)
            //			continue;

            //		movedPixels[moveToY, moveToX] = currentColorIndex;
            //		// FILL IN THE PREVIOUS SPOT WITH THE FILL COLOR INDEX
            //		movedPixels[y, x] = (Byte)packet.FillColorIndex; 
            //	}
            //}
            //PixelColors = movedPixels;
        }

        /// <summary>
        /// Applies the transparent color packet.
        /// </summary>
        /// <param name="packet">The packet.</param>
        protected virtual void ApplyTransparentColorPacket(DefineTransparencyPacket packet)
        {
            TransparentColor = packet.ColorIndex;
        }

        /// <summary>
        /// Applies the tile block packet.
        /// </summary>
        /// <param name="packet">The packet.</param>
        protected virtual void ApplyTileBlockPacket(TileBlockPacket packet)
        {
            // CHECK TO MAKE SURE THE PACKET IS WITHIN THE RANGE OF THE BITMAP
            if ((packet.Row >= (FullHeight / TileBlockPacket.TileHeight)))
            {
                OnErrorDetected(packet, "Packet row is out of range");
                return;
            }
            if ((packet.Column >= (FullWidth / TileBlockPacket.TileWidth)))
            {
                OnErrorDetected(packet, "Packet column is out of range");
                return;
            }

            var pixels = new Byte[TileBlockPacket.TileHeight];
            for (var row = 0; row < TileBlockPacket.TileHeight; row++)
            {
                pixels[row] = (Byte)(packet.Data[Packet.PacketDataOffset + TileBlockPacket.PixelOffset + row] & 0x3F);
            }



            for (var y = 0; y < TileBlockPacket.TileHeight; y++)
            {
                for (var x = 0; x < TileBlockPacket.TileWidth; x++)
                {
                    // CHECK IF THIS PIXEL IS ON OR OFF, AND PICK THE CORRECT COLOR
                    var color = packet.Pixels[y, x] ? (Int32)packet.OnColorIndex : (Int32)packet.OffColorIndex;

                    // FIND THE LOCATION OF THIS PIXEL INSIDE THE ENTIRE BITMAP
                    var pixelX = (packet.Column * TileBlockPacket.TileWidth) + x;
                    var pixelY = (packet.Row * TileBlockPacket.TileHeight) + y;

                    // IF FOR SOME REASON THIS PIXEL IS OUTSIDE OF THE RANGE OF THE BITMAP, SKIP IT
                    if (pixelX > CdgBitmap.FullWidth || pixelY > CdgBitmap.FullHeight)
                        continue;

                    var bitTest = 0x20 >> x;

                    if (packet.Type == TileBlockType.XOR)
                    {
                        var currentIndex = PixelColors[pixelY, pixelX];
                        var newColor = currentIndex ^ (((pixels[y] & bitTest) == bitTest)
                            ? packet.OnColorIndex
                            : packet.OffColorIndex);

                        color = newColor;
                    }
                    else
                    {
                        color = (pixels[y] & bitTest) == bitTest
                            ? packet.OnColorIndex
                            : packet.OffColorIndex;
                    }

                    PixelColors[pixelY, pixelX] = (Byte)color;
                }
            }
        }

        /// <summary>
        /// Determines whether [is in border] [the specified x].
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        protected virtual Boolean IsInBorder(Int32 x, Int32 y)
        {
            return ((
                y < TileBlockPacket.TileHeight ||
                y >= FullHeight - TileBlockPacket.TileHeight ||
                x < TileBlockPacket.TileWidth ||
                x >= FullWidth - TileBlockPacket.TileWidth));
        }

        /// <summary>
        /// Converts the specified stream to a bitmap.
        /// </summary>
        /// <param name="stream">The stream to convert to a bitmap.</param>
        /// <returns></returns>
        protected virtual Bitmap ConvertStreamToBitmap(MemoryStream stream)
        {
            // CREATE A NEW BITMAP
            var bmp = new Bitmap(FullWidth, FullHeight, PixelFormat.Format32bppArgb);
            var bmpData = bmp.LockBits(new Rectangle(0, 0, FullWidth, FullHeight), ImageLockMode.WriteOnly, bmp.PixelFormat);
            stream.Seek(0, SeekOrigin.Begin);

            // COPY THE STREAM TO PIXELS
            for (var n = 0; n < stream.Length; n++)
            {
                var buffer = new byte[1];
                stream.Read(buffer, 0, 1);
                Marshal.WriteByte(bmpData.Scan0, n, buffer[0]);
            }
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        /// <summary>
        /// Scrolls the bitmap horizontally in the specified direction.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="fillColorIndex">Index of the fill color.</param>
        protected virtual void ScrollHorizontal(HorizontalScrollDirection direction, Int32 fillColorIndex)
        {
            if (direction == HorizontalScrollDirection.None)
                return;
        }

        /// <summary>
        /// Scrolls the bitmap vertically in the specified direction.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="fillColorIndex">Index of the fill color.</param>
        protected virtual void ScrollVertical(VerticalScrollDirection direction, Int32 fillColorIndex)
        {
            if (direction == VerticalScrollDirection.None)
                return;
        }


        #endregion PRIVATE METHODS

        #region PUBLIC METHODS


        /// <summary>
        /// Applies the specified packet to this bitmap and executes it's instruction.
        /// </summary>
        /// <param name="packet">The packet used to alter this bitmap's appearance.</param>
        public virtual void Apply(Packet packet)
        {
            switch (packet.Instruction)
            {
                case Instruction.BorderPreset: ApplyBorderPresetPacket((BorderPresetPacket)packet); break;
                case Instruction.DefineTransparentColor: ApplyTransparentColorPacket((DefineTransparencyPacket)packet); break;
                case Instruction.LoadColorTableHigh:
                case Instruction.LoadColorTableLow: ApplyColorTablePacket((LoadColorTablePacket)packet); break;
                case Instruction.MemoryPreset: ApplyMemoryPreset((MemoryPresetPacket)packet); break;
                case Instruction.ScrollCopy: ApplyScrollCopyPacket((ScrollCopyPacket)packet); break;
                case Instruction.ScrollPreset: ApplyScrollPresetPacket((ScrollPresetPacket)packet); break;
                case Instruction.TileBlock:
                case Instruction.TileBlockXOR: ApplyTileBlockPacket((TileBlockPacket)packet); break;
                    //default: throw new InvalidOperationException("Couln't apply unknown packet type to this bitmap");
            }
        }

        /// <summary>
        /// Resets all the in-memory values of this bitmap.
        /// </summary>
        public virtual void Reset()
        {
            Array.Clear(PixelColors, 0, PixelColors.Length);
            Array.Clear(ColorTable, 0, ColorTable.Length);

            PresetColorIndex = 0;
            TransparentColor = 0;
            HorizontalOffset = 0;
            VerticalOffset = 0;
            BorderColorIndex = 0;

            if (RgbData != null)
                Array.Clear(RgbData, 0, RgbData.Length);
        }

        /// <summary>
        /// Renders the image to match the generated pixels.
        /// </summary>
        public virtual void Render()
        {
            for (var row = 0; row < FullHeight; row++)
            {
                for (var col = 0; col < FullWidth; col++)
                {
                    var colorIndex = IsInBorder(col, row)
                        ? BorderColorIndex
                        : PixelColors[row + VerticalOffset, col + HorizontalOffset];

                    if (colorIndex >= ColorTable.Length)
                        colorIndex = 0;


                    RgbData[row, col] = ColorTable[colorIndex];
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() => Image.Dispose();


        #endregion PUBLIC METHODS

        #region PUBLIC EVENTS


        /// <summary>
        /// Occurs when an error is detected in a CDG packet.
        /// </summary>
        public event EventHandler<CdgErrorEventArgs>? ErrorDetected;

        /// <summary>
        /// Called when an error is detected in a CDG packet.
        /// </summary>
        /// <param name="packet">The packet that caused an error.</param>
        /// <param name="message">The message describing the error.</param>
        protected virtual void OnErrorDetected(Packet packet, String message) => ErrorDetected?.Invoke(this, new CdgErrorEventArgs(message, packet));


        #endregion PUBLIC EVENTS

    }

}
