using System.IO;

namespace BackupReport
{
    internal static class TextWriterExtensions
    {
        public static void WriteHeader(this TextWriter writer, string header)
        {
            var horizontalLine = string.Empty.PadLeft(header.Length, '=');

            writer.WriteLine(horizontalLine);
            writer.WriteLine(header);
            writer.WriteLine(horizontalLine);
        }
    }
}
