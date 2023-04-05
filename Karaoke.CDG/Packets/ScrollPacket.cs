namespace Karaoke.CDG.Packets
{

    /// <summary>
    /// Base class for classes that represent CDG scroll instructions.
    /// </summary>
    /// <seealso cref="WordSmith.CDG.Packets.Packet" />
    public abstract class ScrollPacket : Packet
    {

        #region CONSTANTS


        /// <summary>
        /// The data offset to use when setting the fill color index field.
        /// </summary>
        public const Int32 FillColorIndexOffset = 0;

        /// <summary>
        /// The data offset to use when setting the horizontal scroll index field.
        /// </summary>
        public const Int32 HorizontalIndexOffset = 1;

        /// <summary>
        /// The data offset to use when setting the vertical scroll index field.
        /// </summary>
        public const Int32 VerticalIndexOffset = 2;


        #endregion CONSTANTS

        #region PUBLIC ACCESSORS


        /// <summary>
        /// Gets or sets the horizontal scroll offset for this scroll command.
        /// </summary>
        public Int16 HorizontalScrollOffset { get; set; }

        /// <summary>
        /// Gets or sets the vertical scroll offset for this scroll command.
        /// </summary>
        public Int16 VerticalScrollOffset { get; set; }

        /// <summary>
        /// Gets or sets the horizontal scroll direction.
        /// </summary>
        public HorizontalScrollDirection HorizontalScrollDirection { get; set; }

        /// <summary>
        /// Gets or sets the vertical scroll direction.
        /// </summary>
        public VerticalScrollDirection VerticalScrollDirection { get; set; }


        #endregion PUBLIC ACCESSORS

        #region PUBLIC METHODS


        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override String ToString() => "Scroll Packet";

        /// <summary>
        /// Converts the data in this packet into a byte array.
        /// </summary>
        /// <returns>A 24 byte array that represents the data in this object.</returns>
        public override Byte[] ToByteArray()
        {
            var result = base.ToByteArray();
            // SCROLL DIRECTION IS IN FIRST NIBBLE AND OFFSET IS IN SECOND NIBBLE OF EACH BYTE
            var hByte = (((Byte)HorizontalScrollDirection & 0x03) << 4) | (HorizontalScrollOffset & 0x07);
            var vByte = (((Byte)VerticalScrollDirection & 0x03) << 4 | (VerticalScrollOffset & 0x07));
            result[Packet.PacketDataOffset + HorizontalIndexOffset] = (Byte)hByte;
            result[Packet.PacketDataOffset + VerticalIndexOffset] = (Byte)vByte;
            return result;
        }


        #endregion PUBLIC METHODS

    }

}
