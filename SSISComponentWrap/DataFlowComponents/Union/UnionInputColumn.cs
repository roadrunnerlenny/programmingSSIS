using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALE.SSISComponentWrap.ControlFlow;
using SSIS = Microsoft.SqlServer.Dts.Runtime;
using Pipe = Microsoft.SqlServer.Dts.Pipeline.Wrapper;

namespace ALE.SSISComponentWrap.DataFlowComponents
{ 
    public class UnionInputColumn
    {
        public string InputName { get; set; }
        public string ColumnName { get; set; }

        public UnionInputColumn()
        { }

        public UnionInputColumn(string inputName, string columnName)
        {
            this.InputName = inputName;
            this.ColumnName = columnName;
        }
    }
}
