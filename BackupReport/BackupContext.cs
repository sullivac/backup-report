using System.Collections.Generic;

namespace BackupReport
{
    public class BackupContext
    {
        public BackupContext(IList<BackupManifestItem> backupManifest, IList<string> backupTargets)
        {
            if (backupManifest == null) { throw ArgumentIs.Null(nameof(backupManifest)); }
            if (backupTargets == null) { throw ArgumentIs.Null(nameof(backupTargets)); }

            BackupManifest = backupManifest;
            BackupTargets = backupTargets;
        }

        public IList<BackupManifestItem> BackupManifest { get; }

        public IList<string> BackupTargets { get; }
    }
}
