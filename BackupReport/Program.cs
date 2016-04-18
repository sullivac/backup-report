using BackupReport.Reports;
using StructureMap;

namespace BackupReport
{
    class Program
    {
        static void Main(string[] args)
        {
            using (IContainer container = new Container(new BackupReportRegistry(args)).CreateChildContainer())
            {
                ReportClient reportClient = container.GetInstance<ReportClient>();

                reportClient.RunAll();
            }
        }
    }
}
