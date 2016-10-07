using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSIS = Microsoft.SqlServer.Dts.Runtime;

namespace ALE.SSISComponentWrap.ControlFlow
{
    public class TaskBase : ExecutableBase
    {
        protected SSIS.TaskHost TaskHost { get; set; }

        protected override void AddComponent(ISequence sequence, string executableID)
        {
            base.AddComponent(sequence, executableID);
            TaskHost = Executable as SSIS.TaskHost; //alternativ: sequence.Executables[0] as SSIS.TaskHost;
            TaskHost.Name = Name;
            TaskHost.Description = Description;
        }

        public EventHandlerWrap GetPreExecuteEventHandler()
        {
            return new EventHandlerWrap(TaskHost.EventHandlers.Add(EventHandlerWrap.OnPreExecute) as SSIS.DtsEventHandler);
        }

        public EventHandlerWrap GetPostExecuteEventHandler()
        {
            return new EventHandlerWrap(TaskHost.EventHandlers.Add(EventHandlerWrap.OnPostExecute) as SSIS.DtsEventHandler);
        }

        public EventHandlerWrap GetErrorEventHandler()
        {
            return new EventHandlerWrap(TaskHost.EventHandlers.Add(EventHandlerWrap.OnError) as SSIS.DtsEventHandler);
        }
    }
}
