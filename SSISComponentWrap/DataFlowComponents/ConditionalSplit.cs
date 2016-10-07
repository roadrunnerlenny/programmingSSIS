using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALE.SSISComponentWrap.ControlFlow;
using SSIS = Microsoft.SqlServer.Dts.Runtime;
using Pipe = Microsoft.SqlServer.Dts.Pipeline.Wrapper;

namespace ALE.SSISComponentWrap.DataFlowComponents
{
    public class ConditionalSplit : ComponentBase
    {
        public string DefaultOutputName { get; set; }
        public bool HasDefaultOutputName
        {
            get
            {
                return !String.IsNullOrWhiteSpace(DefaultOutputName);
            }
        }

        public ConditionalSplit()
        { }

        public ConditionalSplit(string name, string defaultOutputName)
            : this()
        {
            this.Name = name;
            this.Description = name;
            this.DefaultOutputName = defaultOutputName;
        }

        public ConditionalSplit AddComponent(DataFlow dataFlowTask)
        {
            base.AddComponent(dataFlowTask.SSISObject, "DTSTransform.ConditionalSplit.3");
            if (HasDefaultOutputName)
                SSISDefaultOutput.Name = DefaultOutputName;
            return this;
        }

        public ConditionalSplit AddConditionalSplitItem(string outputName, string expression)
        {
            Pipe.IDTSOutput100 newOutput = base.AddSSISOutput(outputName);            
            ComponentWrapper.SetOutputProperty(newOutput.ID, "FriendlyExpression", expression);
            return this;
        }

        public new ConditionalSplit AddInputColumn(InputColumn column)
        {
            base.AddInputColumn(column);
            return this;
        }

        public new ConditionalSplit AddInputColumn(IList<InputColumn> columnName)
        {
            base.AddInputColumn(columnName);
            return this;
        } 

        public new ConditionalSplit AddReadOnlyInputColumn(string columnName)
        {
            base.AddReadOnlyInputColumn(columnName);
            return this;
        }

        
    }
}
