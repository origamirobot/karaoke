using Karaoke.CDG.Configuration;
using Karaoke.CDG.Packets;

namespace Karaoke.CDG.IO
{

    /// <summary>
    /// CD+G (also known as CD-G, CD+Graphics and TV-Graphics) is an extension of the compact disc standard
    /// that can present low-resolution graphics alongside the audio data.
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/CD%2BG"/>
    public class CdgFile
    {

        #region CONSTANTS


        /// <summary>
        /// The number of CDG packets that appear in a sector.
        /// </summary>
        public const Double PacketsPerSector = 4.0d;

        /// <summary>
        /// The sectors per second. The duration of one packet is 1/300 seconds 
        /// (4 packets per sector, 75 sectors per second)
        /// </summary>
        public const Double SectorsPerSecond = 75.0d;

        /// <summary>
        /// The amount of packets needed to represent 1 second of a CDG file.
        /// </summary>
        public const Double PacketsPerSecond = SectorsPerSecond * PacketsPerSector;

        /// <summary>
        /// The amount of packets needed to represent 1 millisecond of a CDG file.
        /// </summary>
        public const Double PacketsPerMillisecond = PacketsPerSecond / 1000;

        /// <summary>
        /// When searching for the next, or previous packet of a certain type, it helps to limit the
        /// amount of packets to search through to avoid over taxing the operation.
        /// </summary>
        private const Int32 MaxPacketSearch = 1000;


        #endregion CONSTANTS

        #region PRIVATE PROPERTIES


        private Int64 _position;
        private IList<Packet> _packets;


        #endregion PRIVATE PROPERTIES

        #region PUBLIC ACCESSORS


        /// <summary>
        /// Gets the duration time span.
        /// </summary>
        public virtual TimeSpan DurationTimeSpan => TimeSpan.FromMilliseconds(Duration);

        /// <summary>
        /// Gets the position time span.
        /// </summary>
        public TimeSpan PositionTimeSpan => TimeSpan.FromMilliseconds(Position);

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        public String FileName { get; set; }

        /// <summary>
        /// Gets or sets the duration in milliseconds.
        /// </summary>
        public Int64 Duration => (Packets.Count / CdgSettings.PacketSize) * 1000 / 300;

        /// <summary>
        /// Gets or sets the current position in milliseconds.
        /// </summary>
        public Int64 Position
        {
            get => _position;
            set => _position = value;
        }

        /// <summary>
        /// Gets or sets a list of all the data packets in this file.
        /// </summary>
        public IList<Packet> Packets
        {
            get => _packets;
            set => _packets = value;
        }


        #endregion PUBLIC ACCESSORS

        #region CONSTRUCTORS


        /// <summary>
        /// Initializes a new instance of the <see cref="CdgFile" /> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public CdgFile(String fileName) 
        {
            _packets= new List<Packet>();
            FileName = fileName;
        }


        #endregion CONSTRUCTORS

        #region PUBLIC METHODS


        /// <summary>
        /// Converts a number of packets into the amount of seconds that has passed.
        /// </summary>
        /// <param name="count">The number of packets to calculate seconds for.</param>
        /// <returns></returns>
        public static TimeSpan PacketCountToTimeSpan(Int64 count)
        {
            var seconds = (count / PacketsPerSecond);
            var milli = seconds * 1000;
            return TimeSpan.FromMilliseconds(milli);
        }

        /// <summary>
        /// Finds a previous packet in the packet array of the given type starting at the specified position and working backwards.
        /// </summary>
        /// <param name="instruction">The type of the packet to find.</param>
        /// <param name="startPosition">The start position to work backward from.</param>
        /// <returns>The index of where to find the previous packet of the given type.</returns>
        public virtual Int32 FindPrevious(Instruction instruction, Int32 startPosition)
        {
            return FindPrevious(instruction, startPosition, MaxPacketSearch);
        }

        /// <summary>
        /// Finds a previous packet in the packet array of the given type starting at the specified position and working backwards.
        /// </summary>
        /// <param name="instruction">The type of the packet to find.</param>
        /// <param name="startPosition">The start position to work backward from.</param>
        /// <param name="limit">The maximum number of packets to go searching through when looking for this instruction.</param>
        /// <returns>
        /// The index of where to find the previous packet of the given type.
        /// </returns>
        public virtual Int32 FindPrevious(Instruction instruction, Int32 startPosition, Int32 limit)
        {
            var count = 0;
            var result = startPosition;
            if (startPosition <= 0)
                return 0;

            while (result > 0)
            {
                var packet = Packets[result];
                if (packet.Instruction == instruction)
                {
                    if (packet.Instruction != Instruction.MemoryPreset)
                        break;

                    // IF WE'RE LOOKING FOR A MEMORY PRESET PACKET, LOOK FOR THE FIRST ONE IN THE REPEAT SEQUENCE
                    if (packet is MemoryPresetPacket preset && preset.Repeat == 0)
                        break;
                }
                result--;
                count++;
                // LIMIT THE AMOUNT OF PACKETS TO SEARCH THROUGH OR ELSE WE MIGHT BE HERE ALL DAY
                if (count > limit)
                    break;
            }
            return result;
        }

        /// <summary>
        /// Finds the next packet in the packet array of the given type from the current position.
        /// </summary>
        /// <param name="instruction">The type of packet to look for.</param>
        /// <param name="startPosition">The start position.</param>
        /// <returns>
        /// The index of where to find the next packet of the given type. Returns -1 if not found.
        /// </returns>
        public virtual Int32 FindNext(Instruction instruction, Int32 startPosition)
        {
            return FindNext(instruction, startPosition, MaxPacketSearch);
        }

        /// <summary>
        /// Finds the next packet in the packet array of the given type from the current position.
        /// </summary>
        /// <param name="instruction">The type of packet to look for.</param>
        /// <param name="startPosition">The start position.</param>
        /// <param name="limit">The maximum number of packets to go searching through when looking for this instruction.</param>
        /// <returns>
        /// The index of where to find the next packet of the given type. Returns -1 if not found.
        /// </returns>
        public virtual Int32 FindNext(Instruction instruction, Int32 startPosition, Int32 limit)
        {
            var count = 0;
            var result = startPosition;
            if (startPosition < 0 || startPosition >= Packets.Count)
                return 0;

            var found = false;

            while (result < Packets.Count - 1)
            {
                var packet = Packets[result];
                if (packet.Instruction == instruction)
                {
                    if (instruction != Instruction.MemoryPreset)
                    {
                        found = true;
                        break;
                    }

                    if (packet is MemoryPresetPacket memoryPreset && memoryPreset.Repeat == 0)
                    {
                        found = true;
                        break;
                    }
                }
                count++;
                // LIMIT THE AMOUNT OF PACKETS TO SEARCH THROUGH OR ELSE WE MIGHT BE HERE ALL DAY
                if (count > limit)
                    break;
            }
            return found ? result : -1;
        }


        #endregion PUBLIC METHODS


    }

}
