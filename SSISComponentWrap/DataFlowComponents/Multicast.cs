using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALE.SSISComponentWrap.ControlFlow;
using SSIS = Microsoft.SqlServer.Dts.Runtime;
using Pipe = Microsoft.SqlServer.Dts.Pipeline.Wrapper;

namespace ALE.SSISComponentWrap.DataFlowComponents
{
    public class Multicast : ComponentBase
    {
        public Multicast()
        { }

        public Multicast(string name)
        {
            this.Name = name;
            this.Description = name;
        }

        public Multicast AddComponent(DataFlow dataFlowTask)
        {
            base.AddComponent(dataFlowTask.SSISObject, "DTSTransform.Multicast.3");
            return this;
        }

        public Multicast RenameLastOutput(string outputName)
        {
            Component.OutputCollection[SSISOutputs.Count - 1].Name = outputName;
            Component.OutputCollection[SSISOutputs.Count - 1].Description = outputName;
            return this;
        }

        //public Multicast AddOutput(string outputName)
        //{
        //    Pipe.IDTSOutput100 output = Component.OutputCollection.New();
        //    output.Name = outputName;
        //    return this;
        //}

        //public Multicast RenameDefaultOutput(string outputName)
        //{
        //    this.DefaultOutput.SSISObject.Name = outputName;
        //    return this;
        //}

    }   
}
