namespace Karaoke.CDG.Packets
{

    /// <summary>
    /// Represents a CDG packet that contains a border preset instruction. 
    /// Set the index of the color to use as the border of the CDG image.
    /// </summary>
    /// <seealso cref="WordSmith.CDG.Packets.Packet" />
    public class BorderPresetPacket : Packet
    {

        #region CONSTANTS


        /// <summary>
        /// The data offset to use when populating the color index field.
        /// </summary>
        public const Int32 ColorIndexOffset = 0;

        /// <summary>
        /// The bit mask to use when setting the color index field.
        /// </summary>
        public const Byte ColorIndexBitMask = 0x3F;


        #endregion CONSTANTS

        #region PRIVATE PROPERTIES


        private Int16 _colorIndex;


        #endregion PRIVATE PROPERTIES

        #region PUBLIC ACCESSORS


        /// <summary>
        /// Gets or sets the instruction.
        /// </summary>
        public override Instruction Instruction => Instruction.BorderPreset;

        /// <summary>
        /// Gets or sets the index of the color.
        /// </summary>
        public Int16 ColorIndex
        {
            get => _colorIndex;
            set => _colorIndex = value; 
        }


        #endregion PUBLIC ACCESSORS

        #region CONSTRUCTORS


        /// <summary>
        /// Initializes a new instance of the <see cref="BorderPresetPacket"/> class.
        /// </summary>
        public BorderPresetPacket()
        {

        }


        #endregion CONSTRUCTORS

        #region PUBLIC METHODS


        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override String ToString() => "Border Preset";

        /// <summary>
        /// Converts the data in this packet into a byte array.
        /// </summary>
        /// <returns>A 24 byte array that represents the data in this object.</returns>
        public override Byte[] ToByteArray()
        {
            var result = base.ToByteArray();
            result[Packet.PacketDataOffset + BorderPresetPacket.ColorIndexOffset] = (Byte)ColorIndex;
            return result;
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            var packet = new BorderPresetPacket() { Index = this.Index };
            Array.Copy(this.Data, packet.Data, this.Data.Length);
            return packet;
        }


        #endregion PUBLIC METHODS

    }

}
