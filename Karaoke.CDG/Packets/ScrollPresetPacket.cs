namespace Karaoke.CDG.Packets
{

    /// <summary>
    /// Represents a packet of CDG data the contains a scroll preset instruction. This instruction shifts the CDG canvas left/right and/or up/down, making the graphics appear as moving across the screen. 
    /// Cells that are shifted out of the CDG canvas at the edges (as defined by the HShift and VShift arguments) are lost, and cells that are shifted in from the opposite edges are filled with the color 
    /// specified by the Color argument. In addition to shifting cells sideways and up/down, the XOffset and YOffset arguments can be used to offset the display of the CDG canvas by the specified number 
    /// of pixels to the left and up, respectively.
    /// </summary>
    /// <seealso cref="WordSmith.CDG.Packets.ScrollPacket" />
    /// <see href="http://www.cdgfix.com/help/3.x/Technical_information/The_CDG_graphics_format.htm#Scroll%20Preset"/>
    public class ScrollPresetPacket : ScrollPacket
    {

        #region PUBLIC ACCESSORS


        /// <summary>
        /// Gets the instruction.
        /// </summary>
        public override Instruction Instruction => Instruction.ScrollPreset;

        /// <summary>
        /// Gets or sets the color of the fill.
        /// </summary>
        public Int16 FillColorIndex { get; set; }


        #endregion PUBLIC ACCESSORS

        #region PUBLIC METHODS


        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override String ToString() => "Scroll Preset";

        /// <summary>
        /// Converts the data in this packet into a byte array.
        /// </summary>
        /// <returns>A 24 byte array that represents the data in this object.</returns>
        public override Byte[] ToByteArray()
        {
            var result = base.ToByteArray();
            result[Packet.PacketDataOffset + ScrollPacket.FillColorIndexOffset] = (Byte)FillColorIndex;
            return result;
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public override Object Clone()
        {
            var packet = new ScrollPresetPacket() { Index = this.Index };
            Array.Copy(this.Data, packet.Data, this.Data.Length);
            return packet;
        }


        #endregion PUBLIC METHODS

    }

}
