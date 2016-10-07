using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALE.SSISComponentWrap.ControlFlow;
using SSIS = Microsoft.SqlServer.Dts.Runtime;
using Pipe = Microsoft.SqlServer.Dts.Pipeline.Wrapper;

namespace ALE.SSISComponentWrap.DataFlowComponents
{
    public class OLEDBDestination : ComponentBase
    {
        public OLEDBDestinationProperties Properties { get; set; }

        public OLEDBDestination()
        { }

        public OLEDBDestination(string name, OLEDBDestinationProperties properties)
            : this()
        {
            this.Name = name;
            this.Description = name;
            this.Properties = properties;
        }

        public OLEDBDestination AddComponent(DataFlow dataFlowTask)
        {
            base.AddComponent(dataFlowTask.SSISObject, "DTSAdapter.OleDbDestination");
            Properties.SetComponentProperties(Component);
            Properties.SetComponentWrapperProperties(ComponentWrapper);
            return this;
        }

        public new OLEDBDestination MapColumnsByMatchingNames()
        {
            base.MapColumnsByMatchingNames();
            return this;
        }

        public new OLEDBDestination MapColumns(List<OLEDBDestinationMapping> mappings)
        {
            base.MapColumns(mappings);
            return this;
        }

        public new OLEDBDestination MapColumn(OLEDBDestinationMapping mapping)
        {
            base.MapColumn(mapping);
            return this;
        }
    }
}
