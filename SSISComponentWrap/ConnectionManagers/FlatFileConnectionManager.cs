using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALE.SSISComponentWrap.ControlFlow;
using SSIS = Microsoft.SqlServer.Dts.Runtime;
using Wrapper = Microsoft.SqlServer.Dts.Runtime.Wrapper;


namespace ALE.SSISComponentWrap
{
    public class FlatFileConnectionManager : ConnectionManager
    {
        private Wrapper.IDTSConnectionManagerFlatFile100 FlatFileConnection { get; set; }

        public Delimiter HeaderRowDelimiter { get; set; }
        public int CodePage { get; set; }
        public string Format { get; set; }
        public string TextQualifier { get; set; }
        public bool ColumnNamesInFirstDataRow { get; set; }
        public int HeaderRowsToSkip { get; set; }
        public Delimiter ColumnDelimiter { get; set; }
        public Delimiter RowDelimiter { get; set; }

        public string FileName
        {
            get {
                return ConnectionString;
            }
            set
            {
                ConnectionString = value;
            }
        }

        public FlatFileConnectionManager()
        { }

        public FlatFileConnectionManager(string name)
            : this()
        {
            this.Name = name;
            this.Description = name;           
        }

        public FlatFileConnectionManager AddComponent(Package package)
        {
            base.AddComponent(package, "FLATFILE");
            FlatFileConnection = SSISObject.InnerObject as Wrapper.IDTSConnectionManagerFlatFile100;            
            FlatFileConnection.CodePage = CodePage;
            FlatFileConnection.HeaderRowDelimiter = Convert(HeaderRowDelimiter);
            FlatFileConnection.Format = Format;
            FlatFileConnection.TextQualifier = TextQualifier;
            FlatFileConnection.ColumnNamesInFirstDataRow = ColumnNamesInFirstDataRow;
            FlatFileConnection.HeaderRowsToSkip = HeaderRowsToSkip;
            FlatFileConnection.RowDelimiter = Convert(RowDelimiter);             
            return this;
        }

        public FlatFileConnectionManager AddAdvancedColumn(FlatFileColumn flatFileColumn)
        {
            Wrapper.IDTSConnectionManagerFlatFileColumn100 newColumn = FlatFileConnection.Columns.Add();
            newColumn.MaximumWidth = flatFileColumn.OutputColumnWidth;
            newColumn.ColumnDelimiter = Convert(ColumnDelimiter);  
            newColumn.TextQualified = flatFileColumn.TextQualified;
            newColumn.ColumnType = flatFileColumn.ColumnType;                                 
            if (flatFileColumn.InputColumnWidth > 0)
                newColumn.ColumnWidth = flatFileColumn.InputColumnWidth;
            else
                newColumn.ColumnWidth = flatFileColumn.OutputColumnWidth;
            newColumn.DataType = (Wrapper.DataType)flatFileColumn.DataType;
            newColumn.DataScale = flatFileColumn.DataScale;
            newColumn.DataPrecision = flatFileColumn.DataPrecision;
            
            Wrapper.IDTSName100 columnName = newColumn as Wrapper.IDTSName100;
            columnName.Name = flatFileColumn.Name;
            return this;   
        }

        public FlatFileConnectionManager AdjustDelimiterForLastColumn()
        {
            if (FlatFileConnection.Columns.Count > 0)
                FlatFileConnection.Columns[FlatFileConnection.Columns.Count - 1].ColumnDelimiter = Convert(RowDelimiter);
            return this;
        }

        public new FlatFileConnectionManager AddExpression(Expression expression)
        {
            base.AddExpression(expression);
            return this;
        }

        public static string Convert(Delimiter delimiter)
        {
            switch (delimiter)
            {
                case Delimiter.CR_LF: return "\r\n";//return Environment.NewLine;//"{CR}{LF}";
                case Delimiter.CR: return "\r";//return Environment.//return "{CR}";
                case Delimiter.LF : return "\n";//"{LF}";
                case Delimiter.Colon: return ":";
                case Delimiter.Comma: return ",";
                case Delimiter.Semicolon: return ";";
                case Delimiter.Tab: return "\t";//"{t}";
                case Delimiter.VerticalBar: return "|";
                default: return "";
            }
        }
    }

    public enum Delimiter
    {
        CR_LF,
        CR,
        LF,
        Semicolon,
        Colon,
        Comma,
        Tab,
        VerticalBar
    }
}
