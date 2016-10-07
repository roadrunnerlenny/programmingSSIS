using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSIS = Microsoft.SqlServer.Dts.Runtime;

namespace ALE.SSISComponentWrap.ControlFlow
{
    public interface IExecutable
    {
        string Name { get; set; }
        string Description { get; set; }
        SSIS.Executable Executable { get; }      
    }    
}
