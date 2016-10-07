using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSIS = Microsoft.SqlServer.Dts.Runtime;

namespace ALE.SSISComponentWrap.ControlFlow
{
    public class Sequence : SequenceBase, ISSISObjectWrap<SSIS.Sequence>
    {
        public SSIS.Sequence SSISObject { get; private set; }

        public Sequence() : base()
        {   }

        public Sequence(string name)
            : this()
        {
            this.Name = name;
            this.Description = name;
        }

        public Sequence AddComponent(ISequence sequence)
        {
            base.AddComponent(sequence, "STOCK:SEQUENCE");
            SSISObject = Executable as SSIS.Sequence;
            base.SetSSISObjectBaseProperties(SSISObject); 
            base.SetBaseProperties(SSISObject, SSISObject);                  
            return this;
        }

        
    }
}
