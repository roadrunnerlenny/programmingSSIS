using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSIS = Microsoft.SqlServer.Dts.Runtime;

namespace ALE.SSISComponentWrap.ControlFlow
{
    public class ForEachLoop : SequenceBase, ISSISObjectWrap<SSIS.ForEachLoop>
    {
        public SSIS.ForEachLoop SSISObject { get; private set; }

        public ForEachLoop()
            : base()
        { }

        public ForEachLoop(string name)
            : this()
        {
            this.Name = name;
            this.Description = name;
        }

        public ForEachLoop AddComponent(ISequence sequence)
        {
            base.AddComponent(sequence, "STOCK:FOREACHLOOP");
            SSISObject = Executable as SSIS.ForEachLoop;
            base.SetSSISObjectBaseProperties(SSISObject);
            base.SetBaseProperties(SSISObject, SSISObject);        
            return this;
        }

        public ForEachLoop SetADOEnumerator(Package package, Variable variable)
        {            
            SSIS.ForEachEnumeratorInfo ADOEnumeratorInfo = package.App.ForEachEnumeratorInfos["Foreach ADO Enumerator"];
            SSIS.ForEachEnumeratorHost ADOEnumeratorHost = ADOEnumeratorInfo.CreateNew();
            ADOEnumeratorHost.CollectionEnumerator = true;
            ADOEnumeratorHost.Properties["DataObjectVariable"].SetValue(ADOEnumeratorHost, variable.QualifiedName);            
            SSISObject.ForEachEnumerator = ADOEnumeratorHost;
            return this;
        }

        public ForEachLoop AddVariableMapping(Variable variable, int index)
        {
            SSIS.ForEachVariableMapping newMapping = SSISObject.VariableMappings.Add();
            newMapping.VariableName = variable.QualifiedName;
            newMapping.ValueIndex = index;
            return this;
        }
        
    }
}
