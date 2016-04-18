using System;
using System.IO;
using System.Linq;

namespace BackupReport
{
    public class FileSystemInfoFileSizeFactory : ICreateFileSize
    {
        private readonly DirectoryInfo root;

        public FileSystemInfoFileSizeFactory(DirectoryInfo root)
        {
            if (root == null) { throw ArgumentIs.Null(nameof(root)); }

            this.root = root;
        }

        public FileSize Create(string path)
        {
            var fullPath = Path.Combine(root.FullName, path);

            return IsADirectory(fullPath) ? FileSizeForDirectory(new DirectoryInfo(fullPath)) : FileSize(new FileInfo(fullPath));
        }

        private static FileSize FileSize(FileInfo file)
        {
            return new FileSize(file.Length);
        }

        private static FileSize FileSizeForDirectory(DirectoryInfo directory)
        {
            return directory.EnumerateFiles("*", SearchOption.AllDirectories)
                .Select(FileSize)
                .Aggregate((left, right) => left + right);
        }

        private static bool IsADirectory(string fullPath)
        {
            return (File.GetAttributes(fullPath) & FileAttributes.Directory) == FileAttributes.Directory;
        }
    }
}
