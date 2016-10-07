using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALE.SSISComponentWrap.DataFlowComponents
{
    public class OLEDBDestinationMapping
    {
        public string InputColumnName { get; set; }
        public string DestinationColumnName { get; set; }

        public OLEDBDestinationMapping()
        { }

        public OLEDBDestinationMapping(string inputColumnName, string destinationColumnName)
        {
            this.InputColumnName = inputColumnName;
            this.DestinationColumnName = destinationColumnName;
        }
    }
}
