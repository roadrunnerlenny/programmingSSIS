using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALE.SSISComponentWrap.ControlFlow;
using SSIS = Microsoft.SqlServer.Dts.Runtime;
using Pipe = Microsoft.SqlServer.Dts.Pipeline.Wrapper;

namespace ALE.SSISComponentWrap.DataFlowComponents
{
    public class Path
    {
        public InputWrapper Input { get; set; }
        public OutputWrapper Output { get; set; }
        public string Name { get; set; }
        public bool HasName
        {
            get
            {
                return !string.IsNullOrEmpty(Name);
            }
        }
        public Path()
        { }

        public Path(OutputWrapper output, InputWrapper input)
            : this()
        {
            this.Output = output;
            this.Input = input;
        }

        public Path(string name, OutputWrapper output, InputWrapper input)
            : this(output, input)
        {
            this.Name = name;
        }

        public void Connect(DataFlow dataFlowTask)
        {
            Pipe.IDTSPath100 path = dataFlowTask.SSISObject.PathCollection.New();
            path.AttachPathAndPropagateNotifications(Output.SSISObject, Input.SSISObject);
            if (HasName)
                path.Name = Name;
            
        }       
    }
}
