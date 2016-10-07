using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lib = ALE.SSISComponentWrap;
using LibCF = ALE.SSISComponentWrap.ControlFlow;
using LibDF = ALE.SSISComponentWrap.DataFlowComponents;

namespace ALE.ProgramSSIS
{
    class SimpleDataFlow
    {
        LibCF.Package _Package { get; set; }
        Lib.OLEDBConnectionManager _OLEDBConnectionManager { get; set; }
        Lib.FlatFileConnectionManager _FlatFileConnectionManager { get; set; }
        Lib.Variable _RowCountVar { get; set; }
        LibCF.DataFlow _DataFlow { get; set; }
        LibDF.RowCount _RowCount { get; set; }
        LibDF.FlatFileSource _FlatFileSource { get; set; }
        LibDF.OLEDBDestination _OLEDBDestination { get; set; }

        public const int DEFAULTBUFFERMAXROWS = 100000;

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
            CreateConnectionManagers();
            CreateExecutables();
        }

        private void CreateVariables()
        {
            _RowCountVar = new Lib.Variable("RowCount", 0, Lib.DataType.I4).AddComponent(_Package);
        }

        private void CreateConnectionManagers()
        {
            _OLEDBConnectionManager = new Lib.OLEDBConnectionManager("ConnMan",
                @"Data Source=.;Initial Catalog=Setup;Provider=SQLNCLI11.1;Integrated Security=SSPI;Auto Translate=False;"
                )
                .AddComponent(_Package);

            _FlatFileConnectionManager = new Lib.FlatFileConnectionManager("FlatFile Manager")
            {
                FileName = "\\notfound.csv",
                CodePage = 65001,
                HeaderRowsToSkip = 0,
                Format = "Delimited",
                TextQualifier = "\"",
                ColumnNamesInFirstDataRow = true,
                HeaderRowDelimiter = Lib.Delimiter.CR_LF,
                RowDelimiter = Lib.Delimiter.CR_LF,
                ColumnDelimiter = Lib.Delimiter.Semicolon
            }.AddComponent(_Package);
        }

        private void CreateExecutables()
        {
            _DataFlow = CreateDataFlow();

            CreateFlatFileSource(_DataFlow);
            CreateTransformation(_DataFlow);
            CreateDestination(_DataFlow);

            new LibDF.Path(_FlatFileSource.DefaultOutput, _RowCount.DefaultInput).Connect(_DataFlow);
            new LibDF.Path(_RowCount.DefaultOutput, _OLEDBDestination.DefaultInput).Connect(_DataFlow);

        }

        private LibCF.DataFlow CreateDataFlow()
        {
            return new LibCF.DataFlow(
                        "Example Data Flow", DEFAULTBUFFERMAXROWS)
                        .AddComponent(_Package);
        }

        private void CreateFlatFileSource(LibCF.DataFlow dataFlow)
        {
            _FlatFileSource = new LibDF.FlatFileSource(
               "Read from CSV",
               _FlatFileConnectionManager,
               true
               ).AddComponent(dataFlow)
               .SetErrorOutputForAllColumns(LibDF.RowDisposition.RD_FailComponent, LibDF.RowDisposition.RD_IgnoreFailure);

            _FlatFileConnectionManager.AddAdvancedColumn(new Lib.FlatFileColumn()
            {
                DataType = Lib.DataType.WSTR,
                OutputColumnWidth = 50,
                TextQualified = true,
                Name = "Column1",
                ColumnType = "Delimited",
            });
            _FlatFileConnectionManager.AdjustDelimiterForLastColumn();
            _FlatFileConnectionManager.AddExpression(new Lib.Expression("HeaderRowsToSkip", "2"));            
        }

        private void CreateTransformation(LibCF.DataFlow dataFlow)
        {
            _RowCount = new LibDF.RowCount("rc,hk - Count inserted rows", _RowCountVar)
                     .AddComponent(dataFlow);
        }

        private void CreateDestination(LibCF.DataFlow dataFlow)
        {
            _OLEDBDestination = new LibDF.OLEDBDestination(
                    "Destination",
                    new LibDF.OLEDBDestinationProperties(_OLEDBConnectionManager)
                    .SetTableOrViewName("[temp].[Test]")
                    ).AddComponent(dataFlow);
        }
    }

}
