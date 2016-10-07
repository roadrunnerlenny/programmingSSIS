using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ALE.SSISComponentWrap.DataFlowComponents
{
    public class OutputColumn
    {
        public string Name { get; set; }
        public DataType DataType { get; set; }
        public int Length { get; set; }
        public int Precision { get; set; }
        public int Scale { get; set; }
        public int CodePage { get; set; }

        public OutputColumn()
        { }

        public OutputColumn(string name, DataType dataType) : this()
        {
            this.Name = name;
            this.DataType = dataType;
        }

        public OutputColumn(string name, DataType dataType, int length, int precision, int scale, int codePage) : this(name, dataType)
        {
            this.Length = length;
            this.Precision = precision;
            this.Scale = scale;
            this.CodePage = codePage;
        }
    }   
}
