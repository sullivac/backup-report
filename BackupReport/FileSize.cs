using System;

namespace BackupReport
{
    public class FileSize : IEquatable<FileSize>
    {
        private const double BytesPerGigabyte = 1073741824;
        private const double BytesPerMegabyte = 1048576;

        private readonly long value;

        public FileSize(long value)
        {
            if (value < 0) { throw new ArgumentOutOfRangeException(nameof(value), value, $"{nameof(value)} is less than zero (0)."); }

            this.value = value;
        }

        public double InMegabytes
        {
            get { return value / BytesPerMegabyte; }
        }

        public double InGigabytes
        {
            get { return value / BytesPerGigabyte; }
        }

        public FileSize Add(FileSize other)
        {
            if (other == null) { throw ArgumentIs.Null(nameof(other)); }

            return new FileSize(value + other.value);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as FileSize);
        }

        public bool Equals(FileSize other)
        {
            if (this == other) { return true; }
            if (other == null) { return false; }

            return value == other.value;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public static FileSize operator +(FileSize left, FileSize right)
        {
            if (left == null) { throw ArgumentIs.Null(nameof(left)); }
            if (right == null) { throw ArgumentIs.Null(nameof(right)); }

            return left.Add(right);
        }
    }
}
