namespace BackupReport.Reports
{
    public class ReportRunner<T> : IRunReport
    {
        private readonly ICollectReportData<T> collector;
        private readonly IOutputReport<T> report;

        public ReportRunner(ICollectReportData<T> collector, IOutputReport<T> report)
        {
            if (collector == null) { throw ArgumentIs.Null(nameof(collector)); }
            if (report == null) { throw ArgumentIs.Null(nameof(report)); }

            this.collector = collector;
            this.report = report;
        }

        public void RunReport(BackupContext context)
        {
            T reportData = collector.Collect(context);

            report.Output(reportData);
        }
    }
}
