using Karaoke.CDG.Packets;

namespace Karaoke.CDG.Validation
{

    public class ValidationResult
    {

        /// <summary>
        /// Gets or sets the packet.
        /// </summary>
        public Packet Packet { get; private set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public String Message { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult"/> class.
        /// </summary>
        /// <param name="packet">The packet.</param>
        /// <param name="message">The message.</param>
        public ValidationResult(Packet packet, String message)
        {
            Packet = packet;
            Message = message;
        }

    }

}
