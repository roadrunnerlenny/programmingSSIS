using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALE.SSISComponentWrap.ControlFlow;
using SSIS = Microsoft.SqlServer.Dts.Runtime;
using Pipe = Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Wrapper = Microsoft.SqlServer.Dts.Runtime.Wrapper;


namespace ALE.SSISComponentWrap.DataFlowComponents
{
    public class Lookup : ComponentBase
    {
        public ConnectionManager ConnectionManager { get; set; }
        public string SQLCommand { get; set; }
        public LookupAdvancedParameters Advanced { get; set; }
        public bool HasAdvanced
        {
            get
            {
                return Advanced != null;
            }
        }
        public CacheType CacheType { get; set; }
        public NoMatchBehaviour MatchBehaviour { get; set; }

        public OutputWrapper MatchOutput
        {
            get
            {
                return DefaultOutput;
            }
        }

        public OutputWrapper NoMatchOutput
        {
            get
            {
                return new OutputWrapper(SSISNoMatchOutput);
            }
        }

        protected Pipe.IDTSOutput100 SSISNoMatchOutput
        {
            get
            {
                return Component.OutputCollection[1];
            }
        }


        public Lookup()
        { }

        public Lookup(string name, ConnectionManager connectionManager, string sqlCommand)
        {
            this.Name = name;
            this.Description = name;
            this.ConnectionManager = connectionManager;
            this.SQLCommand = sqlCommand;
            this.MatchBehaviour = NoMatchBehaviour.RedirectRowsToNoMatchOutput;
        }

        public Lookup(string name, ConnectionManager connectionManager, string sqlCommand, NoMatchBehaviour noMatchBehaviour) :
            this(name, connectionManager, sqlCommand)
        {
            this.MatchBehaviour = noMatchBehaviour;
        }

        public Lookup(string name, ConnectionManager connectionManager, string sqlCommand, NoMatchBehaviour noMatchBehaviour, CacheType cacheType) :
            this(name, connectionManager, sqlCommand, noMatchBehaviour)
        {
            this.CacheType = cacheType;

        }

        public Lookup AddComponent(DataFlow dataFlowTask)
        {
            base.AddComponent(dataFlowTask.SSISObject, "DTSTransform.Lookup");
            Component.RuntimeConnectionCollection[0].ConnectionManager = SSIS.DtsConvert.GetExtendedInterface(ConnectionManager.SSISObject);
            Component.RuntimeConnectionCollection[0].ConnectionManagerID = ConnectionManager.SSISObject.ID;
            ComponentWrapper.SetComponentProperty("CacheType", (int)CacheType);
            ComponentWrapper.SetComponentProperty("SqlCommand", SQLCommand);

            if (MatchBehaviour == NoMatchBehaviour.FailComponent || MatchBehaviour == NoMatchBehaviour.RedirectRowsToNoMatchOutput)
                ComponentWrapper.SetComponentProperty("NoMatchBehavior", MatchBehaviour);
            else if (MatchBehaviour == NoMatchBehaviour.IgnoreFailure)
            {
                ComponentWrapper.SetComponentProperty("NoMatchBehavior", NoMatchBehaviour.FailComponent);
                this.MatchOutput.SetErrorRowDisposition(RowDisposition.RD_IgnoreFailure);
            }
            else
                ComponentWrapper.SetComponentProperty("NoMatchBehavior", NoMatchBehaviour.FailComponent);
            

            return this;
        }

        public Lookup SetAdvancedParameters(LookupAdvancedParameters advanced)
        {
            this.Advanced = advanced;

            if (Advanced.HasSqlCommandParam)
            {
                ComponentWrapper.SetComponentProperty("SqlCommandParam", Advanced.SqlCommandParam);
                if (Advanced.HasParameters)
                {
                    string parameterMap = string.Join(";", Advanced.Parameters.Select(par => "#" + FindSSISInputColumn(par).LineageID.ToString())) + ";"; 
                    ComponentWrapper.SetComponentProperty("ParameterMap", parameterMap);                        
                }
            }

            if (Advanced.HasCacheSize32Bit)
                ComponentWrapper.SetComponentProperty("MaxMemoryUsage", Advanced.CacheSize32Bit);

            if (Advanced.HasCacheSize64Bit)
                ComponentWrapper.SetComponentProperty("MaxMemoryUsage64", Advanced.CacheSize64Bit);

            if (Advanced.HasNoMatchCachePercentage)
                ComponentWrapper.SetComponentProperty("NoMatchCachePercentage", Advanced.NoMatchCachePercentage);
           
            return this;
        }

        public Lookup AddLookupColumn(LookupColumn lookupItem)
        {
            Refresh();
            if (!lookupItem.IsReplacingOutput)
            {
                Pipe.IDTSOutputColumn100 newColumn = ComponentWrapper.InsertOutputColumnAt(SSISDefaultOutput.ID, 0, lookupItem.OutputAlias, "");
                ComponentWrapper.SetOutputColumnProperty(SSISDefaultOutput.ID, newColumn.ID, "CopyFromReferenceColumn", lookupItem.Name);
            }
            else
            {
                Pipe.IDTSVirtualInputColumn100 virtualColumn = base.FindVirtualInputColumn(lookupItem.ReplaceColumnName);
                Pipe.IDTSInputColumn100 inputColumn = ComponentWrapper.SetUsageType(SSISDefaultInput.ID, base.DefaultVirtualInput, virtualColumn.LineageID, Pipe.DTSUsageType.UT_READWRITE);
                ComponentWrapper.SetInputColumnProperty(SSISDefaultInput.ID, inputColumn.ID, "CopyFromReferenceColumn", lookupItem.Name);
            }

            return this;
        }

        public Lookup AddLookupMapping(LookupMapping lookupMapping)
        {
            ComponentWrapper.SetInputColumnProperty(SSISDefaultInput.ID, base.FindSSISInputColumn(lookupMapping.InputColumnName).ID, "JoinToReferenceColumn", lookupMapping.LookupColumnName);
            return this;
        }

        public new Lookup AddInputColumn(InputColumn column)
        {
            base.AddInputColumn(column);
            return this;
        }

        public new Lookup AddInputColumn(IList<InputColumn> columnName)
        {
            base.AddInputColumn(columnName);
            return this;
        }

        public new Lookup AddReadOnlyInputColumn(string columnName)
        {
            base.AddReadOnlyInputColumn(columnName);
            return this;
        }
    }

    public enum CacheType
    {
        Full = 0,
        Partial = 1,
        None = 2
    }

    public enum NoMatchBehaviour
    {
        FailComponent = 0,
        RedirectRowsToNoMatchOutput = 1,
        IgnoreFailure = 2
    }
}
