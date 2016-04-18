using System;
using System.Collections.Generic;
using System.Linq;

namespace BackupReport.Reports.MissingBackup
{
    public class Collector : ICollectReportData<IList<string>>
    {
        public IList<string> Collect(BackupContext context)
        {
            return context.BackupTargets.GroupJoin(
                context.BackupManifest,
                backupTarget => backupTarget,
                backupManifest => backupManifest.BackupTarget,
                (backupTarget, matches) => new { backupTarget, matches })
                .Where(item => !item.matches.Any())
                .Select(item => item.backupTarget)
                .ToList();
        }
    }
}
