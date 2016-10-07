using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSIS = Microsoft.SqlServer.Dts.Runtime;
using DtsSqlTask = Microsoft.SqlServer.Dts.Tasks.ExecuteSQLTask;

namespace ALE.SSISComponentWrap.ControlFlow
{
    public class SQLTask : TaskBase, ISSISObjectWrap<DtsSqlTask.ExecuteSQLTask>
    {
        public ConnectionManager ConnectionManager { get; set; }
        public string SqlStatement { get; set; }
        public SqlStatementType InputType { get; set; }

        public DtsSqlTask.ExecuteSQLTask SSISObject { get; private set; }

        public SQLTask()
        { }

        public SQLTask(string name, ConnectionManager connectionManager) : this()
        {
            this.Name = name;
            this.Description = name;
            this.ConnectionManager = connectionManager ;                       
        }        

        public SQLTask AddComponent(ISequence sequence)
        {
            base.AddComponent(sequence,"STOCK:SQLTask");            
            SSISObject = TaskHost.InnerObject as DtsSqlTask.ExecuteSQLTask;            
            SSISObject.Connection = ConnectionManager.SSISObject.Name;            
            return this;
            
        }

        public SQLTask AddDirectInput(string SqlStatement)
        {
            this.InputType = SqlStatementType.DirectInput;
            SSISObject.SqlStatementSourceType = (DtsSqlTask.SqlStatementSourceType)InputType;
            SSISObject.SqlStatementSource = SqlStatement;
            return this;
        }

        public SQLTask AddParameter(SQLTaskParameter parameter)
        {          
            DtsSqlTask.IDTSParameterBinding parameterBinding = SSISObject.ParameterBindings.Add();
            parameterBinding.ParameterName = parameter.Name;
            parameterBinding.ParameterDirection = (DtsSqlTask.ParameterDirections)parameter.ParameterDirection;
            parameterBinding.DataType = (int)parameter.DataType;
            parameterBinding.DtsVariableName = parameter.VariableName;
            return this;
        }

        public SQLTask SetResultSetType(ResultSetType resultSetType)
        {
            SSISObject.ResultSetType = (DtsSqlTask.ResultSetType)resultSetType;
            return this;
        }

        public SQLTask AddResultSet(SQLResultSet resultSet)
        {
            DtsSqlTask.IDTSResultBinding resultSetBinding = SSISObject.ResultSetBindings.Add();
            resultSetBinding.ResultName = resultSet.ResultName;
            resultSetBinding.DtsVariableName = resultSet.Variable.QualifiedName;
            return this;
        }     
    }

    public enum SqlStatementType
    {
        DirectInput = 1,
        FileConnection = 2,
        Variable = 3,
    }

    public enum ResultSetType
    {
        None = 1,
        SingleRow = 2,
        Full = 3,
        XML = 4,
    }


}
