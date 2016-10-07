using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALE.SSISComponentWrap
{
    public class FlatFileColumn
    {
        public string ColumnType { get; set; }
        public string Name { get; set; }
        public int OutputColumnWidth { get; set; }
        public int InputColumnWidth { get; set; }
        public DataType DataType { get; set; }
        public int DataScale { get; set; }
        public int DataPrecision { get; set; }
        public bool TextQualified { get; set; }
    }
}
