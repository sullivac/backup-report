using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BackupReport
{
    public class FileSystemInfoBackupTargetReader : IRead<IList<string>>
    {
        private readonly DirectoryInfo root;

        public FileSystemInfoBackupTargetReader(DirectoryInfo root)
        {
            if (root == null) { throw ArgumentIs.Null(nameof(root)); }

            this.root = root;
        }

        public IList<string> Read()
        {
            return root.EnumerateFileSystemInfos().Select(fileSystemInfo => fileSystemInfo.Name).ToList();
        }
    }
}
