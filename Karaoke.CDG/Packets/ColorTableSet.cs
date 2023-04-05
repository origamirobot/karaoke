using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karaoke.CDG.Packets
{

    /// <summary>
    /// Types of color table sets that can be loaded.
    /// </summary>
    public enum ColorTableSet
    {
        /// <summary>
        /// Color table entries 8-15
        /// </summary>
        High,
        /// <summary>
        /// Color table entries 0-7
        /// </summary>
        Low
    }


    public enum ColorType
    {
        OnColor,
        OffColor
    }

}
