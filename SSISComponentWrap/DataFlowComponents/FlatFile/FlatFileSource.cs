using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALE.SSISComponentWrap.ControlFlow;
using SSIS = Microsoft.SqlServer.Dts.Runtime;
using Pipe = Microsoft.SqlServer.Dts.Pipeline.Wrapper;

namespace ALE.SSISComponentWrap.DataFlowComponents
{
    public class FlatFileSource : ComponentBase
    {      
        public FlatFileConnectionManager ConnectionManager { get; set; }
        public bool RetainNulls { get; set; }

        public FlatFileSource()
        { }

        public FlatFileSource(string name, FlatFileConnectionManager connectionManager)
            : this()
        {
            this.Name = name;
            this.Description = name;
            this.ConnectionManager = connectionManager;        
        }

        public FlatFileSource(string name, FlatFileConnectionManager connectionManager, bool retainNulls)
            : this(name, connectionManager)
        {
            this.RetainNulls = retainNulls;
        }

        public FlatFileSource AddComponent(DataFlow dataFlowTask)
        {
            base.AddComponent(dataFlowTask.SSISObject, "DTSAdapter.FlatFileSource");
            Component.CustomPropertyCollection["RetainNulls"].Value = RetainNulls;
            Component.RuntimeConnectionCollection[0].ConnectionManagerID = ConnectionManager.SSISObject.ID;
            Component.RuntimeConnectionCollection[0].ConnectionManager = SSIS.DtsConvert.GetExtendedInterface(ConnectionManager.SSISObject);            Refresh();
            return this;
        }

        public new FlatFileSource SetErrorOutputForAllColumns(RowDisposition errorRowDisposition, RowDisposition truncationRowDisposition) 
        {
            base.SetErrorOutputForAllColumns(errorRowDisposition, truncationRowDisposition);
            return this;
        }
       
    }

  
}
