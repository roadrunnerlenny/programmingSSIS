using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSIS = Microsoft.SqlServer.Dts.Runtime;
using Pipe = Microsoft.SqlServer.Dts.Pipeline.Wrapper;

namespace ALE.SSISComponentWrap.DataFlowComponents
{
    public sealed class InputWrapper : ISSISObjectWrap<Pipe.IDTSInput100>
    {
        public Pipe.IDTSInput100 SSISObject { get; private set; }

        internal InputWrapper(Pipe.IDTSInput100 input)
        {
            this.SSISObject = input;
        }
    }
}
