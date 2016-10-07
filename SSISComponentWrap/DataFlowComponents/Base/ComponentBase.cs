using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSIS = Microsoft.SqlServer.Dts.Runtime;
using Pipe = Microsoft.SqlServer.Dts.Pipeline.Wrapper;

namespace ALE.SSISComponentWrap.DataFlowComponents
{
    public abstract class ComponentBase
    {
        #region General Properties and Methods

        public string Name { get; set; }
        public string Description { get; set; }

        public Pipe.IDTSComponentMetaData100 Component { get; protected set; }
        public Pipe.CManagedComponentWrapper ComponentWrapper { get; protected set; }

        protected virtual void AddComponent(Pipe.MainPipe dataFlowTask, string componentClassID)
        {
            Component = dataFlowTask.ComponentMetaDataCollection.New();
            Component.ComponentClassID = componentClassID;
            ComponentWrapper = Component.Instantiate();
            ComponentWrapper.ProvideComponentProperties();
            Component.Name = Name;
            Component.Description = Description;
        }

        public virtual void Refresh()
        {
            ComponentWrapper.AcquireConnections(null);
            ComponentWrapper.ReinitializeMetaData();
            ComponentWrapper.ReleaseConnections();
        }

        #endregion

        #region Input/Output Properties

        public OutputWrapper DefaultOutput
        {
            get
            {
                return new OutputWrapper(SSISDefaultOutput);
            }
        }

        protected Pipe.IDTSOutput100 SSISDefaultOutput
        {
            get
            {
                return Component.OutputCollection[0];
            }
        }

        public InputWrapper DefaultInput
        {
            get
            {
                return new InputWrapper(SSISDefaultInput);
            }
        }

        protected Pipe.IDTSInput100 SSISDefaultInput
        {
            get
            {
                return Component.InputCollection[0];
            }
        }

        protected List<Pipe.IDTSInput100> SSISInputs
        {
            get
            {
                return Component.InputCollection.Cast<Pipe.IDTSInput100>().ToList();
            }
        }

        protected List<Pipe.IDTSOutput100> SSISOutputs
        {
            get
            {
                return Component.OutputCollection.Cast<Pipe.IDTSOutput100>().ToList();
            }
        }

        public List<InputColumn> DefaultInputColumnCollection
        {
            get
            {
                return SSISDefaultInputColumnCollection.Select(
                    dtsColumnn => new InputColumn(dtsColumnn.Name, (InputColumnUsageType)dtsColumnn.UsageType)
                    ).ToList();
            }
        }

        protected List<Pipe.IDTSInputColumn100> SSISDefaultInputColumnCollection
        {
            get
            {
                return SSISDefaultInput.InputColumnCollection.Cast<Pipe.IDTSInputColumn100>().ToList();
            }
        }    

        public OutputWrapper FindOutput(string outputName)
        {
            Pipe.IDTSOutput100 ssisOutput = FindSSISOutput(outputName);
            if (ssisOutput != null)
                return new OutputWrapper(ssisOutput);
            else
                throw new KeyNotFoundException("Can't find output with name " + outputName);
        }

        protected Pipe.IDTSOutput100 FindSSISOutput(string outputName)
        {
            return SSISOutputs.Where(output => output.Name == outputName).FirstOrDefault();
        }

        protected Pipe.IDTSOutput100 AddSSISOutput(string outputName)
        {
            Pipe.IDTSOutput100 output = ComponentWrapper.InsertOutput(Pipe.DTSInsertPlacement.IP_AFTER, SSISDefaultOutput.ID);
            output.Name = outputName;
            output.Description = outputName;
            return output;
        }

        public InputWrapper FindInput(string inputName)
        {
            Pipe.IDTSInput100 ssisInput = FindSSISInput(inputName);
            if (ssisInput != null)
                return new InputWrapper(ssisInput);
            else
                throw new KeyNotFoundException("Can't find input with name " + inputName);
        }

        protected Pipe.IDTSInput100 FindSSISInput(string inputName)
        {
            return SSISInputs.Where(input => input.Name == inputName).FirstOrDefault();
        }

        protected Pipe.IDTSInputColumn100 FindSSISInputColumn(string columnName)
        {
            return SSISDefaultInputColumnCollection.Where(col => col.Name == columnName).First();
        }

        public InputColumn FindInputColumn(string columnName)
        {
            return DefaultInputColumnCollection.Where(col => col.Name == columnName).First();
        }

        public Pipe.IDTSInputColumn100 FindSSISInputColumn(string columnName, string inputName)
        {
            return FindSSISInput(inputName).InputColumnCollection.Cast<Pipe.IDTSInputColumn100>().Where(col => col.Name == columnName).First();
        }

        public bool HasInputColumn(string columnName)
        {
            return DefaultInputColumnCollection.Any(col => col.Name == columnName);
        }

        protected Pipe.IDTSOutputColumn100 FindOutputColumn(string columnName)
        {
            return SSISDefaultOutput.OutputColumnCollection.Cast<Pipe.IDTSOutputColumn100>().Where(col => col.Name == columnName).First();
        }

        protected Pipe.IDTSVirtualInput100 DefaultVirtualInput { get; set; }
        protected Pipe.IDTSVirtualInputColumnCollection100 DefaultVirtualInputColumnCollection { get; set; }
        protected bool HasDefaultVirtualInputInitialized { get; set; }

        protected void GetDefaultVirtualInput()
        {
            if (HasDefaultVirtualInputInitialized)
                return; //System.Diagnostics.Debug.WriteLine("[Warning] Virtual Input already initialized!");
            DefaultVirtualInput = SSISDefaultInput.GetVirtualInput();
            DefaultVirtualInputColumnCollection = DefaultVirtualInput.VirtualInputColumnCollection;
            HasDefaultVirtualInputInitialized = true;
        }

        protected List<Pipe.IDTSVirtualInput100> VirtualInputs { get; set; }
        protected Dictionary<Pipe.IDTSVirtualInput100, Pipe.IDTSVirtualInputColumnCollection100> VirtualInputsColumnCollections { get; set; }
        protected bool HasAllVirtualInputsInitialized { get; set; }

        protected void GetAllVirtualInputs()
        {
            if (HasAllVirtualInputsInitialized)
                return;
            VirtualInputs = new List<Pipe.IDTSVirtualInput100>();
            VirtualInputsColumnCollections = new Dictionary<Pipe.IDTSVirtualInput100, Pipe.IDTSVirtualInputColumnCollection100>();
            foreach (Pipe.IDTSInput100 input in this.SSISInputs)
            {
                Pipe.IDTSVirtualInput100 virtualInput = input.GetVirtualInput();
                VirtualInputs.Add(virtualInput);
                VirtualInputsColumnCollections.Add(virtualInput, virtualInput.VirtualInputColumnCollection);
            }
        }

        public void AddInputColumn(IList<InputColumn> columns)
        {
            GetDefaultVirtualInput();
            for (int i = 0; i < DefaultVirtualInputColumnCollection.Count; i++)
            {
                InputColumn column = columns.Where(input => input.Name == DefaultVirtualInputColumnCollection
[i].Name).FirstOrDefault();
                if (column != null)
                    ComponentWrapper.SetUsageType(SSISDefaultInput.ID, DefaultVirtualInput, DefaultVirtualInputColumnCollection[i].LineageID, (Pipe.DTSUsageType)column.UsageType);
            }
        }

        protected void AddInputColumn(InputColumn column)
        {
            this.AddInputColumn(new List<InputColumn>() { column });
        }

        protected void AddReadOnlyInputColumn(string columnName)
        {
            this.AddInputColumn(new InputColumn(columnName, InputColumnUsageType.ReadOnly));
        }

        protected void AddReadWriteInputColumn(string columnName)
        {
            this.AddInputColumn(new InputColumn(columnName, InputColumnUsageType.ReadWrite));
        }

        protected void AddInputColumnForInput(InputColumn column, string inputName)
        {
            GetAllVirtualInputs();
            Pipe.IDTSVirtualInput100 virtualInput = VirtualInputs.Where(vi => vi.Description == inputName).First();
           
            Pipe.IDTSInput100 input = SSISInputs.Where(i => i.Name == inputName).First();
            for (int i = 0; i <  VirtualInputsColumnCollections[virtualInput].Count; i++)
            {                
                if (column.Name == VirtualInputsColumnCollections[virtualInput][i].Name)
                    ComponentWrapper.SetUsageType(input.ID, virtualInput, VirtualInputsColumnCollections[virtualInput][i].LineageID, (Pipe.DTSUsageType)column.UsageType);
            };
        }

        protected void AddReadOnlyInputColumnForInput(string columnName, string inputName)
        {
            AddInputColumnForInput(new InputColumn(columnName, InputColumnUsageType.ReadOnly), inputName);
        }

        protected Pipe.IDTSVirtualInputColumn100 FindVirtualInputColumn(string columnName)
        {
            GetDefaultVirtualInput();
            return DefaultVirtualInputColumnCollection[columnName];
        }

        protected void MapColumnsByMatchingNames()
        {
            Refresh();
            GetDefaultVirtualInput();
            foreach (Pipe.IDTSVirtualInputColumn100 virtualColumn in DefaultVirtualInputColumnCollection)
            {
                if (SSISDefaultInput.ExternalMetadataColumnCollection.Cast<Pipe.IDTSExternalMetadataColumn100>().Any(externalColumn => externalColumn.Name == virtualColumn.Name))
                    MapInputColumn(virtualColumn, virtualColumn.Name);
            }
            Refresh();
        }

        protected void MapColumns(List<OLEDBDestinationMapping> mappings)
        {
            Refresh();
            GetDefaultVirtualInput();
            foreach (Pipe.IDTSVirtualInputColumn100 virtualColumn in DefaultVirtualInputColumnCollection)
            {
                var column = mappings.Where(mapping => mapping.InputColumnName == virtualColumn.Name).FirstOrDefault();
                if (column != null)
                    MapInputColumn(virtualColumn, column.DestinationColumnName);
            }
            Refresh();
        }

        protected void MapColumn(OLEDBDestinationMapping mapping)
        {
            this.MapColumns(new List<OLEDBDestinationMapping>() { mapping });
        }

        private void MapInputColumn(Pipe.IDTSVirtualInputColumn100 virtualColumn, string destinationColumnName)
        {
            Pipe.IDTSInputColumn100 inputColumn = ComponentWrapper.SetUsageType(SSISDefaultInput.ID, DefaultVirtualInput, virtualColumn.LineageID, Pipe.DTSUsageType.UT_READONLY);
            ComponentWrapper.MapInputColumn(SSISDefaultInput.ID, inputColumn.ID, SSISDefaultInput.ExternalMetadataColumnCollection[destinationColumnName].ID);
        }                

        protected void SetErrorOutputForAllColumns(RowDisposition errorRowDisposition, RowDisposition truncationRowDisposition) {
            foreach (Pipe.IDTSOutputColumn100 column in SSISDefaultOutput.OutputColumnCollection)
            {
                column.TruncationRowDisposition = (Pipe.DTSRowDisposition)truncationRowDisposition;
                column.ErrorRowDisposition = (Pipe.DTSRowDisposition)errorRowDisposition; 
            }
        }
        #endregion

    }

    public enum RowDisposition
    {
        RD_NotUsed = 0,
        RD_IgnoreFailure = 1,
        RD_RedirectRow = 2,
        RD_FailComponent = 4,
    }
}
