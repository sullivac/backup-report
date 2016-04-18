using System.Collections.Generic;
using System.Linq;

namespace BackupReport.Reports.BackupTargetSize
{
    public class Collector : ICollectReportData<IList<DataItem>>
    {
        private readonly ICreateFileSize fileSizeFactory;

        public Collector(ICreateFileSize fileSizeFactory)
        {
            if (fileSizeFactory == null) { throw ArgumentIs.Null(nameof(fileSizeFactory)); }

            this.fileSizeFactory = fileSizeFactory;
        }

        public IList<DataItem> Collect(BackupContext context)
        {
            List<DataItem> result = context.BackupTargets.Select(DataItem).ToList();

            result.Add(new DataItem("Total", result.Select(item => item.FileSize).Aggregate((left, right) => left + right)));

            return result;
        }

        private DataItem DataItem(string backupTarget)
        {
            return new DataItem(backupTarget, fileSizeFactory.Create(backupTarget));
        }
    }
}
