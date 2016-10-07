using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALE.SSISComponentWrap.ControlFlow;
using SSIS = Microsoft.SqlServer.Dts.Runtime;

namespace ALE.SSISComponentWrap.ControlFlow
{
    public class SequenceBase : ExecutableBase, ISequence
    {
        public SSIS.Executables Executables { get; protected set; }

        public SSIS.PrecedenceConstraints PrecedenceConstraints { get; protected set; }

        public List<IExecutable> TaskList { get; protected set; }

        public SSIS.DtsEventHandlers EventHandlers { get; protected set; }

        public SequenceBase() : base()
        {
            TaskList = new List<IExecutable>();
        }

        public void SetBaseProperties(SSIS.IDTSSequence sequence, SSIS.EventsProvider eventsProvider)
        {            
            this.Executables = sequence.Executables;
            this.PrecedenceConstraints = sequence.PrecedenceConstraints;
            if (eventsProvider != null)
                this.EventHandlers = eventsProvider.EventHandlers;
        }

        public void SetSSISObjectBaseProperties(SSIS.DtsContainer container)
        {
            container.Name = Name;
            container.Description = Description;
            if (container.GetType() != typeof(SSIS.Package))
            {
                container.FailPackageOnFailure = true;
                container.FailParentOnFailure = true;
            }
        }

        public void AddTask(IExecutable task)
        {
            TaskList.Add(task); 
        }
        
        public void ConnectAllTasksWithExpression(string expression)
        {
            if (TaskList.Count > 1)
            {
                for (int index = 1; index < TaskList.Count; index++)
                {
                    new PrecedenceConstraint(TaskList[index - 1], TaskList[index], expression).AddComponent(this);
                }
            }
        }

        public void ConnectAllTasks()
        {
            if (TaskList.Count > 1)
            {
                for (int index = 1; index < TaskList.Count; index++)
                {
                    new PrecedenceConstraint(TaskList[index - 1], TaskList[index]).AddComponent(this);
                }
            }
        }

        public EventHandlerWrap GetPreExecuteEventHandler()
        {
            return new EventHandlerWrap(EventHandlers.Add(EventHandlerWrap.OnPreExecute) as SSIS.DtsEventHandler);
        }

        public EventHandlerWrap GetPostExecuteEventHandler()
        {
            return new EventHandlerWrap(EventHandlers.Add(EventHandlerWrap.OnPostExecute) as SSIS.DtsEventHandler);
        }

        public EventHandlerWrap GetErrorEventHandler()
        {
            return new EventHandlerWrap(EventHandlers.Add(EventHandlerWrap.OnError) as SSIS.DtsEventHandler);
        }
    }
}
