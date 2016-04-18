using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BackupReport.Reports.MissingBackup
{
    public class Report : IOutputReport<IList<string>>
    {
        private readonly TextWriter writer;

        public Report(TextWriter writer)
        {
            this.writer = writer;
        }

        public void Output(IList<string> data)
        {
            writer.WriteHeader("Missing Backup");

            data.Select((backupTarget, index) => new { backupTarget, index })
                .Aggregate(
                    writer,
                    (output, item) =>
                    {
                        if (item.index > 0) { output.WriteLine(); }

                        output.Write(item.backupTarget);

                        return output;
                    });
        }
    }
}
