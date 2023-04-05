namespace Karaoke.Core.IO
{

    /// <summary>
    /// Abstracts the low level <see cref="System.IO.File"/> operations to make testing easier.
    /// </summary>
    public class FileUtility : IFileUtility
    {

        #region PUBLIC METHODS


        /// <summary>
        /// Determines whether the specified file exists.
        /// </summary>
        /// <param name="location">The location of the file.</param>
        /// <returns><c>true</c> if the file exists; otherwise <c>false</c></returns>
        public Boolean FileExists(String location)
        {
            var exists = File.Exists(location);
            return exists;
        }

        /// <summary>
        /// Reads all bytes from tge specified file.
        /// </summary>
        /// <param name="location">The location of the file.</param>
        /// <returns>Returns a byte array for the specified file</returns>
        public Byte[] ReadAllBytes(String location) => File.ReadAllBytes(location);

        /// <summary>
        /// Opens a System.IO.FileStream on the specified path, having the specified mode
        /// with read, write, or read/write access and the specified sharing option.
        /// </summary>
        /// <param name="location">The location of the file.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="access">The access.</param>
        /// <param name="share">The share.</param>
        public FileStream Open(String location, FileMode mode, FileAccess access, FileShare share) => File.Open(location, mode, access, share);

        /// <summary>
        /// Gets the file information.
        /// </summary>
        /// <param name="location">The location of the file.</param>
        /// <returns>Returns a <see cref="FileInfo"/> object for the specified file</returns>
        public FileInfo GetFileInfo(String location) => new FileInfo(location);

        /// <summary>
        /// Creates a new file, writes the specified string to the file, and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="location">The location of the file to write to.</param>
        /// <param name="textToWrite">The text to write.</param>
        public void WriteAllText(String location, String textToWrite) => File.WriteAllText(location, textToWrite);

        /// <summary>
        /// Creates a new file, writes the specified bytes to the file, and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="location">The location of the file to write to.</param>
        /// <param name="data">The data.</param>
        public void WriteAllBytes(String location, Byte[] data) => File.WriteAllBytes(location, data);


        /// <summary>
        /// Opens a text file, reads all lines of the file, and then closes the file
        /// </summary>
        /// <param name="location">The file to open for reading.</param>
        /// <returns></returns>
        public String ReadAllText(String location) => File.ReadAllText(location);

        /// <summary>
        /// Opens a text file, reads all lines of the file, and then closes the file.
        /// </summary>
        /// <param name="location">The file to open for reading.</param>
        /// <returns>A string array containing all lines of the file.</returns>
        public String[] ReadAllLines(String location) => File.ReadAllLines(location);

        /// <summary>
        /// Gets the containing folder of the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public String? GetContainingFolder(String fileName)
        {
            var file = new FileInfo(fileName);
            return file.DirectoryName;
        }

        /// <summary>
        /// Gets the type of the path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        /// <see href="http://stackoverflow.com/questions/1395205/better-way-to-check-if-path-is-a-file-or-a-directory"/>
        public PathType GetPathType(String path)
        {
            try
            {
                var attr = File.GetAttributes(path);
                return (attr & FileAttributes.Directory) == FileAttributes.Directory
                    ? PathType.FolderPath
                    : PathType.FilePath;
            }
            catch (Exception)
            {
                return PathType.None;
            }
        }

        /// <summary>
        /// Deletes the file at the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        public void Delete(String path) => File.Delete(path);


        #endregion PUBLIC METHODS

    }

    /// <summary>
    /// Defines an interface that all FileUtility classes must implement.
    /// </summary>
    public interface IFileUtility
    {

        /// <summary>
        /// Determines whether the specified file exists.
        /// </summary>
        /// <param name="location">The location of the file.</param>
        /// <returns><c>true</c> if the file exists; otherwise <c>false</c></returns>
        bool FileExists(String location);

        /// <summary>
        /// Reads all bytes from tge specified file.
        /// </summary>
        /// <param name="location">The location of the file.</param>
        /// <returns>Returns a byte array for the specified file</returns>
        byte[] ReadAllBytes(String location);

        /// <summary>
        /// Opens a text file, reads all lines of the file, and then closes the file
        /// </summary>
        /// <param name="location">The file to open for reading.</param>
        /// <returns></returns>
        String ReadAllText(String location);

        /// <summary>
        /// Gets the file information.
        /// </summary>
        /// <param name="location">The location of the file.</param>
        /// <returns>Returns a <see cref="FileInfo"/> object for the specified file</returns>
        FileInfo GetFileInfo(String location);

        /// <summary>
        /// Creates a new file, writes the specified string to the file, and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="location">The location of the file to write to.</param>
        /// <param name="textToWrite">The text to write.</param>
        void WriteAllText(String location, String textToWrite);

        /// <summary>
        /// Gets the type of the path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        PathType GetPathType(String path);

        /// <summary>
        /// Deletes the file at the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        void Delete(String path);

        /// <summary>
        /// Gets the containing folder of the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        String? GetContainingFolder(String fileName);

        /// <summary>
        /// Gets the file information.
        /// </summary>
        /// <param name="location">The location of the file.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="access">The access.</param>
        /// <param name="share">The share.</param>
        FileStream Open(String location, FileMode mode, FileAccess access, FileShare share);

        /// <summary>
        /// Creates a new file, writes the specified bytes to the file, and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="location">The location of the file to write to.</param>
        /// <param name="data">The data.</param>
        void WriteAllBytes(String location, Byte[] data);

        /// <summary>
        /// Opens a text file, reads all lines of the file, and then closes the file.
        /// </summary>
        /// <param name="location">The file to open for reading.</param>
        /// <returns>A string array containing all lines of the file.</returns>
        String[] ReadAllLines(String location);
    
    }

    /// <summary>
    /// Types of paths
    /// </summary>
    public enum PathType
    {
        /// <summary>
        /// File path
        /// </summary>
        FilePath,
        /// <summary>
        /// Folder path
        /// </summary>
        FolderPath,
        /// <summary>
        /// Not a path.
        /// </summary>
        None
    }

}
