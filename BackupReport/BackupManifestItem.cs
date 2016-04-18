using System;

namespace BackupReport
{
    public class BackupManifestItem
    {
        private const string InvalidFormatMessage = "Invalid format: [backupFileName]   [backupTarget].";

        private readonly string backupTarget;
        private readonly string fileName;

        public BackupManifestItem(string fileName, string backupTarget)
        {
            this.fileName = fileName;
            this.backupTarget = backupTarget;
        }

        public string BackupTarget => backupTarget;

        public bool IsMatch(string backupTarget)
        {
            return string.Equals(this.backupTarget, backupTarget, StringComparison.Ordinal);
        }

        public static ParseResult Parse(string line)
        {
            string[] tokens = line.Split(new[] { "  " }, StringSplitOptions.RemoveEmptyEntries);

            if (tokens.Length < 2) { return ParseResult.Error(InvalidFormatMessage); }

            return ParseResult.Valid(new BackupManifestItem(tokens[0], tokens[1]));
        }

        public class ParseResult
        {
            private readonly string errorMessage;
            private readonly BackupManifestItem result;

            private ParseResult(string errorMessage)
            {
                this.errorMessage = errorMessage;
            }

            private ParseResult(BackupManifestItem result)
            {
                this.result = result;
            }

            public static ParseResult Error(string errorMessage)
            {
                return new ParseResult(errorMessage ?? string.Empty);
            }

            public static ParseResult Valid(BackupManifestItem result)
            {
                return new ParseResult(result);
            }

            public void Accept(Action<BackupManifestItem> onValid, Action<string> onError = null)
            {
                if (onValid == null) { throw ArgumentIs.Null(nameof(onValid)); }

                if (result == null)
                {
                    if (onError != null)
                    {
                        onError(errorMessage);
                    }
                }
                else
                {
                    onValid(result);
                }
            }
        }
    }
}
