using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALE.SSISComponentWrap.ControlFlow;
using SSIS = Microsoft.SqlServer.Dts.Runtime;
using Pipe = Microsoft.SqlServer.Dts.Pipeline.Wrapper;


namespace ALE.SSISComponentWrap.DataFlowComponents
{
    public enum OLEDBSourceAccessMode
    {
        OpenRowset = 0,
        OpenRowsetFromVariable = 1,
        SQLCommand = 2,
        SQLCommandFromVariable = 3
    }

    public class OLEDBSourceProperties : OLEDBProperties
    {
        public OLEDBSourceProperties() : base()
        {}


        public OLEDBSourceProperties(OLEDBConnectionManager connectionManager)
            : base(connectionManager)
        { }

        public new OLEDBSourceProperties SetSqlCommand(string sqlCommand)
        {
            base.SetSqlCommand(sqlCommand);
            return this;
        }
        
        public new OLEDBSourceProperties SetTableOrViewName(string tableOrViewName)
        {
            base.SetTableOrViewName(tableOrViewName);
            return this;

        }

        protected override void AddTableOrViewCommand(Pipe.CManagedComponentWrapper ComponentWrapper)
        {
            ComponentWrapper.SetComponentProperty("AccessMode", OLEDBSourceAccessMode.OpenRowset);    
            base.AddTableOrViewCommand(ComponentWrapper);
        }
    }
}
