using System;

namespace BackupReport
{
    internal static class ArgumentIs
    {
        public static ArgumentNullException Null(string parameterName)
        {
            return new ArgumentNullException(parameterName, $"{parameterName} is null.");
        }
    }
}
