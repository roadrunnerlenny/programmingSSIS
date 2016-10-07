using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALE.SSISComponentWrap.DataFlowComponents
{
    public class LookupColumn
    {
        public string OutputAlias { get; set; }
        public string Name { get; set; }
        public bool IsReplacingOutput { get; set; }
        public string ReplaceColumnName { get; set; }

        public LookupColumn()
        { }

        public LookupColumn(string name, string outputAlias) 
            : this()
        {
            this.OutputAlias = outputAlias;
            this.Name = name;     
        }

        public LookupColumn(string name, string outputAlias, bool isReplacingOutput, string replaceColumnName)
            : this(name, outputAlias)
        {
            this.IsReplacingOutput = isReplacingOutput;
            this.ReplaceColumnName = replaceColumnName;

        }
    }
}
