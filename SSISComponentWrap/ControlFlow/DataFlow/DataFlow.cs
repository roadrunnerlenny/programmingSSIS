using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSIS = Microsoft.SqlServer.Dts.Runtime;
using Pipe = Microsoft.SqlServer.Dts.Pipeline.Wrapper;

namespace ALE.SSISComponentWrap.ControlFlow
{
    public class DataFlow : TaskBase, ISSISObjectWrap<Pipe.MainPipe>
    {
        public Pipe.MainPipe SSISObject { get; private set; }

        public DebugComponentEventHandler ComponentEventHandler { get; set; }

        public int? DefaultBufferMaxRows { get; set; }
        public bool HasDefaultBufferMaxRows
        {
            get
            {
                return DefaultBufferMaxRows != null;
            }
        }

        public DataFlow()
        { }

        public DataFlow(string name) : this()
        {
            this.Name = name;
        }

        public DataFlow(string name, int defaultBufferMaxRows) : this(name)
        {
            this.DefaultBufferMaxRows = defaultBufferMaxRows;
        }

        public DataFlow AddComponent(ISequence sequence)
        {
            base.AddComponent(sequence, "STOCK:PipelineTask");
            SSISObject = TaskHost.InnerObject as Pipe.MainPipe;
            AddComponentEventHandler();            
            if (HasDefaultBufferMaxRows)
                TaskHost.Properties["DefaultBufferMaxRows"].SetValue(TaskHost, DefaultBufferMaxRows ?? 0);            
            return this;
        }

        private void AddComponentEventHandler()
        {
            ComponentEventHandler = new DebugComponentEventHandler();
            SSISObject.Events = SSIS.DtsConvert.GetExtendedInterface(ComponentEventHandler as SSIS.IDTSComponentEvents);
        }              
      
    }
}
