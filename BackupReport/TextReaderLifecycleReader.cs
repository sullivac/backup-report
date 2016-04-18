using System;
using System.IO;

namespace BackupReport
{
    public class TextReaderLifecycleReader<T> : IRead<T>
    {
        private readonly Func<TextReader, IRead<T>> createReader;
        private readonly Func<TextReader> createTextReader;

        public TextReaderLifecycleReader(Func<TextReader> createTextReader, Func<TextReader, IRead<T>> createReader)
        {
            if (createTextReader == null) { throw ArgumentIs.Null(nameof(createTextReader)); }
            if (createReader == null) { throw ArgumentIs.Null(nameof(createReader)); }

            this.createTextReader = createTextReader;
            this.createReader = createReader;
        }

        public T Read()
        {
            using (TextReader textReader = createTextReader())
            {
                return createReader(textReader).Read();
            }
        }
    }
}
