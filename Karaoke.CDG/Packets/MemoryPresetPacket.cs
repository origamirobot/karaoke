namespace Karaoke.CDG.Packets
{

    /// <summary>
    /// Represents a CDG packet that contains a memory preset instruction. CDG Packet that sets the screen to a particular color
    /// </summary>
    /// <seealso cref="WordSmith.CDG.Packets.Packet" />
    public class MemoryPresetPacket : Packet
    {

        #region CONSTANTS


        /// <summary>
        /// The data offset to use when setting the color index field.
        /// </summary>
        public const Int32 ColorIndexOffset = 0;

        /// <summary>
        /// The data offset to use when setting the Repeat field
        /// </summary>
        public const Int32 RepeatOffset = 1;


        #endregion CONSTANTS

        #region PUBLIC ACCESSORS


        /// <summary>
        /// Gets or sets the instruction.
        /// </summary>
        public override Instruction Instruction => Instruction.MemoryPreset;

        /// <summary>
        /// Gets or sets the color to set the screen to.
        /// </summary>
        public Int16 ColorIndex { get; set; }

        /// <summary>
        /// When these commands appear in bunches (to insure that the screen gets cleared),
        /// the repeat count is used to number them. If this is true, and you have a
        /// reliable data stream, you can ignore the command if repeat != 0.
        /// </summary>
        public Int16 Repeat { get; set; }


        #endregion PUBLIC ACCESSORS

        #region PUBLIC METHODS


        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override String ToString() => "Memory Preset";

        /// <summary>
        /// Converts the data in this packet into a byte array.
        /// </summary>
        /// <returns>A 24 byte array that represents the data in this object.</returns>
        public override Byte[] ToByteArray()
        {
            var result = base.ToByteArray();
            result[Packet.PacketDataOffset + MemoryPresetPacket.ColorIndexOffset] = (Byte)ColorIndex;
            result[Packet.PacketDataOffset + MemoryPresetPacket.RepeatOffset] = (Byte)Repeat;
            return result;
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public override Object Clone()
        {
            var packet = new MemoryPresetPacket() { Index = this.Index };
            Array.Copy(this.Data, packet.Data, this.Data.Length);
            return packet;
        }


        #endregion PUBLIC METHODS

    }

}
