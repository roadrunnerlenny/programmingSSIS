using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSIS = Microsoft.SqlServer.Dts.Runtime;


namespace ALE.SSISComponentWrap.ControlFlow
{
    public class ExecutableBase : IExecutable
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public SSIS.Executable Executable { get; protected set; }

        protected virtual void AddComponent(ISequence sequence, string ExecutableID)
        {
            sequence.AddTask(this);
            Executable = sequence.Executables.Add(ExecutableID);            
        }
    }
}
