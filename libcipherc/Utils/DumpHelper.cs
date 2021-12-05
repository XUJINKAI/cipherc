using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libcipherc.Utils
{
    public struct DumpColumn
    {
        public bool AutoWidth { get; set; } = false;
        public int Width { get; set; } = 0;
        public bool AlignRight { get; set; } = false;
        public int PaddingRight { get; set; } = 2;
    }

    public struct DumpCell
    {
        public string Content { get; set; }
        public int Level { get; set; }
    }

    public class DumpHelper
    {
        public int LevelIndent { get; set; } = 2;
        public string EndLine { get; set; } = "\n";
        public DumpColumn[] Columns { get; }

        public List<DumpCell[]> Datas { get; } = new List<DumpCell[]>();
        private DumpCell[] currentRow;

        public DumpHelper(params int[] columnsWidth)
        {
            Columns = columnsWidth.Select(
                x => x switch
                {
                    > 0 => new DumpColumn() { AutoWidth = false, Width = x },
                    0 => new DumpColumn() { AutoWidth = true },
                    < 0 => new DumpColumn() { AutoWidth = false, Width = -x, AlignRight = true },
                }).ToArray();
            currentRow = new DumpCell[Columns.Length];
        }


        public void SetLineLevel(int column, int level)
        {
            currentRow[column].Level = level;
        }

        public void SetLineContent(int column, string s)
        {
            currentRow[column].Content = s;
        }

        public void SubmitLine()
        {
            Datas.Add(currentRow);
            currentRow = new DumpCell[Columns.Length];
        }

        public void AppendLine(params string[] columns)
        {
            if (columns.Length > Columns.Length)
            {
                throw new ArgumentException();
            }

            Datas.Add(columns.Select(column => new DumpCell() { Content = column }).ToArray());
        }

        private string GetIndent(int level) => new string(' ', LevelIndent * level);

        public override string ToString()
        {
            string[][] indentDatas = Datas.Select(
                row => row.Select(
                    cell => GetIndent(cell.Level) + cell.Content)
                .ToArray()).ToArray();

            int datasMaxColumnCount = indentDatas.Max(row => row.Length);
            int[] colWidth = Enumerable.Range(0, datasMaxColumnCount).Select(
                index => indentDatas.Max(
                    row => row.Length > index ? row[index].Length : 0)
                ).ToArray();

            int[] realWidth = Columns.Select((column, index) => column.AutoWidth ? colWidth[index] : column.Width).ToArray();

            string[][] displayDatas = indentDatas.Select(
                row => row.Zip(Columns, realWidth).Select(
                    item =>
                    {
                        (string str, DumpColumn column, int width) = item;
                        var paddingRight = new string(' ', column.PaddingRight);
                        if (str.Length > width)
                        {
                            return str.Substring(0, width - 3) + "..." + paddingRight;
                        }
                        else
                        {
                            var spaces = new string(' ', width - str.Length);
                            if (column.AlignRight)
                            {
                                return spaces + str + paddingRight;
                            }
                            else
                            {
                                return str + spaces + paddingRight;
                            }
                        }
                    }).ToArray()
                ).ToArray();

            string dump = displayDatas.Select(
                row => row.JoinToString("").TrimEnd())
                .JoinToString(EndLine);

            return dump;
        }
    }
}
