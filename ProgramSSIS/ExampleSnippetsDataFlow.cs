using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lib = ALE.SSISComponentWrap;
using LibCF = ALE.SSISComponentWrap.ControlFlow;
using LibDF = ALE.SSISComponentWrap.DataFlowComponents;

namespace ALE.ProgramSSIS
{
    class ExampleSnippetsDataFlow
    {
        //DataFlow Snippets

        public void ConditionalSplit(LibCF.DataFlow dataFlow, LibDF.OutputWrapper output)
        {
            LibDF.ConditionalSplit cspl = new LibDF.ConditionalSplit("Catch value not null", "Valid value")
               .AddComponent(dataFlow);
            new LibDF.Path(output, cspl.DefaultInput).Connect(dataFlow);
            cspl.AddReadOnlyInputColumn("columnName");
            cspl.AddConditionalSplitItem("Value IsNull", "ISNULL(columnName)");            
        }

        public void DerivedColumn(LibCF.DataFlow dataFlow, LibDF.OutputWrapper output)
        {
            LibDF.DerivedColumn drvc = new LibDF.DerivedColumn("Derive some columns")
                .AddComponent(dataFlow);
            new LibDF.Path(output, drvc.DefaultInput).Connect(dataFlow);
            drvc.AddReadOnlyInputColumn("columnName");
            drvc.AddDerivedColumn(new LibDF.DerivedColumnItem("columnName",
                    "Expression", Lib.DataType.WSTR, 50));
            drvc.AddReadWriteInputColumn("column");
            drvc.AddDerivedColumn(new LibDF.DerivedColumnItem("excpressionAndColumn", "replaceColumnName"));
        }

        public void LookupEasy(LibCF.DataFlow dataFlow, LibDF.OutputWrapper output, Lib.ConnectionManager connection)
        {
            LibDF.Lookup Lookup = new LibDF.Lookup("Lookup Easy" , connection , "SQL")
                       .AddComponent(dataFlow);
            new LibDF.Path(output, Lookup.DefaultInput).Connect(dataFlow);
            Lookup.AddReadOnlyInputColumn("ColumnName")
                  .AddLookupMapping(new LibDF.LookupMapping("inputColumn", "lookupColumn"));
        }

        public void LookupAdvanced(LibCF.DataFlow dataFlow, LibDF.OutputWrapper output, Lib.ConnectionManager connection)
        {
            LibDF.Lookup lkp = new LibDF.Lookup(
             "Lookup",connection,"LOOKUP SQL",
             LibDF.NoMatchBehaviour.RedirectRowsToNoMatchOutput,
             LibDF.CacheType.Partial)
             .AddComponent(dataFlow);

            new LibDF.Path(output, lkp.DefaultInput).Connect(dataFlow);

            lkp.AddReadOnlyInputColumn("inputColumn");
            lkp.AddLookupMapping(new LibDF.LookupMapping("inputColumn","lookupColumn"));

            LibDF.LookupAdvancedParameters advPars = new LibDF.LookupAdvancedParameters("select Advanced SQL Query");
            advPars.AddParameter("Parameter");
            lkp.SetAdvancedParameters(advPars);           
        }

        public void OLEDBSource(LibCF.DataFlow dataFlow, LibDF.OutputWrapper output, Lib.OLEDBConnectionManager connection)
        {
            var srcprops = new LibDF.OLEDBSourceProperties(connection)
                    .SetSqlCommand("Some SQL");
            LibDF.OLEDBSource source = new LibDF.OLEDBSource(
                    "Source Name",
                    srcprops )
                    .AddComponent(dataFlow);
        }

        public void Union (LibCF.DataFlow dataFlow, LibDF.OutputWrapper output1, LibDF.OutputWrapper output2, LibDF.OutputWrapper output3)
        {
            LibDF.Union union = new LibDF.Union("Union Name")
               .AddComponent(dataFlow);

            union.RenameLastInput("Input 1");
            new LibDF.Path(output1, union.DefaultInput).Connect(dataFlow);
            union.RenameLastInput("Input 2");
            new LibDF.Path(output2, union.FindInput("Input 2")).Connect(dataFlow);
            union.RenameLastInput("Input 3");
            new LibDF.Path(output3, union.FindInput("Input 3")).Connect(dataFlow);            

        }

        private void OLEDBDestination(LibCF.DataFlow dataFlow, LibDF.OutputWrapper output, Lib.OLEDBConnectionManager connection)
        {
            LibDF.OLEDBDestination Destination = new LibDF.OLEDBDestination(
                "Destination Name",
                new LibDF.OLEDBDestinationProperties(connection)
                .SetTableOrViewName("dbo.Test")
                ).AddComponent(dataFlow);

            new LibDF.Path(output, Destination.DefaultInput).Connect(dataFlow);

            Destination.MapColumnsByMatchingNames();
            Destination.MapColumn(new LibDF.OLEDBDestinationMapping("ColumnNameA", "ColumnNameB"));
        }

        private void RowCount(LibCF.DataFlow dataFlow, LibDF.OutputWrapper output, Lib.Variable variable)
        {
            LibDF.RowCount RowCount = new LibDF.RowCount("RowCount", variable)
                  .AddComponent(dataFlow);
            new LibDF.Path(output, RowCount.DefaultInput).Connect(dataFlow);
        }
    }
}
