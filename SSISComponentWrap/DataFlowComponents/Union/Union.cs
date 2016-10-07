using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALE.SSISComponentWrap.ControlFlow;
using SSIS = Microsoft.SqlServer.Dts.Runtime;
using Pipe = Microsoft.SqlServer.Dts.Pipeline.Wrapper;

namespace ALE.SSISComponentWrap.DataFlowComponents
{
    public class Union : ComponentBase
    {
        public Union()
        { }

        public Union(string name)
        {
            this.Name = name;
            this.Description = name;
        }

        public Union AddComponent(DataFlow dataFlowTask)
        {
            base.AddComponent(dataFlowTask.SSISObject, "DTSTransform.UnionAll.3");
            return this;
        }

        public Union RenameLastInput(string inputName)
        {
            Component.InputCollection[SSISInputs.Count - 1].Name = inputName;
            Component.InputCollection[SSISInputs.Count - 1].Description = inputName;
            return this;
        }


        public Union InsertOutputColumn(OutputColumn outputColumn, List<UnionInputColumn> unionInputColumns)
        {
            Pipe.IDTSOutputColumn100 newSSISOutputColumn = Component.OutputCollection[0].OutputColumnCollection.New();
            newSSISOutputColumn.Name = outputColumn.Name;
            newSSISOutputColumn.SetDataTypeProperties((SSIS.Wrapper.DataType)outputColumn.DataType, outputColumn.Length, outputColumn.Precision, outputColumn.Scale, outputColumn.CodePage);
            foreach (UnionInputColumn unionInputColumn in unionInputColumns)
            {
                base.AddReadOnlyInputColumnForInput(unionInputColumn.ColumnName, unionInputColumn.InputName);
                Pipe.IDTSInputColumn100 inputColumn = base.FindSSISInputColumn(unionInputColumn.ColumnName, unionInputColumn.InputName);
                inputColumn.CustomPropertyCollection[0].Value = newSSISOutputColumn.LineageID;
            }            

            return this;
        }

        public Union InsertOutputColumn(OutputColumn outputColumn, UnionInputColumn unionInputColumn)
        {
            return this.InsertOutputColumn(outputColumn, new List<UnionInputColumn>() { unionInputColumn });
        }


    }   
}
