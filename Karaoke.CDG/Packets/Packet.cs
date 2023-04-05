using Karaoke.CDG.IO;
using Karaoke.CDG.Validation;

namespace Karaoke.CDG.Packets
{

    /// <summary>
    /// Base class for CDG packets.
    /// </summary>
    public abstract class Packet : ICloneable
    {

        #region CONSTANTS


        /// <summary>
        /// The packet size
        /// </summary>
        public const Int32 PacketSize = 24;

        /// <summary>
        /// The packet data size
        /// </summary>
        public const Int32 PacketDataSize = 16;

        /// <summary>
        /// The offset index of where the packet data starts.
        /// </summary>
        public const Int32 PacketDataOffset = 4;

        /// <summary>
        /// The command that marks this packet as a CDG packet.
        /// </summary>
        public const Int32 CdgCommand = 0x09;

        /// <summary>
        /// The offset where the CDG command can be found;
        /// </summary>
        public const Int32 CommandOffset = 0;

        /// <summary>
        /// The offset index of where the CDG instruction can be found.
        /// </summary>
        public const Int32 InstructionOffset = 1;

        /// <summary>
        /// The maximum number of tiled columns in a CDG image.
        /// </summary>
        public const Int32 MaxColumns = 50;

        /// <summary>
        /// The maximum number of tiled rows in a CDG image.
        /// </summary>
        public const Int32 MaxRows = 18;

        /// <summary>
        /// The maximum number of colors in a CDG image.
        /// </summary>
        public const Int32 MaxColors = 15;


        #endregion CONSTANTS

        #region PRIVATE PROPERTIES


        private Int32? _index;


        #endregion PRIVATE PROPERTIES

        #region PUBLIC ACCESSORS


        /// <summary>
        /// Gets or sets the instruction.
        /// </summary>
        public abstract Instruction Instruction { get; }

        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        public virtual Int32? Index
        {
            get { return _index; }
            set
            {
                if (value == _index) return;
                _index = value;
            }
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        public Byte[] Data { get; }

        /// <summary>
        /// Gets the position of this packet in the CDG file.
        /// </summary>
        public TimeSpan Position
        {
            get
            {
                return Index.HasValue
                    ? CdgFile.PacketCountToTimeSpan(Convert.ToInt64(Index))
                    : new TimeSpan();
            }
        }


        #endregion PUBLIC ACCESSORS

        #region CONSTRUCTORS


        /// <summary>
        /// Initializes a new instance of the <see cref="Packet"/> class.
        /// </summary>
        protected Packet()
        {
            Data = new Byte[PacketSize];
        }


        #endregion CONSTRUCTORS

        #region PUBLIC METHODS


        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public virtual Object Clone()
        {
            // IMPLEMENTED IN DERIVED CLASSES
            return null;
        }

        /// <summary>
        /// Converts the data in this packet into a byte array.
        /// </summary>
        /// <returns>A 24 byte array that represents the data in this object.</returns>
        public virtual Byte[] ToByteArray()
        {
            var result = new Byte[Packet.PacketSize];
            result[Packet.CommandOffset] = Instruction == Instruction.Timing ? (Byte)0x00 : (Byte)Packet.CdgCommand;
            result[Packet.InstructionOffset] = (Byte)Instruction;
            return result;
        }

        /// <summary>
        /// Validates the specified rules.
        /// </summary>
        /// <param name="rules">The rules.</param>
        /// <returns></returns>
        public virtual IEnumerable<ValidationResult> Validate(Rules rules) => Enumerable.Empty<ValidationResult>();


        #endregion PUBLIC METHODS

    }

}
