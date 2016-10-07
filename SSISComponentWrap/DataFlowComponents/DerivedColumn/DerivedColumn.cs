using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALE.SSISComponentWrap.ControlFlow;
using SSIS = Microsoft.SqlServer.Dts.Runtime;
using Pipe = Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Wrapper = Microsoft.SqlServer.Dts.Runtime.Wrapper;

namespace ALE.SSISComponentWrap.DataFlowComponents
{
    public class DerivedColumn : ComponentBase
    {        
        public DerivedColumn()
        { }

        public DerivedColumn(string name)
        {
            this.Name = name;
            this.Description = name;
        }

        public DerivedColumn AddComponent(DataFlow dataFlowTask)
        {
            base.AddComponent(dataFlowTask.SSISObject, "DTSTransform.DerivedColumn");           
            return this;
        }

        public DerivedColumn AddDerivedColumn(DerivedColumnItem derivedColumnItem)
        {
            if (!derivedColumnItem.IsReplacingColumn)
            {
                Pipe.IDTSOutputColumn100 col = SSISDefaultOutput.OutputColumnCollection.New();
                col.Name = derivedColumnItem.Name;
                col.SetDataTypeProperties((Wrapper.DataType)derivedColumnItem.DataType, derivedColumnItem.Length, derivedColumnItem.Precision, derivedColumnItem.Scale, derivedColumnItem.CodePage);
                col.ExternalMetadataColumnID = 0;
                col.ErrorRowDisposition = Pipe.DTSRowDisposition.RD_FailComponent;
                col.TruncationRowDisposition = Pipe.DTSRowDisposition.RD_FailComponent;
                Pipe.IDTSCustomProperty100 propEx = col.CustomPropertyCollection.New();
                propEx.Name = "Expression";
                propEx.Value = derivedColumnItem.Expression;
                Pipe.IDTSCustomProperty100 propFex = col.CustomPropertyCollection.New();
                propFex.Name = "FriendlyExpression";
                propFex.Value = derivedColumnItem.Expression;
            }
            else
            {
                    Pipe.IDTSInputColumn100 col = base.FindSSISInputColumn(derivedColumnItem.ReplaceColumnName);
                    col.ExternalMetadataColumnID = 0;
                    col.ErrorRowDisposition = Pipe.DTSRowDisposition.RD_FailComponent;
                    col.TruncationRowDisposition = Pipe.DTSRowDisposition.RD_FailComponent;
                    Pipe.IDTSCustomProperty100 propEx = col.CustomPropertyCollection["Expression"];
                    propEx.Value = derivedColumnItem.Expression;
                    Pipe.IDTSCustomProperty100 propFex = col.CustomPropertyCollection["FriendlyExpression"];
                    propFex.Value = derivedColumnItem.Expression;
                
            }

            return this;
        }

        public DerivedColumn AddDerivedColumns(List<DerivedColumnItem> derivedColumnItems)
        {
            derivedColumnItems.ForEach(item => AddDerivedColumn(item));
            return this;
        }

        public new DerivedColumn AddInputColumn(List<InputColumn> columns)
        {
            base.AddInputColumn(columns);
            return this;
        }

        public new DerivedColumn AddInputColumn(InputColumn column)
        {
            base.AddInputColumn(column);
            return this;
        }

        public new DerivedColumn AddReadOnlyInputColumn(string columnName)
        {
            base.AddReadOnlyInputColumn(columnName);
            return this;
        }

        public new DerivedColumn AddReadWriteInputColumn(string columnName)
        {
            base.AddReadWriteInputColumn(columnName);
            return this;
        } 
       
    }
}
