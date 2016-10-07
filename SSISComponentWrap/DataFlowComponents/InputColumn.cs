using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ALE.SSISComponentWrap.DataFlowComponents
{
    public class InputColumn
    {
        public string Name { get; set; }
        public InputColumnUsageType UsageType { get; set; }

        public InputColumn()
        { }

        public InputColumn(string name, InputColumnUsageType usageType)
        {
            this.Name = name;
            this.UsageType = usageType;
        }
    }

    public enum InputColumnUsageType
    {
        ReadOnly = 0,
        ReadWrite = 1,      
        Ignored = 2,
    }
}
