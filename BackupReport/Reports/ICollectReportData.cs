namespace BackupReport.Reports
{
    public interface ICollectReportData<T>
    {
        T Collect(BackupContext context);
    }
}
