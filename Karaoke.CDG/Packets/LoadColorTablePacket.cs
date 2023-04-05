using System.Drawing;

namespace Karaoke.CDG.Packets
{

    /// <summary>
    /// Represents a CDG packet that contains a load color table instruction. Contains the color table that
    /// will be available when drawing pixels on the screen. 
    /// </summary>
    /// <seealso cref="WordSmith.CDG.Packets.Packet" />
    public class LoadColorTablePacket : Packet
    {

        #region CONSTANTS



        /// <summary>
        /// The color table size
        /// </summary>
        public const Int32 ColorTableSize = 8;

        /// <summary>
        /// The default low color set colors
        /// </summary>
        public static readonly Color[] DefaultLowColors = new Color[LoadColorTablePacket.ColorTableSize]
        {
            Color.FromArgb(0, 0, 0),
            Color.FromArgb(127, 0, 0),
            Color.FromArgb(0, 127, 0),
            Color.FromArgb(0, 0, 127),
            Color.FromArgb(127, 127, 0),
            Color.FromArgb(127, 0, 127),
            Color.FromArgb(0, 127, 127),
            Color.FromArgb(64, 64, 64),
        };

        /// <summary>
        /// The default high color set colors
        /// </summary>
        public static readonly Color[] DefaultHighColors = new Color[LoadColorTablePacket.ColorTableSize]
        {
            Color.FromArgb(127, 127, 127),
            Color.FromArgb(255, 0, 0),
            Color.FromArgb(0, 255, 0),
            Color.FromArgb(0, 0, 255),
            Color.FromArgb(255, 255, 0),
            Color.FromArgb(255, 0, 255),
            Color.FromArgb(0, 255, 255),
            Color.FromArgb(255, 255, 255),
        };


        #endregion CONSTANTS

        #region PUBLIC ACCESSORS


        /// <summary>
        /// Gets or sets the instruction.
        /// </summary>
        public override Instruction Instruction => (Set == ColorTableSet.High) ? Instruction.LoadColorTableHigh : Instruction.LoadColorTableLow;

        /// <summary>
        /// Gets the set.
        /// </summary>
        public ColorTableSet Set { get; set; }

        /// <summary>
        /// Gets or sets the colors.
        /// </summary>
        public Color[] Colors { get; set; }



        #endregion PUBLIC ACCESSORS

        #region CONSTRUCTORS


        /// <summary>
        /// Initializes a new instance of the <see cref="LoadColorTablePacket" /> class.
        /// </summary>
        /// <param name="set">The set.</param>
        public LoadColorTablePacket(ColorTableSet set)
        {
            Set = set;
            // SET THE DEFAULT COLORS
            Colors = new Color[ColorTableSize];
            for (var i = 0; i < ColorTableSize; i++)
            {
                Colors[i] = Set == ColorTableSet.Low
                    ? Color.FromArgb(DefaultLowColors[i].A, DefaultLowColors[i].R, DefaultLowColors[i].G, DefaultLowColors[i].B)
                    : Color.FromArgb(DefaultHighColors[i].A, DefaultHighColors[i].R, DefaultHighColors[i].G, DefaultHighColors[i].B);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadColorTablePacket"/> class.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <param name="colors">The colors.</param>
        public LoadColorTablePacket(ColorTableSet set, params Color[] colors)
            : this(set)
        {
            if (colors.Length < ColorTableSize)
                throw new ArgumentException($"There must be {ColorTableSize} colors", nameof(colors));

            Colors = colors;
        }


        #endregion CONSTRUCTORS

        #region PUBLIC METHODS


        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override String ToString() => $"Color Table {Set}";

        /// <summary>
        /// Converts the data in this packet into a byte array.
        /// </summary>
        /// <returns>A 24 byte array that represents the data in this object.</returns>
        public override Byte[] ToByteArray()
        {
            var result = base.ToByteArray();

            for (var i = 0; i < LoadColorTablePacket.ColorTableSize; i++)
            {
                var color = Colors[i];

                var r = (Int32)(color.R / 17);
                var g = (Int32)(color.G / 17);
                var b = (Int32)(color.B / 17);

                var byte1 = (Byte)((r << 2) | (g >> 2));
                var byte2 = (Byte)(((g & 0x3) << 4) | b);

                result[Packet.PacketDataOffset + i * 2] = byte1;
                result[Packet.PacketDataOffset + i * 2 + 1] = byte2;
            }

            return result;
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public override Object Clone()
        {
            var clone = new LoadColorTablePacket(Set);
            Array.Copy(Colors, clone.Colors, Colors.Length);
            return clone;
        }


        #endregion PUBLIC METHODS

    }

}
