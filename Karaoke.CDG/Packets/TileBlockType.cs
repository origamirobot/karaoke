namespace Karaoke.CDG.Packets
{

    /// <summary>
    /// Types of tile blocks.
    /// </summary>
    public enum TileBlockType
    {
        /// <summary>
        /// Normal tile block replaces the pixels directly
        /// </summary>
        Normal,
        /// <summary>
        /// XOR tile block replaces a given pixel with the XOR 
        /// of its current colour index and the foreground/background 
        /// colours in the command.
        /// </summary>
        XOR
    }

}
