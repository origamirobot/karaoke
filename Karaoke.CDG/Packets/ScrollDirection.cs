namespace Karaoke.CDG.Packets
{

    /// <summary>
    /// The directions for horizontal scrolling.
    /// </summary>
    public enum HorizontalScrollDirection
    {
        /// <summary>
        /// No horizontal scroll instruction
        /// </summary>
        None,
        /// <summary>
        /// Scroll 6 pixels right
        /// </summary>
        Right,
        /// <summary>
        /// Scroll 6 pixels left
        /// </summary>
        Left
    }

    /// <summary>
    /// The directions for vertical scrolling.
    /// </summary>
    public enum VerticalScrollDirection
    {
        /// <summary>
        /// No vertical scroll instruction
        /// </summary>
        None,
        /// <summary>
        /// Scroll 12 pixels down
        /// </summary>
        Down,
        /// <summary>
        /// Scroll 12 pixels up.
        /// </summary>
        Up,
    }

}
