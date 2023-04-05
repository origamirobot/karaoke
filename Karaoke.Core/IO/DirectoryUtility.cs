namespace Karaoke.Core.IO
{

    /// <summary>
    /// 
    /// </summary>
    public class DirectoryUtility : IDirectoryUtility
    {

        /// <summary>
        /// Retrieves the parent directory of the specified path, including both absolute and relative paths.
        /// </summary>
        /// <param name="path">The path for which to retrieve the parent directory.</param>
        /// <returns>The parent directory, or null if path is the root directory, including the root of a UNC server or share name.</returns>
        public DirectoryInfo? GetParent(String path) => Directory.GetParent(path);

        /// <summary>
        /// Creates all directories and subdirectories in the specified path.
        /// </summary>
        /// <param name="path">The directory path to create.</param>
        /// <returns>
        /// An object that represents the directory for the specified path
        /// </returns>
        public DirectoryInfo CreateDirectory(String path) => Directory.CreateDirectory(path);

        /// <summary>
        /// Determines whether the given path refers to an existing directory on disk.
        /// </summary>
        /// <param name="path">The path to test.</param>
        /// <returns>
        /// true if path refers to an existing directory; otherwise, false.
        /// </returns>
        public Boolean Exists(String path) => Directory.Exists(path);

        /// <summary>
        /// Sets the creation date and time for the specified file or directory.
        /// </summary>
        /// <param name="path">The file or directory for which to set the creation date and time information..</param>
        /// <param name="creationTime">An object that contains the value to set for the creation date and time of path. This value is expressed in local time..</param>
        public void SetCreationTime(String path, DateTime creationTime) => Directory.SetCreationTime(path, creationTime);

        /// <summary>
        /// Sets the creation date and time, in Coordinated Universal Time (UTC) format, for the specified file or directory.
        /// </summary>
        /// <param name="path">The file or directory for which to set the creation date and time information.</param>
        /// <param name="creationTime">An object that contains the value to set for the creation date and time of path. This value is expressed in UTC time.</param>
        public void SetCreationTimeUtc(String path, DateTime creationTime) => Directory.SetCreationTimeUtc(path, creationTime);

        /// <summary>
        /// Gets the creation date and time of a directory.
        /// </summary>
        /// <param name="path">The path of the directory.</param>
        /// <returns>
        /// A structure that is set to the creation date and time for the specified directory.
        /// This value is expressed in local time.
        /// </returns>
        public DateTime GetCreationTime(String path) => Directory.GetCreationTime(path);

        /// <summary>
        /// Gets the creation date and time, in Coordinated Universal Time (UTC) format, of a directory.
        /// </summary>
        /// <param name="path">The path of the directory.</param>
        /// <returns>
        /// A structure that is set to the creation date and time for the specified directory.
        /// This value is expressed in UTC time
        /// </returns>
        public DateTime GetCreationTimeUtc(String path) => Directory.GetCreationTimeUtc(path);

        /// <summary>
        /// Sets the date and time the specified file or directory was last accessed.
        /// </summary>
        /// <param name="path">The file or directory for which to set the access date and time information.</param>
        /// <param name="lastWriteTime">An object that contains the value to set for the access date and time of
        /// path. This value is expressed in local time.</param>
        public void SetLastWriteTime(String path, DateTime lastWriteTime) => Directory.SetLastWriteTime(path, lastWriteTime);

        /// <summary>
        /// Sets the date and time, in Coordinated Universal Time (UTC) format, that
        /// the specified file or directory was last accessed.
        /// </summary>
        /// <param name="path">The file or directory for which to set the access date and time information.</param>
        /// <param name="lastWriteTime">An object that contains the value to set for the access date and time of
        /// path. This value is expressed in UTC time.</param>
        public void SetLastWriteTimeUtc(String path, DateTime lastWriteTime) => Directory.SetLastWriteTimeUtc(path, lastWriteTime);

        /// <summary>
        /// Returns the date and time the specified file or directory was last written to.
        /// </summary>
        /// <param name="path">The file or directory for which to obtain modification date and time information.</param>
        /// <returns>
        /// A structure that is set to the date and time the specified file or directory
        /// was last written to. This value is expressed in local time.
        /// </returns>
        public DateTime GetLastWriteTime(String path) => Directory.GetLastAccessTime(path);

        /// <summary>
        /// Returns the date and time, in Coordinated Universal Time (UTC) format, that
        /// the specified file or directory was last written to.
        /// </summary>
        /// <param name="path">The file or directory for which to obtain modification date and time information.</param>
        /// <returns>
        /// A structure that is set to the date and time the specified file or directory
        /// was last written to. This value is expressed in UTC time
        /// </returns>
        public DateTime GetLastWriteTimeUtc(String path) => Directory.GetLastAccessTimeUtc(path);

        /// <summary>
        /// Sets the date and time the specified file or directory was last accessed.
        /// </summary>
        /// <param name="path">The file or directory for which to set the access date and time information.</param>
        /// <param name="lastAccessTime">An object that contains the value to set for the access date and time of
        /// path. This value is expressed in local time.</param>
        public void SetLastAccessTime(String path, DateTime lastAccessTime) => Directory.SetLastAccessTime(path, lastAccessTime);

        /// <summary>
        /// Sets the date and time, in Coordinated Universal Time (UTC) format, that
        /// the specified file or directory was last accessed.
        /// </summary>
        /// <param name="path">The file or directory for which to set the access date and time information.</param>
        /// <param name="lastAccessTime">An object that contains the value to set for the access date and time of
        /// path. This value is expressed in UTC time.</param>
        public void SetLastAccessTimeUtc(String path, DateTime lastAccessTime) => Directory.SetLastWriteTimeUtc(path, lastAccessTime);

        /// <summary>
        /// Returns the date and time the specified file or directory was last accessed.
        /// </summary>
        /// <param name="path">The file or directory for which to obtain access date and time information.</param>
        /// <returns>
        /// A structure that is set to the date and time the specified file or directory
        /// was last accessed. This value is expressed in local time.
        /// </returns>
        public DateTime GetLastAccessTime(String path) => Directory.GetLastAccessTime(path);

        /// <summary>
        /// Returns the date and time, in Coordinated Universal Time (UTC) format, that
        /// the specified file or directory was last accessed.
        /// </summary>
        /// <param name="path">The file or directory for which to obtain access date and time information.</param>
        /// <returns>
        /// A structure that is set to the date and time the specified file or directory
        /// was last accessed. This value is expressed in UTC time.
        /// </returns>
        public DateTime GetLastAccessTimeUtc(String path) => Directory.GetLastAccessTimeUtc(path);

        /// <summary>
        /// Returns the names of files (including their paths) that match the specified
        /// search pattern in the specified directory, using a value to determine whether
        /// to search subdirectories.
        /// </summary>
        /// <param name="path">The directory to search.</param>
        /// <returns>
        /// An array of the full names (including paths) for the files in the specified
        /// directory that match the specified search pattern and option.
        /// </returns>
        public String[] GetFiles(String path) => Directory.GetFiles(path);

        /// <summary>
        /// Returns the names of files (including their paths) that match the specified
        /// search pattern in the specified directory, using a value to determine whether
        /// to search subdirectories.
        /// </summary>
        /// <param name="path">The directory to search.</param>
        /// <param name="searchPattern">The search string to match against the names of files in path. The parameter
        /// cannot end in two periods ("..") or contain two periods ("..") followed by
        /// System.IO.Path.DirectorySeparatorChar or System.IO.Path.AltDirectorySeparatorChar,
        /// nor can it contain any of the characters in System.IO.Path.InvalidPathChars.</param>
        /// <returns>
        /// An array of the full names (including paths) for the files in the specified
        /// directory that match the specified search pattern and option.
        /// </returns>
        public String[] GetFiles(String path, String searchPattern) => Directory.GetFiles(path, searchPattern);

        /// <summary>
        /// Returns the names of files (including their paths) that match the specified
        /// search pattern in the specified directory, using a value to determine whether
        /// to search subdirectories.
        /// </summary>
        /// <param name="path">The directory to search.</param>
        /// <param name="searchPattern">The search string to match against the names of files in path. The parameter
        /// cannot end in two periods ("..") or contain two periods ("..") followed by
        /// System.IO.Path.DirectorySeparatorChar or System.IO.Path.AltDirectorySeparatorChar,
        /// nor can it contain any of the characters in System.IO.Path.InvalidPathChars.</param>
        /// <param name="searchOption">The search option
        /// One of the enumeration values that specifies whether the search operation
        /// should include all subdirectories or only the current directory.</param>
        /// <returns>
        /// An array of the full names (including paths) for the files in the specified
        /// directory that match the specified search pattern and option.
        /// </returns>
        public String[] GetFiles(String path, String searchPattern, SearchOption searchOption) => Directory.GetFiles(path, searchPattern, searchOption);

        /// <summary>
        /// Gets the names of the subdirectories (including their paths) that match the
        /// specified search pattern in the current directory, and optionally searches
        /// subdirectories.
        /// </summary>
        /// <param name="path">The path to search.</param>
        /// <returns>
        /// An array of the full names (including paths) of the subdirectories that match
        /// the search pattern
        /// </returns>
        public String[] GetDirectories(String path) => Directory.GetDirectories(path);

        /// <summary>
        /// Gets the names of the subdirectories (including their paths) that match the
        /// specified search pattern in the current directory, and optionally searches
        /// subdirectories.
        /// </summary>
        /// <param name="path">The path to search.</param>
        /// <param name="searchPattern">The search string to match against the names of files in path. The parameter
        /// cannot end in two periods ("..") or contain two periods ("..") followed by
        /// System.IO.Path.DirectorySeparatorChar or System.IO.Path.AltDirectorySeparatorChar,
        /// nor can it contain any of the characters in System.IO.Path.InvalidPathChars.</param>
        /// <returns>
        /// An array of the full names (including paths) of the subdirectories that match
        /// the search pattern
        /// </returns>
        public String[] GetDirectories(String path, String searchPattern) => Directory.GetDirectories(path, searchPattern);

        /// <summary>
        /// Gets the names of the subdirectories (including their paths) that match the
        /// specified search pattern in the current directory, and optionally searches
        /// subdirectories.
        /// </summary>
        /// <param name="path">The path to search.</param>
        /// <param name="searchPattern">The search string to match against the names of files in path. The parameter
        /// cannot end in two periods ("..") or contain two periods ("..") followed by
        /// System.IO.Path.DirectorySeparatorChar or System.IO.Path.AltDirectorySeparatorChar,
        /// nor can it contain any of the characters in System.IO.Path.InvalidPathChars.</param>
        /// <param name="searchOption">One of the enumeration values that specifies whether the search operation
        /// should include all subdirectories or only the current directory.</param>
        /// <returns>
        /// An array of the full names (including paths) of the subdirectories that match
        /// the search pattern
        /// </returns>
        public String[] GetDirectories(String path, String searchPattern, SearchOption searchOption) => Directory.GetDirectories(path, searchPattern, searchOption);

        /// <summary>
        /// Gets an array of all the file names and directory names that match a search
        /// pattern in a specified path, and optionally searches subdirectories.
        /// </summary>
        /// <param name="path">The directory to search.</param>
        /// <returns>
        /// An array of file system entries that match the specified search criteria.
        /// </returns>
        public String[] GetFileSystemEntries(String path) => Directory.GetFileSystemEntries(path);

        /// <summary>
        /// Gets an array of all the file names and directory names that match a search
        /// pattern in a specified path, and optionally searches subdirectories.
        /// </summary>
        /// <param name="path">The directory to search.</param>
        /// <param name="searchPattern">The string used to search for all files or directories that match its search
        /// pattern. The default pattern is for all files and directories: "*".</param>
        /// <returns>
        /// An array of file system entries that match the specified search criteria.
        /// </returns>
        public String[] GetFileSystemEntries(String path, String searchPattern) => Directory.GetFileSystemEntries(path, searchPattern);

        /// <summary>
        /// Gets an array of all the file names and directory names that match a search
        /// pattern in a specified path, and optionally searches subdirectories.
        /// </summary>
        /// <param name="path">The directory to search.</param>
        /// <param name="searchPattern">The string used to search for all files or directories that match its search
        /// pattern. The default pattern is for all files and directories: "*".</param>
        /// <param name="searchOption">One of the enumeration values that specifies whether the search operation
        /// should include only the current directory or should include all subdirectories.The
        /// default value is System.IO.SearchOption.TopDirectoryOnly.</param>
        /// <returns>
        /// An array of file system entries that match the specified search criteria.
        /// </returns>
        public String[] GetFileSystemEntries(String path, String searchPattern, SearchOption searchOption) => Directory.GetFileSystemEntries(path, searchPattern, searchOption);

        /// <summary>
        /// Returns an enumerable collection of directory names that match a search pattern in a specified path, and optionally searches subdirectories.
        /// </summary>
        /// <param name="path">The directory to search.</param>
        /// <returns>
        /// An enumerable collection of the full names (including paths) for the directories
        /// in the directory specified by path and that match the specified search pattern
        /// and option.
        /// </returns>
        public IEnumerable<String> EnumerateDirectories(String path) => Directory.EnumerateDirectories(path);

        /// <summary>
        /// Returns an enumerable collection of directory names that match a search pattern in a specified path, and optionally searches subdirectories.
        /// </summary>
        /// <param name="path">The directory to search.</param>
        /// <param name="searchPattern">The search string to match against the names of directories in path.</param>
        /// <returns>
        /// An enumerable collection of the full names (including paths) for the directories
        /// in the directory specified by path and that match the specified search pattern
        /// and option.
        /// </returns>
        public IEnumerable<String> EnumerateDirectories(String path, String searchPattern) => Directory.EnumerateDirectories(path, searchPattern);

        /// <summary>
        /// Returns an enumerable collection of directory names that match a search pattern in a specified path, and optionally searches subdirectories.
        /// </summary>
        /// <param name="path">The directory to search.</param>
        /// <param name="searchPattern">The search string to match against the names of directories in path.</param>
        /// <param name="searchOption">One of the enumeration values that specifies whether the search operation
        /// should include only the current directory or should include all subdirectories.The
        /// default value is System.IO.SearchOption.TopDirectoryOnly.</param>
        /// <returns>
        /// An enumerable collection of the full names (including paths) for the directories
        /// in the directory specified by path and that match the specified search pattern
        /// and option.
        /// </returns>
        public IEnumerable<String> EnumerateDirectories(String path, String searchPattern, SearchOption searchOption) => Directory.EnumerateDirectories(path, searchPattern, searchOption);

        public IEnumerable<String> EnumerateFiles(String path) => Directory.EnumerateFiles(path);

        /// <summary>
        /// Returns an enumerable collection of file names that match a search pattern in a specified path, and optionally searches subdirectories.
        /// </summary>
        /// <param name="path">The directory to search.</param>
        /// <param name="searchPattern">The search string to match against the names of directories in path.</param>
        /// <returns>
        /// An enumerable collection of the full names (including paths) for the files
        /// in the directory specified by path and that match the specified search pattern
        /// and option
        /// </returns>
        public IEnumerable<String> EnumerateFiles(String path, String searchPattern) => Directory.EnumerateFiles(path, searchPattern);

        /// <summary>
        /// Returns an enumerable collection of file names that match a search pattern in a specified path, and optionally searches subdirectories.
        /// </summary>
        /// <param name="path">The directory to search.</param>
        /// <param name="searchPattern">The search string to match against the names of directories in path.</param>
        /// <param name="searchOption">One of the enumeration values that specifies whether the search operation
        /// should include only the current directory or should include all subdirectories. The
        /// default value is System.IO.SearchOption.TopDirectoryOnly.</param>
        /// <returns>
        /// An enumerable collection of the full names (including paths) for the files
        /// in the directory specified by path and that match the specified search pattern
        /// and option
        /// </returns>
        public IEnumerable<String> EnumerateFiles(String path, String searchPattern, SearchOption searchOption) => Directory.EnumerateFiles(path, searchPattern, searchOption);

        /// <summary>
        /// Returns an enumerable collection of file names and directory names that match a search pattern in a specified path, and optionally searches subdirectories.
        /// </summary>
        /// <param name="path">The directory to search.</param>
        /// <returns>
        /// An enumerable collection of file-system entries in the directory specified by path and that match the specified search pattern and option.
        /// </returns>
        public IEnumerable<String> EnumerateFileSystemEntries(String path) => Directory.EnumerateFileSystemEntries(path);

        /// <summary>
        /// Returns an enumerable collection of file names and directory names that match a search pattern in a specified path, and optionally searches subdirectories.
        /// </summary>
        /// <param name="path">The directory to search.</param>
        /// <param name="searchPattern">The search string to match against the names of directories in path.</param>
        /// <returns>
        /// An enumerable collection of file-system entries in the directory specified by path and that match the specified search pattern and option.
        /// </returns>
        public IEnumerable<String> EnumerateFileSystemEntries(String path, String searchPattern) => Directory.EnumerateFileSystemEntries(path, searchPattern);

        /// <summary>
        /// Returns an enumerable collection of file names and directory names that match a search pattern in a specified path, and optionally searches subdirectories.
        /// </summary>
        /// <param name="path">The directory to search.</param>
        /// <param name="searchPattern">The search string to match against the names of directories in path.</param>
        /// <param name="searchOption">One of the enumeration values that specifies whether the search operation
        /// should include only the current directory or should include all subdirectories.The
        /// default value is System.IO.SearchOption.TopDirectoryOnly.</param>
        /// <returns>
        /// An enumerable collection of file-system entries in the directory specified by path and that match the specified search pattern and option.
        /// </returns>
        public IEnumerable<String> EnumerateFileSystemEntries(String path, String searchPattern, SearchOption searchOption) => Directory.EnumerateFileSystemEntries(path, searchPattern, searchOption);

        /// <summary>
        /// Returns the volume information, root information, or both for the specified path.
        /// </summary>
        /// <param name="path">The path of a file or directory.</param>
        /// <returns>
        /// A string that contains the volume information, root information, or both for the specified path.
        /// </returns>
        public String GetDirectoryRoot(String path) => Directory.GetDirectoryRoot(path);

        /// <summary>
        /// Gets the current working directory of the application.
        /// </summary>
        /// <returns>
        /// A string that contains the path of the current working directory, and does
        /// not end with a backslash (\).
        /// </returns>
        public String GetCurrentDirectory() => Directory.GetCurrentDirectory();

        /// <summary>
        /// Sets the application's current working directory to the specified directory.
        /// </summary>
        /// <param name="path">The path to which the current working directory is set.</param>
        public void SetCurrentDirectory(String path) => Directory.SetCurrentDirectory(path);

        /// <summary>
        /// Moves a file or a directory and its contents to a new location.
        /// </summary>
        /// <param name="sourceDirName">The path of the file or directory to move.</param>
        /// <param name="destDirName">The path to the new location for sourceDirName. If sourceDirName is a file, then destDirName must also be a file name.</param>
        public void Move(String sourceDirName, String destDirName) => Directory.Move(sourceDirName, destDirName);

        /// <summary>
        /// Deletes the specified directory and, if indicated, any subdirectories and files in the directory.
        /// </summary>
        /// <param name="path">The name of the directory to remove.</param>
        public void Delete(String path) => Directory.Delete(path);

        /// <summary>
        /// Deletes the specified directory and, if indicated, any subdirectories and files in the directory.
        /// </summary>
        /// <param name="path">The name of the directory to remove.</param>
        /// <param name="recursive">true to remove directories, subdirectories, and files in path; otherwise, false</param>
        public void Delete(String path, bool recursive) => Directory.Delete(path, recursive);

    }


    /// <summary>
    /// Defines an interface that all DirectoryUtility classes must implement. Acts as a wrapper around the <see cref="System.IO.Directory"/> class for testability.
    /// </summary>
    public interface IDirectoryUtility
    {

        /// <summary>
        /// Retrieves the parent directory of the specified path, including both absolute and relative paths.
        /// </summary>
        /// <param name="path">The path for which to retrieve the parent directory.</param>
        /// <returns>The parent directory, or null if path is the root directory, including the root of a UNC server or share name.</returns>
        DirectoryInfo? GetParent(String path);

        /// <summary>
        /// Creates all directories and subdirectories in the specified path.
        /// </summary>
        /// <param name="path">The directory path to create.</param>
        /// <returns>An object that represents the directory for the specified path</returns>
        DirectoryInfo CreateDirectory(String path);

        /// <summary>
        /// Determines whether the given path refers to an existing directory on disk.
        /// </summary>
        /// <param name="path">The path to test.</param>
        /// <returns>true if path refers to an existing directory; otherwise, false.</returns>
        Boolean Exists(String path);

        /// <summary>
        /// Sets the creation date and time for the specified file or directory.
        /// </summary>
        /// <param name="path">The file or directory for which to set the creation date and time information..</param>
        /// <param name="creationTime">An object that contains the value to set for the creation date and time of path. This value is expressed in local time..</param>
        void SetCreationTime(String path, DateTime creationTime);

        /// <summary>
        /// Sets the creation date and time, in Coordinated Universal Time (UTC) format, for the specified file or directory.
        /// </summary>
        /// <param name="path">The file or directory for which to set the creation date and time information.</param>
        /// <param name="creationTime">An object that contains the value to set for the creation date and time of path. This value is expressed in UTC time.</param>
        void SetCreationTimeUtc(String path, DateTime creationTime);

        /// <summary>
        /// Gets the creation date and time of a directory.
        /// </summary>
        /// <param name="path">The path of the directory.</param>
        /// <returns>
        /// A structure that is set to the creation date and time for the specified directory.
        /// This value is expressed in local time.
        /// </returns>
        DateTime GetCreationTime(String path);

        /// <summary>
        /// Gets the creation date and time, in Coordinated Universal Time (UTC) format, of a directory.
        /// </summary>
        /// <param name="path">The path of the directory.</param>
        /// <returns>
        /// A structure that is set to the creation date and time for the specified directory.
        /// This value is expressed in UTC time
        /// </returns>
        DateTime GetCreationTimeUtc(String path);

        /// <summary>
        /// Sets the date and time the specified file or directory was last accessed.
        /// </summary>
        /// <param name="path">The file or directory for which to set the access date and time information.</param>
        /// <param name="lastWriteTime">
        /// An object that contains the value to set for the access date and time of
        /// path. This value is expressed in local time.
        /// </param>
        void SetLastWriteTime(String path, DateTime lastWriteTime);

        /// <summary>
        /// Sets the date and time, in Coordinated Universal Time (UTC) format, that
        /// the specified file or directory was last accessed.
        /// </summary>
        /// <param name="path">The file or directory for which to set the access date and time information.</param>
        /// <param name="lastWriteTime">
        /// An object that contains the value to set for the access date and time of
        /// path. This value is expressed in UTC time.
        /// </param>
        void SetLastWriteTimeUtc(String path, DateTime lastWriteTime);

        /// <summary>
        /// Returns the date and time the specified file or directory was last written to.
        /// </summary>
        /// <param name="path">The file or directory for which to obtain modification date and time information.</param>
        /// <returns>
        /// A structure that is set to the date and time the specified file or directory
        /// was last written to. This value is expressed in local time.
        /// </returns>
        DateTime GetLastWriteTime(String path);

        /// <summary>
        /// Returns the date and time, in Coordinated Universal Time (UTC) format, that
        /// the specified file or directory was last written to.
        /// </summary>
        /// <param name="path">The file or directory for which to obtain modification date and time information.</param>
        /// <returns>
        /// A structure that is set to the date and time the specified file or directory
        /// was last written to. This value is expressed in UTC time
        /// </returns>
        DateTime GetLastWriteTimeUtc(String path);

        /// <summary>
        /// Sets the date and time the specified file or directory was last accessed.
        /// </summary>
        /// <param name="path">The file or directory for which to set the access date and time information.</param>
        /// <param name="lastAccessTime">
        /// An object that contains the value to set for the access date and time of
        /// path. This value is expressed in local time.
        /// </param>
        void SetLastAccessTime(String path, DateTime lastAccessTime);

        /// <summary>
        /// Sets the date and time, in Coordinated Universal Time (UTC) format, that
        /// the specified file or directory was last accessed.
        /// </summary>
        /// <param name="path">The file or directory for which to set the access date and time information.</param>
        /// <param name="lastAccessTime">
        /// An object that contains the value to set for the access date and time of
        /// path. This value is expressed in UTC time.
        /// </param>
        void SetLastAccessTimeUtc(String path, DateTime lastAccessTime);

        /// <summary>
        /// Returns the date and time the specified file or directory was last accessed.
        /// </summary>
        /// <param name="path">The file or directory for which to obtain access date and time information.</param>
        /// <returns>
        /// A structure that is set to the date and time the specified file or directory
        /// was last accessed. This value is expressed in local time.
        /// </returns>
        DateTime GetLastAccessTime(String path);

        /// <summary>
        /// Returns the date and time, in Coordinated Universal Time (UTC) format, that
        /// the specified file or directory was last accessed.
        /// </summary>
        /// <param name="path">The file or directory for which to obtain access date and time information.</param>
        /// <returns>
        /// A structure that is set to the date and time the specified file or directory
        /// was last accessed. This value is expressed in UTC time.
        /// </returns>
        DateTime GetLastAccessTimeUtc(String path);

        /// <summary>
        /// Returns the names of files (including their paths) that match the specified
        /// search pattern in the specified directory, using a value to determine whether
        /// to search subdirectories.
        /// </summary>
        /// <param name="path">The directory to search.</param>
        /// <returns>
        /// An array of the full names (including paths) for the files in the specified
        /// directory that match the specified search pattern and option.
        /// </returns>
        String[] GetFiles(String path);

        /// <summary>
        /// Returns the names of files (including their paths) that match the specified
        /// search pattern in the specified directory, using a value to determine whether
        /// to search subdirectories.
        /// </summary>
        /// <param name="path">The directory to search.</param>
        /// <param name="searchPattern">The search string to match against the names of files in path. The parameter
        /// cannot end in two periods ("..") or contain two periods ("..") followed by
        /// System.IO.Path.DirectorySeparatorChar or System.IO.Path.AltDirectorySeparatorChar,
        /// nor can it contain any of the characters in System.IO.Path.InvalidPathChars.</param>
        /// <returns>
        /// An array of the full names (including paths) for the files in the specified
        /// directory that match the specified search pattern and option.
        /// </returns>
        String[] GetFiles(String path, String searchPattern);

        /// <summary>
        /// Returns the names of files (including their paths) that match the specified
        /// search pattern in the specified directory, using a value to determine whether
        /// to search subdirectories.
        /// </summary>
        /// <param name="path">The directory to search.</param>
        /// <param name="searchPattern">
        /// The search string to match against the names of files in path. The parameter
        /// cannot end in two periods ("..") or contain two periods ("..") followed by
        /// System.IO.Path.DirectorySeparatorChar or System.IO.Path.AltDirectorySeparatorChar,
        /// nor can it contain any of the characters in System.IO.Path.InvalidPathChars.
        /// </param>
        /// <param name="searchOption">The search option
        /// One of the enumeration values that specifies whether the search operation
        /// should include all subdirectories or only the current directory.
        /// </param>
        /// <returns>
        /// An array of the full names (including paths) for the files in the specified
        /// directory that match the specified search pattern and option.
        /// </returns>
        String[] GetFiles(String path, String searchPattern, SearchOption searchOption);

        /// <summary>
        /// Gets the names of the subdirectories (including their paths) that match the
        /// specified search pattern in the current directory, and optionally searches
        /// subdirectories.
        /// </summary>
        /// <param name="path">The path to search.</param>
        /// <returns>
        /// An array of the full names (including paths) of the subdirectories that match
        /// the search pattern
        /// </returns>
        String[] GetDirectories(String path);

        /// <summary>
        /// Gets the names of the subdirectories (including their paths) that match the
        /// specified search pattern in the current directory, and optionally searches
        /// subdirectories.
        /// </summary>
        /// <param name="path">The path to search.</param>
        /// <param name="searchPattern">The search string to match against the names of files in path. The parameter
        /// cannot end in two periods ("..") or contain two periods ("..") followed by
        /// System.IO.Path.DirectorySeparatorChar or System.IO.Path.AltDirectorySeparatorChar,
        /// nor can it contain any of the characters in System.IO.Path.InvalidPathChars.</param>
        /// <returns>
        /// An array of the full names (including paths) of the subdirectories that match
        /// the search pattern
        /// </returns>
        String[] GetDirectories(String path, String searchPattern);

        /// <summary>
        /// Gets the names of the subdirectories (including their paths) that match the
        /// specified search pattern in the current directory, and optionally searches
        /// subdirectories.
        /// </summary>
        /// <param name="path">The path to search.</param>
        /// <param name="searchPattern">The search string to match against the names of files in path. The parameter
        /// cannot end in two periods ("..") or contain two periods ("..") followed by
        /// System.IO.Path.DirectorySeparatorChar or System.IO.Path.AltDirectorySeparatorChar,
        /// nor can it contain any of the characters in System.IO.Path.InvalidPathChars.</param>
        /// <param name="searchOption">
        /// One of the enumeration values that specifies whether the search operation
        /// should include all subdirectories or only the current directory.
        /// </param>
        /// <returns>
        /// An array of the full names (including paths) of the subdirectories that match
        /// the search pattern
        /// </returns>
        String[] GetDirectories(String path, String searchPattern, SearchOption searchOption);

        /// <summary>
        /// Gets an array of all the file names and directory names that match a search
        /// pattern in a specified path, and optionally searches subdirectories.
        /// </summary>
        /// <param name="path">The directory to search.</param>
        /// <returns>
        /// An array of file system entries that match the specified search criteria.
        /// </returns>
        String[] GetFileSystemEntries(String path);

        /// <summary>
        /// Gets an array of all the file names and directory names that match a search
        /// pattern in a specified path, and optionally searches subdirectories.
        /// </summary>
        /// <param name="path">The directory to search.</param>
        /// <param name="searchPattern">The string used to search for all files or directories that match its search
        /// pattern. The default pattern is for all files and directories: "*".</param>
        /// <returns>
        /// An array of file system entries that match the specified search criteria.
        /// </returns>
        String[] GetFileSystemEntries(String path, String searchPattern);

        /// <summary>
        /// Gets an array of all the file names and directory names that match a search
        /// pattern in a specified path, and optionally searches subdirectories.
        /// </summary>
        /// <param name="path">The directory to search.</param>
        /// <param name="searchPattern">
        /// The string used to search for all files or directories that match its search
        /// pattern. The default pattern is for all files and directories: "*".
        /// </param>
        /// <param name="searchOption">
        /// One of the enumeration values that specifies whether the search operation
        /// should include only the current directory or should include all subdirectories.The
        /// default value is System.IO.SearchOption.TopDirectoryOnly.
        /// </param>
        /// <returns>An array of file system entries that match the specified search criteria.</returns>
        String[] GetFileSystemEntries(String path, String searchPattern, SearchOption searchOption);

        /// <summary>
        /// Returns an enumerable collection of directory names that match a search pattern in a specified path, and optionally searches subdirectories.
        /// </summary>
        /// <param name="path">The directory to search.</param>
        /// <returns>
        /// An enumerable collection of the full names (including paths) for the directories
        /// in the directory specified by path and that match the specified search pattern
        /// and option.
        /// </returns>
        IEnumerable<String> EnumerateDirectories(String path);

        /// <summary>
        /// Returns an enumerable collection of directory names that match a search pattern in a specified path, and optionally searches subdirectories.
        /// </summary>
        /// <param name="path">The directory to search.</param>
        /// <param name="searchPattern">The search string to match against the names of directories in path.</param>
        /// <returns>
        /// An enumerable collection of the full names (including paths) for the directories
        /// in the directory specified by path and that match the specified search pattern
        /// and option.
        /// </returns>
        IEnumerable<String> EnumerateDirectories(String path, String searchPattern);

        /// <summary>
        /// Returns an enumerable collection of directory names that match a search pattern in a specified path, and optionally searches subdirectories.
        /// </summary>
        /// <param name="path">The directory to search.</param>
        /// <param name="searchPattern">The search string to match against the names of directories in path.</param>
        /// <param name="searchOption">
        /// One of the enumeration values that specifies whether the search operation 
        /// should include only the current directory or should include all subdirectories.The
        /// default value is System.IO.SearchOption.TopDirectoryOnly.
        /// </param>
        /// <returns>
        /// An enumerable collection of the full names (including paths) for the directories
        /// in the directory specified by path and that match the specified search pattern
        /// and option.
        /// </returns>
        IEnumerable<String> EnumerateDirectories(String path, String searchPattern, SearchOption searchOption);

        /// <summary>
        /// Returns an enumerable collection of file names that match a search pattern in a specified path, and optionally searches subdirectories.
        /// </summary>
        /// <param name="path">The directory to search.</param>
        /// <returns>
        /// An enumerable collection of the full names (including paths) for the files
        /// in the directory specified by path and that match the specified search pattern
        /// and option
        /// </returns>
        IEnumerable<String> EnumerateFiles(String path);

        /// <summary>
        /// Returns an enumerable collection of file names that match a search pattern in a specified path, and optionally searches subdirectories.
        /// </summary>
        /// <param name="path">The directory to search.</param>
        /// <param name="searchPattern">The search string to match against the names of directories in path.</param>
        /// <returns>
        /// An enumerable collection of the full names (including paths) for the files
        /// in the directory specified by path and that match the specified search pattern
        /// and option
        /// </returns>
        IEnumerable<String> EnumerateFiles(String path, String searchPattern);

        /// <summary>
        /// Returns an enumerable collection of file names that match a search pattern in a specified path, and optionally searches subdirectories.
        /// </summary>
        /// <param name="path">The directory to search.</param>
        /// <param name="searchPattern">The search string to match against the names of directories in path.</param>
        /// <param name="searchOption">
        /// One of the enumeration values that specifies whether the search operation
        /// should include only the current directory or should include all subdirectories. The
        /// default value is System.IO.SearchOption.TopDirectoryOnly.
        /// </param>
        /// <returns>
        /// An enumerable collection of the full names (including paths) for the files
        /// in the directory specified by path and that match the specified search pattern
        /// and option
        /// </returns>
        IEnumerable<String> EnumerateFiles(String path, String searchPattern, SearchOption searchOption);

        /// <summary>
        /// Returns an enumerable collection of file names and directory names that match a search pattern in a specified path, and optionally searches subdirectories.
        /// </summary>
        /// <param name="path">The directory to search.</param>
        /// <returns>
        /// An enumerable collection of file-system entries in the directory specified by path and that match the specified search pattern and option.
        /// </returns>
        IEnumerable<String> EnumerateFileSystemEntries(String path);

        /// <summary>
        /// Returns an enumerable collection of file names and directory names that match a search pattern in a specified path, and optionally searches subdirectories.
        /// </summary>
        /// <param name="path">The directory to search.</param>
        /// <param name="searchPattern">The search string to match against the names of directories in path.</param>
        /// <returns>
        /// An enumerable collection of file-system entries in the directory specified by path and that match the specified search pattern and option.
        /// </returns>
        IEnumerable<String> EnumerateFileSystemEntries(String path, String searchPattern);

        /// <summary>
        /// Returns an enumerable collection of file names and directory names that match a search pattern in a specified path, and optionally searches subdirectories.
        /// </summary>
        /// <param name="path">The directory to search.</param>
        /// <param name="searchPattern">The search string to match against the names of directories in path.</param>
        /// <param name="searchOption">
        /// One of the enumeration values that specifies whether the search operation
        /// should include only the current directory or should include all subdirectories.The
        /// default value is System.IO.SearchOption.TopDirectoryOnly.
        /// </param>
        /// <returns>An enumerable collection of file-system entries in the directory specified by path and that match the specified search pattern and option.</returns>
        IEnumerable<String> EnumerateFileSystemEntries(String path, String searchPattern, SearchOption searchOption);

        /// <summary>
        /// Returns the volume information, root information, or both for the specified path.
        /// </summary>
        /// <param name="path">The path of a file or directory.</param>
        /// <returns>A string that contains the volume information, root information, or both for the specified path.</returns>
        String GetDirectoryRoot(String path);

        /// <summary>
        /// Gets the current working directory of the application.
        /// </summary>
        /// <returns>
        /// A string that contains the path of the current working directory, and does
        /// not end with a backslash (\).
        /// </returns>
        String GetCurrentDirectory();

        /// <summary>
        /// Sets the application's current working directory to the specified directory.
        /// </summary>
        /// <param name="path">The path to which the current working directory is set.</param>
        void SetCurrentDirectory(String path);

        /// <summary>
        /// Moves a file or a directory and its contents to a new location.
        /// </summary>
        /// <param name="sourceDirName">The path of the file or directory to move.</param>
        /// <param name="destDirName">The path to the new location for sourceDirName. If sourceDirName is a file, then destDirName must also be a file name.</param>
        void Move(String sourceDirName, String destDirName);

        /// <summary>
        /// Deletes the specified directory and, if indicated, any subdirectories and files in the directory.
        /// </summary>
        /// <param name="path">The name of the directory to remove.</param>
        void Delete(String path);

        /// <summary>
        /// Deletes the specified directory and, if indicated, any subdirectories and files in the directory.
        /// </summary>
        /// <param name="path">The name of the directory to remove.</param>
        /// <param name="recursive">true to remove directories, subdirectories, and files in path; otherwise, false</param>
        void Delete(String path, bool recursive);

    }



}
