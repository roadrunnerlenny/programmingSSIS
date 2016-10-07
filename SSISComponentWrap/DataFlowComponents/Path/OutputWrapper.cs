using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSIS = Microsoft.SqlServer.Dts.Runtime;
using Pipe = Microsoft.SqlServer.Dts.Pipeline.Wrapper;

namespace ALE.SSISComponentWrap.DataFlowComponents
{
    public sealed class OutputWrapper : ISSISObjectWrap<Pipe.IDTSOutput100>
    {
        public Pipe.IDTSOutput100 SSISObject { get; private set; }

        internal OutputWrapper(Pipe.IDTSOutput100 output)
        {
            this.SSISObject = output;
        }

        internal void SetErrorRowDisposition(RowDisposition rowDispostion) 
        {
            SSISObject.ErrorRowDisposition = (Pipe.DTSRowDisposition)rowDispostion;
        }
    }
   
}
