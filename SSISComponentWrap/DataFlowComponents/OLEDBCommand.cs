using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALE.SSISComponentWrap.ControlFlow;
using SSIS = Microsoft.SqlServer.Dts.Runtime;
using Pipe = Microsoft.SqlServer.Dts.Pipeline.Wrapper;

namespace ALE.SSISComponentWrap.DataFlowComponents
{
    public class OLEDBCommand : ComponentBase
    {
        public ConnectionManager ConnectionManager { get; set; }
        public string SQLCommand { get; set; }

        public OLEDBCommand()
        { }

        public OLEDBCommand(string name, ConnectionManager connectionManager, string sqlCommand)
            : this()
        {
            this.Name = name;
            this.Description = name;
            this.ConnectionManager = connectionManager;
            this.SQLCommand = sqlCommand;
        }

        public OLEDBCommand AddComponent(DataFlow dataFlowTask)
        {
            base.AddComponent(dataFlowTask.SSISObject, "DTSTransform.OLEDBCommand.3");
            Component.RuntimeConnectionCollection[0].ConnectionManager = SSIS.DtsConvert.GetExtendedInterface(ConnectionManager.SSISObject);
            Component.RuntimeConnectionCollection[0].ConnectionManagerID = ConnectionManager.SSISObject.ID;
            ComponentWrapper.SetComponentProperty("SqlCommand", SQLCommand); 
            return this;
        }

        public OLEDBCommand AddParameterMapping(string columnName, int parameterNumber)
        {
            Refresh();
            base.AddReadOnlyInputColumn(columnName);
           
            ComponentWrapper.MapInputColumn(DefaultInput.SSISObject.ID, base.FindSSISInputColumn(columnName).ID,
                DefaultInput.SSISObject.ExternalMetadataColumnCollection[parameterNumber].ID);
            return this;
        }

            
        
    }
}
