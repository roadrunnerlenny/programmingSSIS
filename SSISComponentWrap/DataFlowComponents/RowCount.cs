using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALE.SSISComponentWrap.ControlFlow;
using SSIS = Microsoft.SqlServer.Dts.Runtime;
using Pipe = Microsoft.SqlServer.Dts.Pipeline.Wrapper;


namespace ALE.SSISComponentWrap.DataFlowComponents
{
    public class RowCount : ComponentBase
    {       
        public Variable Variable { get; set; }
        
        public RowCount()
        { }

        public RowCount(string name, Variable variable)
        {
            this.Name = name;
            this.Description = name;
            this.Variable = variable;
        }

        public RowCount AddComponent(DataFlow dataFlowTask)
        {
            base.AddComponent(dataFlowTask.SSISObject, "DTSTransform.RowCount.3");            
            ComponentWrapper.SetComponentProperty("VariableName", Variable.QualifiedName);
            return this;
        }
    }
}
