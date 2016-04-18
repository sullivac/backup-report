using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BackupReport.Reports.BackupTargetSize
{
    public class Report : IOutputReport<IList<DataItem>>
    {
        private const string FileSizeNumberFormat = "#,#.##";
        private readonly TextWriter writer;

        public Report(TextWriter writer)
        {
            if (writer == null) { throw ArgumentIs.Null(nameof(writer)); }

            this.writer = writer;
        }

        public void Output(IList<DataItem> data)
        {
            writer.WriteHeader("Backup Target Sizes");

            var reportTable = new ReportTable<DataItem>();

            reportTable.AddColumn(item => item.BackupTarget, "Backup Target");
            reportTable.AddColumn(item => item.FileSize, FormatFileSize, "Total Size", Justification.Right);

            reportTable.Load(data);
            reportTable.Write(writer);
        }

        private static string FormatFileSize(FileSize value)
        {
            if (value.InGigabytes < 1)
            {
                return $"{value.InMegabytes.ToString(FileSizeNumberFormat)} MB";
            }

            return $"{value.InGigabytes.ToString(FileSizeNumberFormat)} GB";
        }
    }
}
