namespace Karaoke.CDG.Packets
{

    /// <summary>
    /// Represents an instruction found in a <see cref="Packet"/>
    /// </summary>
    public enum Instruction
    {
        /// <summary>
        /// Provides no instruction. However, allows timing between packets to occur.
        /// </summary>
        Timing,
        /// <summary>
        /// The memory preset instruction sets the screen to a particular color.
        /// </summary>
        MemoryPreset = 1,
        /// <summary>
        /// The border preset instruction sets the border of the screen to a particular color.
        /// </summary>
        BorderPreset = 2,
        /// <summary>
        /// The Tile Block instruction loads a 12 x 6, 2 color tile and displays it normally.
        /// </summary>
        TileBlock = 6,
        /// <summary>
        /// The scroll preset instruction scrolls the image, filling in the new area with a color.
        /// </summary>
        ScrollPreset = 20,
        /// <summary>
        /// The scroll copy instruction scrolls the image, rotating the bits back around.
        /// </summary>
        ScrollCopy = 24,
        /// <summary>
        /// The define transparent color instruction defines a specific color as being transparent;
        /// </summary>
        DefineTransparentColor = 28,
        /// <summary>
        /// The load color table low instruction loads in the lower 8 entries of the color table (entries 0-7).
        /// </summary>
        LoadColorTableLow = 30,
        /// <summary>
        /// The load color table high instruction loads in the upper 8 entries of the color table (entries 8-15).
        /// </summary>
        LoadColorTableHigh = 31,
        /// <summary>
        /// The Tile Block XOR instruction loads a 12 x 6, 2 color tile and displays it using the XOR method.
        /// </summary>
        TileBlockXOR = 38,
    }

}
