using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALE.SSISComponentWrap.DataFlowComponents
{
    public class DerivedColumnItem
    {
        public string Name { get; set; }
        public DataType DataType { get; set;}
        public string Expression { get; set; }
        public int Length { get; set; }
        public int Precision { get; set; }
        public int Scale { get; set; }
        public int CodePage { get; set; }
        public bool IsReplacingColumn { get; set; }
        public string ReplaceColumnName { get; set; }

        public DerivedColumnItem()
        { }

        public DerivedColumnItem(string name, string expression, DataType dataType) 
            : this()
        {
            this.Name = name;
            this.DataType = dataType;
            this.Expression = expression;
        }

        public DerivedColumnItem(string name, string expression, DataType dataType, int length) 
            : this(name, expression, dataType)            
        {
            this.Length = length;
        }

        public DerivedColumnItem(string expression, string replaceColumnName)            
        {
            this.IsReplacingColumn = true;
            this.ReplaceColumnName = replaceColumnName;
            this.Expression = expression;
        }
    }

   
}
