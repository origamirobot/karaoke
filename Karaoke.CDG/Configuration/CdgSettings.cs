using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karaoke.CDG.Configuration
{

    /// <summary>
    /// 
    /// </summary>
    public class CdgSettings
    {

        /// <summary>
        /// The size in bytes of each CD+G packet.
        /// </summary>
        public const Int32 PacketSize = 24;

        /// <summary>
        /// The size in bytes of each CD+G packet's data payload.
        /// </summary>
        public const Int32 PacketDataSize = 16;

    }

}
