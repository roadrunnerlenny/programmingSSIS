using System;
using System.Collections.Generic;
using System.Linq;
using SSIS = Microsoft.SqlServer.Dts.Runtime;
using ExpTask = Microsoft.SqlServer.Dts.Tasks.ExpressionTask;

namespace ALE.SSISComponentWrap.ControlFlow
{
    public class ExpressionTask : TaskBase, ISSISObjectWrap<ExpTask.ExpressionTask>
    {        
        public ExpTask.ExpressionTask SSISObject { get; private set; }

        public ExpressionTask()
        { }

        public ExpressionTask(string name)
            : this()
        {
            this.Name = name;
            this.Description = name;
        }

        public ExpressionTask AddComponent(ISequence sequence)
        {
            base.AddComponent(sequence, typeof(Microsoft.SqlServer.Dts.Tasks.ExpressionTask.ExpressionTask).AssemblyQualifiedName);
            SSISObject = TaskHost.InnerObject as ExpTask.ExpressionTask;            
            return this;            
        }

        public ExpressionTask AddExpression(string expression)
        {
            SSISObject.Expression = expression; //alternativ: TaskHost.Properties["Expression"].SetValue(TaskHost, expression);            
            return this;
        }
        
    }
}
