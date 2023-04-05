using Karaoke.CDG.IO;

namespace Karaoke.CDG.Packets
{

    /// <summary>
    /// Represents a packet of data that doesn't contain a CDG instruction. The timing of the CDG instructions are essential to make the graphics - i.e. lyrics and sweep - appear in sync with the music. 
    /// The CDG storage format and packet size dictates that the CDG stream should play at a rate of 300 packets per second. In other words a single CDG packet occupies 1/300th of a second on the time line. 
    /// As an example, if the lyrics has to wait for 4 seconds before continuing to stay in sync with the music, the CDG stream will contain 1200 timing packets.
    /// </summary>
    /// <see href="http://www.cdgfix.com/help/3.x/Technical_information/The_CDG_graphics_format.htm"/>
    public class TimingPacket : Packet
    {

        #region PRIVATE PROPERTIES


        private TimeSpan _duration;
        private Int32 _count;


        #endregion PRIVATE PROPERTIES

        #region PUBLIC ACCESSORS


        /// <summary>
        /// Gets or sets the instruction.
        /// </summary>
        public override Instruction Instruction => Instruction.Timing;

        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        public TimeSpan Duration
        {
            get { return _duration; }
            set
            {
                _duration = value;
                RecalculateCount();
            }
        }

        /// <summary>
        /// Gets or sets the end index.
        /// </summary>
        public Int32? EndIndex
        {
            get { return Index + (Count - 1); }
        }

        /// <summary>
        /// Gets the end position.
        /// </summary>
        public TimeSpan EndPosition
        {
            get
            {
                return EndIndex.HasValue
                    ? CdgFile.PacketCountToTimeSpan(Convert.ToInt64(EndIndex))
                    : new TimeSpan();
            }
        }

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        public Int32 Count
        {
            get { return _count; }
            set
            {
                _count = value;
                RecalculateDuration();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is collapsed.
        /// </summary>
        public Boolean IsCollapsed => Count > 1;


        #endregion PUBLIC ACCESSORS

        #region CONSTRUCTORS


        /// <summary>
        /// Initializes a new instance of the <see cref="TimingPacket" /> class.
        /// </summary>
        public TimingPacket()
        {
            Duration = TimeSpan.FromMilliseconds(3.333333D);
            Count = 1;
        }


        #endregion CONSTRUCTORS

        #region PRIVATE METHODS


        /// <summary>
        /// Recalculates the <see cref="Duration"/> property when the <see cref="Count"/> property is changed.
        /// </summary>
        private void RecalculateDuration()
        {
            var milliseconds = Count * CdgFile.PacketsPerMillisecond;
            _duration = TimeSpan.FromMilliseconds(milliseconds);
        }

        /// <summary>
        /// Recalculates the <see cref="Count"/> property when the <see cref="Duration"/> property is changed.
        /// </summary>
        private void RecalculateCount()
        {
            var milliseconds = Duration.TotalMilliseconds;
            _count = Convert.ToInt32(Math.Round(milliseconds * CdgFile.PacketsPerMillisecond));
        }


        #endregion PRIVATE METHODS

        #region PUBLIC METHODS


        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override String ToString() => "Timing Packet";

        /// <summary>
        /// Converts the data in this packet into a byte array.
        /// </summary>
        /// <returns>A 24 byte array that represents the data in this object.</returns>
        public override Byte[] ToByteArray() => new Byte[Packet.PacketSize]; // TODO: This doesn't look correct.

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public override Object Clone()
        {
            var packet = new TimingPacket() { Index = this.Index };
            Array.Copy(this.Data, packet.Data, this.Data.Length);
            return packet;
        }


        #endregion PUBLIC METHODS

    }

}
