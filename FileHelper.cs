using System;
using System.IO;
using System.Threading;

namespace Utils
{
    public static class FileHelper
    {
        private const int pollInterval = 500;

        /// <summary>
        /// Opens a <see cref="System.IO.FileStream"/> on the specified path for reading or writing. If the file is currently locked, this method will block the current thread until the file is available.
        /// </summary>
        /// <param name="path">The file to be opened for writing.</param>
        /// <param name="fileMode">A <see cref="System.IO.FileMode"/> that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
        /// <returns>An unshared FileStream object on the specified path with write access.</returns>
        public static FileStream OpenWhenAvailable(string path, FileMode fileMode)
        {
            return GetFileWhenAvailable(() => File.Open(path, fileMode));
        }

        /// <summary>
        /// Opens a <see cref="System.IO.FileStream"/> on the specified path for reading or writing. If the file is currently locked, this method will block the current thread until the file is available.
        /// </summary>
        /// <param name="path">The file to be opened for writing.</param>
        /// <param name="fileMode">A <see cref="System.IO.FileMode"/> that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
        /// <param name="fileAccess">A <see cref="System.IO.FileAccess"/> value that specifies the operations that can be performed on the file.</param>
        /// <returns>An unshared FileStream object on the specified path with write access.</returns>
        public static FileStream OpenWhenAvailable(string path, FileMode fileMode, FileAccess fileAccess)
        {
            return GetFileWhenAvailable(() => File.Open(path, fileMode, fileAccess));
        }

        /// <summary>
        /// Opens a <see cref="System.IO.FileStream"/> on the specified path for reading or writing. If the file is currently locked, this method will block the current thread until the file is available.
        /// </summary>
        /// <param name="path">The file to be opened for writing.</param>
        /// <param name="fileMode">A <see cref="System.IO.FileMode"/> that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
        /// <param name="fileAccess">A <see cref="System.IO.FileAccess"/> value that specifies the operations that can be performed on the file.</param>
        /// <param name="fileShare">A <see cref="System.IO.FileShare"/> value specifying the type of access other threads have to the file.</param>
        /// <returns>An unshared FileStream object on the specified path with write access.</returns>
        public static FileStream OpenWhenAvailable(string path, FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
        {
            return GetFileWhenAvailable(() => File.Open(path, fileMode, fileAccess, fileShare));
        }

        /// <summary>
        /// Opens an existing file or creates a new file for writing. If the file is currently locked, this method will block the current thread until the file is available.
        /// </summary>
        /// <param name="path">The file to be opened for writing.</param>
        /// <returns>An unshared FileStream object on the specified path with write access.</returns>
        public static FileStream OpenWriteWhenAvailable(string path)
        {
            return GetFileWhenAvailable(() => File.OpenWrite(path));
        }

        /// <summary>
        /// Creates or overwrites a file in the specified path. If the file is currently locked, this method will block the current thread until the file is available.
        /// </summary>
        /// <param name="path">The path and name of the file to create.</param>
        /// <returns>An unshared FileStream object on the specified path with write access.</returns>
        public static FileStream CreateWhenAvailable(string path)
        {
            return GetFileWhenAvailable(() => File.Create(path));
        }

        /// <summary>
        /// Creates or overwrites a file in the specified path. If the file is currently locked, this method will block the current thread until the file is available.
        /// </summary>
        /// <param name="path">The path and name of the file to create.</param>
        /// <returns>An unshared FileStream object on the specified path with write access.</returns>
        private static FileStream GetFileWhenAvailable(Func<FileStream> getFileFunc)
        {
            while (true)
            {
                try
                {
                    return getFileFunc();
                }
                catch (IOException)
                {
                    Thread.Sleep(pollInterval);
                }
            }
        }
    }
}
