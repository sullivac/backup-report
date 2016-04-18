using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupReport.Reports
{
    public class ReportTable<TSource>
    {
        private readonly List<ReportColumn> columns;
        private readonly List<ReportCell> headers;
        private readonly List<List<ReportCell>> records;

        public ReportTable()
        {
            columns = new List<ReportColumn>();
            headers = new List<ReportCell>();
            records = new List<List<ReportCell>>();
        }

        public void AddColumn(Func<TSource, string> getValue, string headerText, Justification justification = Justification.Left)
        {
            AddColumn(getValue, AsIs, headerText, justification);
        }

        public void AddColumn<TProperty>(Func<TSource, TProperty> getValue, Func<TProperty, string> formatValue, string headerText, Justification justification = Justification.Left)
        {
            var column = new ReportColumn(source => formatValue(getValue(source)), headerText, headerText.Length, justification);

            columns.Add(column);
            headers.Add(column.CreateHeaderCell());
        }

        public void Load(IList<TSource> data)
        {
            if (data == null) { throw ArgumentIs.Null(nameof(data)); }

            if (data.Count == 0) { return; }

            foreach (var item in data)
            {
                records.Add(columns.Select(column => column.CreateCell(item)).ToList());
            }
        }

        public void Write(TextWriter textWriter)
        {
            new ReportWriter(textWriter, columns, headers, records).Write();
        }

        private static string AsIs(string value)
        {
            return value;
        }

        private class ReportCell
        {
            private readonly string formattedValue;
            private readonly Func<string, string> justify;

            public ReportCell(string formattedValue, Func<string, string> justify)
            {
                this.formattedValue = formattedValue;
                this.justify = justify;
            }

            public string Value
            {
                get { return justify(formattedValue); }
            }
        }

        private class ReportColumn
        {
            private int longestLength;
            private readonly Func<TSource, string> getFormattedValue;
            private readonly Justification justification;

            public ReportColumn(
                Func<TSource, string> getFormattedValue,
                string headerText,
                int longestLength,
                Justification justification)
            {
                this.getFormattedValue = getFormattedValue;
                this.justification = justification;

                HeaderText = headerText;
                LongestLength = longestLength;
            }

            public string HeaderText { get; }

            public int LongestLength
            {
                get { return longestLength; }
                private set
                {
                    if (value > longestLength)
                    {
                        longestLength = value;
                        Separator = string.Empty.PadLeft(value, '-');
                    }
                }
            }

            public string Separator { get; set; }

            public ReportCell CreateCell(TSource source)
            {
                string formattedValue = getFormattedValue(source);

                LongestLength = formattedValue.Length;

                Func<string, string> justify;
                if (justification == Justification.Left)
                {
                    justify = JustifyLeft;
                }
                else if (justification == Justification.Right)
                {
                    justify = JustifyRight;
                }
                else
                {
                    throw new InvalidOperationException($"Unknown justification {justification}.");
                }

                return new ReportCell(formattedValue, justify);
            }

            public ReportCell CreateHeaderCell()
            {
                return new ReportCell(HeaderText, JustifyLeft);
            }

            private string JustifyLeft(string value)
            {
                return value.PadRight(LongestLength);
            }

            private string JustifyRight(string value)
            {
                return value.PadLeft(LongestLength);
            }
        }

        private class ReportWriter
        {
            private const string RowJoin = " | ";
            private const string RowPrefix = "| ";
            private const string RowSuffix = " |";
            private const string SeparatorFix = "--";
            private const string SeparatorJoin = "---";

            private readonly List<ReportColumn> columns;
            private readonly List<ReportCell> headers;
            private readonly List<List<ReportCell>> records;
            private readonly Lazy<string> separator;
            private readonly TextWriter writer;

            public ReportWriter(TextWriter writer, List<ReportColumn> columns, List<ReportCell> headers, List<List<ReportCell>> records)
            {
                this.writer = writer;
                this.columns = columns;
                this.headers = headers;
                this.records = records;

                separator =
                    new Lazy<string>(
                        () =>
                        {
                            string edge = string.Join(SeparatorJoin, columns.Select(column => column.Separator));

                            return new StringBuilder()
                                .Append(SeparatorFix).Append(edge).Append(SeparatorFix)
                                .ToString();
                        });
            }

            public void Write()
            {
                WriteSeparator();
                WriteHeader();
                WriteSeparator();

                if (records.Count == 0)
                {
                    WriteEmpty();
                }
                else
                {
                    WriteData();
                }

                WriteSeparator();
            }

            private void WriteData()
            {
                for (int index = 0; index < records.Count; index++)
                {
                    if (index > 0) { WriteSeparator(); }

                    WriteRow(string.Join(RowJoin, records[index].Select(cell => cell.Value)));
                }
            }

            private void WriteSeparator()
            {
                writer.WriteLine(separator.Value);
            }

            private void WriteEmpty()
            {
                WriteRow("No data".PadRight(columns.Sum(column => column.LongestLength) + (columns.Count - 1) * RowJoin.Length));
            }

            private void WriteHeader()
            {
                WriteRow(string.Join(RowJoin, headers.Select(header => header.Value)));
            }

            private void WriteRow(string value)
            {
                writer.Write(RowPrefix);
                writer.Write(value);
                writer.WriteLine(RowSuffix);
            }
        }
    }
}
