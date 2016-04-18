namespace BackupReport.Reports.BackupTargetSize
{
    public class DataItem
    {
        public DataItem(string backupTarget, FileSize fileSize)
        {
            if (backupTarget == null) { throw ArgumentIs.Null(nameof(backupTarget)); }
            if (fileSize == null) { throw ArgumentIs.Null(nameof(fileSize)); }

            BackupTarget = backupTarget;
            FileSize = fileSize;
        }

        public string BackupTarget { get; }

        public FileSize FileSize { get; }
    }
}
