using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lib = ALE.SSISComponentWrap;
using LibCF = ALE.SSISComponentWrap.ControlFlow;
using LibDF = ALE.SSISComponentWrap.DataFlowComponents;

namespace ALE.ProgramSSIS
{
    class ExampleSnippetsControlFlow
    {
        //Control Flow Snippets
        private void AddVariable(LibCF.Package package)
        {
            Lib.Variable Variable = new Lib.Variable("VarName", "Default", Lib.DataType.WSTR).AddComponent(package);
        }

        public void AddParameter(LibCF.Package package)
        {
            Lib.PackageParameter Parameter = new Lib.PackageParameter("Name", TypeCode.Int32).AddComponent(package);
        }

        public virtual void AddConnectionManagerWithExpression(LibCF.Package package)
        {
            Lib.Expression expression = new Lib.Expression(Lib.Expression.ConnectionStringProperty, "connection string");
            Lib.OLEDBConnectionManager ConnectionManager = new Lib.OLEDBConnectionManager("Name", "Default connection string")
              .AddComponent(package)
              .AddExpression(expression);
        }


        private LibCF.ForEachLoop AddForEachFileInfoLoop(LibCF.ISequence seq, LibCF.Package package, Lib.Variable variable)
        {
            LibCF.ForEachLoop felc = new LibCF.ForEachLoop("Loop name").AddComponent(seq)
                 .SetADOEnumerator(package, variable)
                 .AddVariableMapping(variable, 0);
            return felc;
        }

        public void PrecedenceConstraint(LibCF.Sequence seq, Lib.ConnectionManager connection)
        {
            LibCF.SQLTask sql1 = new LibCF.SQLTask("sql task", connection)
                .AddComponent(seq)
                .AddDirectInput("SQL");
            LibCF.SQLTask sql2 = new LibCF.SQLTask("sql task", connection)
                .AddComponent(seq)
                .AddDirectInput("SQL");

            new LibCF.PrecedenceConstraint(sql1, sql2, "condition, e.g. 1 > 0")
                    .AddComponent(seq);
        }

        public void EventHandler(LibCF.Package package, Lib.ConnectionManager connection)
        {
            LibCF.Sequence seq = new LibCF.Sequence("Sequence")
                .AddComponent(package);                

            Lib.EventHandlerWrap postExecute = seq.GetPostExecuteEventHandler();

            LibCF.SQLTask sqlTask = new LibCF.SQLTask("sql task", connection)
                .AddComponent(seq)
                .AddDirectInput("SQL");
            postExecute.AddTask(sqlTask);
        }
    }
}
