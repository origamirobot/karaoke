using Karaoke.CDG.Packets;

namespace Karaoke.CDG.IO
{

    /// <summary>
    /// 
    /// </summary>
    public class CdgErrorEventArgs : EventArgs
    {

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public String Message { get; set; }

        /// <summary>
        /// Gets or sets the packet.
        /// </summary>
        public Packet Packet { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="CdgErrorEventArgs"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="packet">The packet.</param>
        public CdgErrorEventArgs(String message, Packet packet)
        {
            Message = message;
            Packet = packet;
        }


    }

}
