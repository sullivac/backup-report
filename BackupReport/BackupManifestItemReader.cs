using System;
using System.Collections.Generic;
using System.IO;

namespace BackupReport
{
    public class BackupManifestItemReader : IRead<IList<BackupManifestItem>>
    {
        private readonly Action<string> logError;
        private readonly TextReader reader;

        public BackupManifestItemReader(TextReader reader, Action<string> logError)
        {
            if (reader == null) { throw ArgumentIs.Null(nameof(reader)); }
            if (logError == null) { throw ArgumentIs.Null(nameof(logError)); }

            this.reader = reader;
            this.logError = logError;
        }

        public IList<BackupManifestItem> Read()
        {
            var result = new List<BackupManifestItem>();

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                BackupManifestItem.Parse(line).Accept(result.Add, logError);
            }

            return result;
        }
    }
}
