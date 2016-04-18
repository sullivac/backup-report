using System;
using System.IO;

namespace BackupReport.Reports
{
    public class TextWriterLifecycleReport<T> : IOutputReport<T>
    {
        private readonly Func<TextWriter, IOutputReport<T>> createReport;
        private readonly Func<TextWriter> createTextWriter;

        public TextWriterLifecycleReport(Func<TextWriter> createTextWriter, Func<TextWriter, IOutputReport<T>> createReport)
        {
            this.createTextWriter = createTextWriter;
            this.createReport = createReport;
        }

        public void Output(T data)
        {
            using (TextWriter textWriter = createTextWriter())
            {
                createReport(textWriter).Output(data);
            }
        }
    }
}
