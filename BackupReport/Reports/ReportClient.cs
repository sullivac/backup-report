using System.Collections.Generic;
using System.IO;

namespace BackupReport.Reports
{
    public class ReportClient
    {
        private readonly IRead<IList<BackupManifestItem>> backupManifestItemReader;
        private readonly IRead<IList<string>> backupTargetReader;
        private readonly IList<IRunReport> reportRunners;
        private readonly TextWriter writer;

        public ReportClient(
            IRead<IList<BackupManifestItem>> backupManifestItemReader,
            IRead<IList<string>> backupTargetReader,
            IList<IRunReport> reportRunners,
            TextWriter writer)
        {
            this.backupManifestItemReader = backupManifestItemReader;
            this.backupTargetReader = backupTargetReader;
            this.reportRunners = reportRunners;
            this.writer = writer;
        }

        public void RunAll()
        {
            var context = new BackupContext(backupManifestItemReader.Read(), backupTargetReader.Read());

            for (int index = 0; index < reportRunners.Count; index++)
            {
                if (index > 0) { writer.WriteLine(); }

                reportRunners[index].RunReport(context);
            }

            writer.Flush();
        }
    }
}
