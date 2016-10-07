using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALE.SSISComponentWrap.ControlFlow;
using SSIS = Microsoft.SqlServer.Dts.Runtime;

namespace ALE.SSISComponentWrap.ControlFlow
{
    public interface ISequence 
    {
        SSIS.Executables Executables { get; }
        SSIS.PrecedenceConstraints PrecedenceConstraints { get; }
        List<IExecutable> TaskList { get; }        
        void AddTask(IExecutable task);

        void ConnectAllTasksWithExpression(string expression);
        void ConnectAllTasks();
    }
}
