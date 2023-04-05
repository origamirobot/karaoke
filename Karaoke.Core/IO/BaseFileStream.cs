namespace Karaoke.Core.IO
{

    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseFileStream : IFileStream, IDisposable
    {

        #region PRIVATE PROPERTIES


        private Stream? _file;


        #endregion PRIVATE PROPERTIES

        #region PROTECTED PROPERTIES


        /// <summary>
        /// Gets the file utility.
        /// </summary>
        protected IFileUtility FileUtility { get; }


        #endregion PROTECTED PROPERTIES

        #region PUBLIC ACCESSORS


        /// <summary>
        /// Gets the position.
        /// </summary>
        public Int64 Position => _file?.Position ?? -1;


        #endregion PUBLIC ACCESSORS

        #region CONSTRUCTORS


        /// <summary>
        /// Initializes a new instance of the <see cref="BaseFileStream" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="fileUtility">The file utility.</param>
        protected BaseFileStream(IFileUtility fileUtility)
        {
            FileUtility = fileUtility;
            _file = null;
        }


        #endregion CONSTRUCTORS

        #region PUBLIC METHODS


        /// <summary>
        /// Reads from this stream into the specified buffer.
        /// </summary>
        /// <param name="buffer">The bufer to read into.</param>
        /// <param name="count">The amount of data to read.</param>
        /// <returns></returns>
        public virtual Int32 Read(Byte[] buffer, Int32 count)
        {
            if (_file == null)
                return -1;

            return _file.Read(buffer, 0, count);
        }

        /// <summary>
        /// Writes the specified buffer to this stream.
        /// </summary>
        /// <param name="buffer">The buffer to write.</param>
        /// <param name="count">The amount of data to write.</param>
        /// <returns></returns>
        public virtual Int32 Write(Byte[] buffer, Int32 count)
        {
            if (_file == null)
                return -1;

            _file.Write(buffer, 0, count);
            return 1;
        }

        /// <summary>
        /// Changes the position in the file stream by the specified offset.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="whence">The place to start from.</param>
        /// <returns></returns>
        public virtual Int64 Seek(Int32 offset, SeekOrigin whence)
        {
            if (_file == null)
                return -1;

            return _file.Seek(offset, whence);
        }

        /// <summary>
        /// Checks to see if file stream is at the end of the file.
        /// </summary>
        /// <returns></returns>
        public virtual Boolean Eof()
        {
            return _file?.Position >= _file?.Length;
        }

        /// <summary>
        /// Gets the size of this stream.
        /// </summary>
        /// <returns></returns>
        public virtual Int64 GetSize()
        {
            if (_file == null)
                return -1;

            return _file.Length;
        }

        /// <summary>
        /// Opens the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public virtual Boolean Open(String filename)
        {
            Close();
            _file = FileUtility.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            return (_file != null);
        }

        /// <summary>
        /// Closes the file stream.
        /// </summary>
        public virtual void Close()
        {
            if (_file == null)
                return;

            _file.Close();
            _file.Dispose();
            _file = null;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            Close();
        }


        #endregion PUBLIC METHODS

    }


    /// <summary>
    /// Defines a contract that all File Streams must implement.
    /// </summary>
    public interface IFileStream : IDisposable
    {

        #region PROPERTIES


        /// <summary>
        /// Gets the position the file stream is currently at.
        /// </summary>
        Int64 Position { get; }


        #endregion PROPERTIES

        #region METHODS


        /// <summary>
        /// Reads from this stream into the specified buffer.
        /// </summary>
        /// <param name="buffer">The bufer to read into.</param>
        /// <param name="count">The amount of data to read.</param>
        /// <returns></returns>
        Int32 Read(Byte[] buffer, Int32 count);

        /// <summary>
        /// Writes the specified buffer to this stream.
        /// </summary>
        /// <param name="buffer">The buffer to write.</param>
        /// <param name="count">The amount of data to write.</param>
        /// <returns></returns>
        Int32 Write(Byte[] buffer, Int32 count);

        /// <summary>
        /// Changes the position in the file stream by the specified offset.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="whence">The place to start from.</param>
        /// <returns></returns>
        Int64 Seek(Int32 offset, SeekOrigin whence);

        /// <summary>
        /// Checks to see if file stream is at the end of the file.
        /// </summary>
        /// <returns></returns>
        Boolean Eof();

        /// <summary>
        /// Gets the size of this stream.
        /// </summary>
        /// <returns></returns>
        Int64 GetSize();

        /// <summary>
        /// Opens the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        Boolean Open(String filename);

        /// <summary>
        /// Closes the file stream.
        /// </summary>
        void Close();


        #endregion METHODS

    }


}
