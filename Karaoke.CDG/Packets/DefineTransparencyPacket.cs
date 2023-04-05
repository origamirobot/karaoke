namespace Karaoke.CDG.Packets
{

    /// <summary>
    /// Represents a CDG packet that contains a define transparency instruction. 
    /// Defines the index of the color is going to be used for the transparency color.
    /// </summary>
    /// <seealso cref="WordSmith.CDG.Packets.Packet" />
    public class DefineTransparencyPacket : Packet
    {

        #region CONSTANTS


        /// <summary>
        /// The data offset to use when setting the color index field.
        /// </summary>
        public const Int32 ColorIndexOffset = 0;


        #endregion CONSTANTS

        #region PUBLIC ACCESSORS


        /// <summary>
        /// Gets or sets the instruction.
        /// </summary>
        public override Instruction Instruction => Instruction.DefineTransparentColor;

        /// <summary>
        /// Gets or sets the index of the color to use for transparencies.
        /// </summary>
        public Int16 ColorIndex { get; set; }


        #endregion PUBLIC ACCESSORS

        #region PUBLIC METHODS


        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override String ToString() => "Define Transparency";

        /// <summary>
        /// Converts the data in this packet into a byte array.
        /// </summary>
        /// <returns>A 24 byte array that represents the data in this object.</returns>
        public override Byte[] ToByteArray()
        {
            var result = base.ToByteArray();
            result[Packet.PacketDataOffset + DefineTransparencyPacket.ColorIndexOffset] = (Byte)ColorIndex;
            return result;
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public override Object Clone()
        {
            var packet = new DefineTransparencyPacket() { Index = this.Index };
            Array.Copy(this.Data, packet.Data, this.Data.Length);
            return packet;
        }



        #endregion PUBLIC METHODS

    }

}
