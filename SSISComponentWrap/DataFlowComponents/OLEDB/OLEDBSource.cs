using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALE.SSISComponentWrap.ControlFlow;
using SSIS = Microsoft.SqlServer.Dts.Runtime;
using Pipe = Microsoft.SqlServer.Dts.Pipeline.Wrapper;

namespace ALE.SSISComponentWrap.DataFlowComponents
{
    public class OLEDBSource : ComponentBase
    {      
        public OLEDBSourceProperties Properties { get; set; }

        public OLEDBSource()
        { }

        public OLEDBSource(string name, OLEDBSourceProperties properties)
            : this()
        {
            this.Name = name;
            this.Description = name;
            this.Properties = properties;        
        }

        public OLEDBSource AddComponent(DataFlow dataFlowTask)
        {
            base.AddComponent(dataFlowTask.SSISObject, "DTSAdapter.OleDbSource");
            Properties.SetComponentProperties(Component);
            Properties.SetComponentWrapperProperties(ComponentWrapper);           
            Refresh();
            return this;
        }
       
    }

  
}
