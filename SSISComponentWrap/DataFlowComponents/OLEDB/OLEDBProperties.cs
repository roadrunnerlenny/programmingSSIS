using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALE.SSISComponentWrap.ControlFlow;
using SSIS = Microsoft.SqlServer.Dts.Runtime;
using Pipe = Microsoft.SqlServer.Dts.Pipeline.Wrapper;

namespace ALE.SSISComponentWrap.DataFlowComponents
{
    public class OLEDBProperties
    {
        public OLEDBConnectionManager ConnectionManager { get; set; }
        public DataAccessMode DataAccessMode { get; set; }
        public string SqlCommand { get; set; }
        public string TableOrViewName { get; set; }
       

        internal OLEDBProperties()
        {          
        }

        internal OLEDBProperties(OLEDBConnectionManager connectionManager)
            : this()
        {
            this.ConnectionManager = connectionManager;
        }

        protected void SetSqlCommand(string sqlCommand)
        {
            this.SqlCommand = sqlCommand;
            this.DataAccessMode = DataAccessMode.SqlCommand;
        }

        protected void SetTableOrViewName(string tableOrViewName)
        {
            this.TableOrViewName = tableOrViewName;
            this.DataAccessMode = DataAccessMode.TableOrView;
        }

        internal void SetComponentProperties(Pipe.IDTSComponentMetaData100 Component)
        {
            Component.RuntimeConnectionCollection[0].ConnectionManagerID = ConnectionManager.SSISObject.ID;
            Component.RuntimeConnectionCollection[0].ConnectionManager = SSIS.DtsConvert.GetExtendedInterface(ConnectionManager.SSISObject);
        }

        internal void SetComponentWrapperProperties(Pipe.CManagedComponentWrapper ComponentWrapper)
        {
            switch (DataAccessMode)
            {
                case DataAccessMode.SqlCommand: AddSqlCommand(ComponentWrapper); break;
                case DataAccessMode.TableOrView: AddTableOrViewCommand(ComponentWrapper); break;
            }
        }

        protected virtual void AddSqlCommand(Pipe.CManagedComponentWrapper ComponentWrapper)
        {
            ComponentWrapper.SetComponentProperty("SqlCommand", SqlCommand);
            ComponentWrapper.SetComponentProperty("AccessMode", 2);
        }

        protected virtual void AddTableOrViewCommand(Pipe.CManagedComponentWrapper ComponentWrapper)
        {
            ComponentWrapper.SetComponentProperty("OpenRowset", TableOrViewName);
         
            ComponentWrapper.SetComponentProperty("DefaultCodePage", 65001);
            ComponentWrapper.SetComponentProperty("CommandTimeout", 0);
            ComponentWrapper.SetComponentProperty("AlwaysUseDefaultCodePage", false);
        }
    }
}
