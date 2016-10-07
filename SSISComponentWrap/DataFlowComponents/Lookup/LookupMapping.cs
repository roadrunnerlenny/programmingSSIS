using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALE.SSISComponentWrap.DataFlowComponents
{
    public class LookupMapping
    {
        public string InputColumnName { get; set; }
        public string LookupColumnName { get; set; }

        public LookupMapping()
        { }

        public LookupMapping(string inputColumnName, string lookupColumnName)
        {
            this.InputColumnName = inputColumnName;
            this.LookupColumnName = lookupColumnName;
        }
    }
}
