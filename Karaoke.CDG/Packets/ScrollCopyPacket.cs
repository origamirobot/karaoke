namespace Karaoke.CDG.Packets
{

    /// <summary>
    /// Represents a packet of CDG data that is a scroll copy instruction. The Scroll Copy instruction operates in exactly the same way as the Scroll Preset instruction, 
    /// with one exception; the cells that are shifted out of view at one edge are not lost, but instead shifted back into view from the opposite edge. Hence there are no 
    /// Color argument since no cells have to be initialized to a fixed color.
    /// </summary>
    /// <seealso cref="WordSmith.CDG.Packets.ScrollPacket" />
    /// <see href="http://www.cdgfix.com/help/3.x/Technical_information/The_CDG_graphics_format.htm#Scroll%20Copy"/>
    public class ScrollCopyPacket : ScrollPacket
    {

        #region PUBLIC ACCESSORS


        /// <summary>
        /// Gets the instruction.
        /// </summary>
        public override Instruction Instruction => Instruction.ScrollCopy;


        #endregion PUBLIC ACCESSORS

        #region PUBLIC METHODS


        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override String ToString() => "Scroll Copy";

        /// <summary>
        /// Converts the data in this packet into a byte array.
        /// </summary>
        /// <returns>A 24 byte array that represents the data in this object.</returns>
        public override Byte[] ToByteArray()
        {
            var result = base.ToByteArray();
            return result;
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public override Object Clone()
        {
            var packet = new ScrollCopyPacket() { Index = this.Index };
            Array.Copy(this.Data, packet.Data, this.Data.Length);
            return packet;
        }


        #endregion PUBLIC METHODS

    }

}
