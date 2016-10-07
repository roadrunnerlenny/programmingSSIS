using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSIS = Microsoft.SqlServer.Dts.Runtime;
using ALE.SSISComponentWrap.ControlFlow;

namespace ALE.SSISComponentWrap
{
    public class EventHandlerWrap : SequenceBase, ISSISObjectWrap<SSIS.DtsEventHandler>
    {
        public static string OnPreExecute = "OnPreExecute";
        public static string OnPostExecute = "OnPostExecute";
        public static string OnError = "OnError";

        public SSIS.DtsEventHandler SSISObject { get; private set; }
               
        public EventHandlerWrap(SSIS.DtsEventHandler dtsEventHandler)
        {
            SSISObject = dtsEventHandler;            
            base.SetBaseProperties(SSISObject,null);      
        }
    }
}
