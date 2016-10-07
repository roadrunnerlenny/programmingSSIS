using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lib = ALE.SSISComponentWrap;
using LibCF = ALE.SSISComponentWrap.ControlFlow;
using LibDF = ALE.SSISComponentWrap.DataFlowComponents;

namespace ALE.ProgramSSIS
{
    class SimpleControlFlow
    {
        LibCF.Package _Package { get; set; }
        Lib.OLEDBConnectionManager _ConnectionManager { get; set; }
        LibCF.Sequence _Sequence { get; set; }
        Lib.Variable SQLResultVariable { get; set; }

        public void CreatePackage(string packageName)
        {
            _Package = new LibCF.Package(packageName, LibCF.ProtectionLevel.EncryptSensitiveWithUserKey);
            _Package.CreatePackage();
        }

        public void SavePackage(string fileName)
        {
            _Package.SavePackage(fileName);
        }

        public void CreatePackageContent()
        {
            CreateVariables();
            CreateParameters();
            CreateConnectionManagers();
            CreateExecutables();
        }

        private void CreateVariables()
        {
            new Lib.Variable("ExampleVar1", "", Lib.DataType.WSTR).AddComponent(_Package);
            SQLResultVariable = new Lib.Variable("ExampleVar2", 42, Lib.DataType.I4).AddComponent(_Package);
        }

        private void CreateParameters()
        {
            new Lib.PackageParameter("ExamplePar1", TypeCode.Int32).AddComponent(_Package);
            new Lib.PackageParameter("ExamplePar2", TypeCode.String).AddComponent(_Package);
        }

        private void CreateConnectionManagers()
        {
            //TODO Advance with Expression
            _ConnectionManager = new Lib.OLEDBConnectionManager("ConnMan",
                @"Data Source=.;Initial Catalog=Setup;Provider=SQLNCLI11.1;Integrated Security=SSPI;Auto Translate=False;"
                )
                .AddComponent(_Package);
        }

        private void CreateExecutables()
        {
            _Sequence = new LibCF.Sequence("Example for a sequence").AddComponent(_Package);

            new LibCF.SQLTask(
             "Example for a simple sql",
             _ConnectionManager)
             .AddComponent(_Sequence)
             .AddDirectInput("create table #test ( id int not null )");

            new LibCF.SQLTask(
              "Example for a sql with result",
              _ConnectionManager)
              .AddComponent(_Sequence)
              .AddDirectInput("Select 1")
              .SetResultSetType(LibCF.ResultSetType.SingleRow)
              .AddResultSet(new LibCF.SQLResultSet("0", SQLResultVariable));

            _Sequence.ConnectAllTasks();

            new LibCF.SQLTask(
              "Example for a sql with parameter",
              _ConnectionManager)
              .AddComponent(_Package)
              .AddDirectInput(@"
create table #test ( text nvarchar(50) not null )
insert into #test values( '?' )"
              )
              .AddParameter(new LibCF.SQLTaskParameter("0", LibCF.ParameterDirections.Input, Lib.DataType.WSTR, "ExampleVar1"));

            _Package.ConnectAllTasksWithExpression("@[User::ExampleVar2] == 1");
        }


    }
}
