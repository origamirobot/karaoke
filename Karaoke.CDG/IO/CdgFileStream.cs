using Karaoke.Core.IO;

namespace Karaoke.CDG.IO
{

    /// <summary>
    /// Defines a contract that all CD+G File Streams must implement.
    /// </summary>
    public interface ICdgFileStream : IFileStream { }


    /// <summary>
    /// File stream class responsible for reading CD+G files from the file system
    /// </summary>
    public class CdgFileStream : BaseFileStream, ICdgFileStream
    {

        #region CONSTRUCTORS


        /// <summary>
        /// Initializes a new instance of the <see cref="CdgFileStream" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="fileUtility">The file utility.</param>
        public CdgFileStream(
            IFileUtility fileUtility)
            : base(fileUtility) { }


        #endregion CONSTRUCTORS

    }

}
