using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALE.SSISComponentWrap.ControlFlow;
using SSIS = Microsoft.SqlServer.Dts.Runtime;
using Pipe = Microsoft.SqlServer.Dts.Pipeline.Wrapper;


namespace ALE.SSISComponentWrap.DataFlowComponents
{
    public enum OLEDBDestinationAccessMode
    {
        OpenRowset = 0,
        OpenRowsetFromVariable = 1,
        SQLCommand = 2,
        OpenRowsetUsingFastload = 3,
        OpenRowsetUsingFastloadFromVariable = 4
    }

    public class OLEDBDestinationProperties : OLEDBProperties
    {
        public OLEDBDestinationProperties() : base()
        {
            this.HasTabLock = true;
            this.HasCheckConstraints = true;
        }

       public OLEDBDestinationProperties(OLEDBConnectionManager connectionManager)
            : this()
        {
            this.ConnectionManager = connectionManager;
        }

        public bool HasTabLock { get; set; }
        public bool HasCheckConstraints { get; set; }

        private List<string> FastLoadOptions
        {
            get
            {
                List<string> result = new List<string>();
                if (HasTabLock)
                    result.Add("TABLOCK");
                if (HasCheckConstraints)
                    result.Add("CHECK_CONSTRAINTS");
                return result;
            }
        }

        private bool HasFastLoadOptions
        {
            get
            {
                return FastLoadOptions.Count > 0;
            }
        }

        protected override void AddTableOrViewCommand(Pipe.CManagedComponentWrapper ComponentWrapper)
        {
            ComponentWrapper.SetComponentProperty("AccessMode", OLEDBDestinationAccessMode.OpenRowsetUsingFastload);     
            base.AddTableOrViewCommand(ComponentWrapper);
            if (HasFastLoadOptions)
                ComponentWrapper.SetComponentProperty("FastLoadOptions", string.Join(",", FastLoadOptions.ToArray()));
            ComponentWrapper.SetComponentProperty("FastLoadKeepIdentity", false);
            ComponentWrapper.SetComponentProperty("FastLoadKeepNulls", false);
            ComponentWrapper.SetComponentProperty("FastLoadMaxInsertCommitSize", 2147483647);
        }

        public new OLEDBDestinationProperties SetSqlCommand(string sqlCommand)
        {
            base.SetSqlCommand(sqlCommand);
            return this;
        }

        public new OLEDBDestinationProperties SetTableOrViewName(string tableOrViewName)
        {
            base.SetTableOrViewName(tableOrViewName);
            return this;

        }


    }
}
