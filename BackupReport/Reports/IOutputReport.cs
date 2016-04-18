namespace BackupReport.Reports
{
    public interface IOutputReport<T>
    {
        void Output(T data);
    }
}
