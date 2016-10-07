using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Dts.Runtime;

namespace ALE.SSISComponentWrap.DataFlowComponents
{
    public class ComponentInfo
    {
        public List<string> InstallatedPipelineComponents()
        {
            List<string> result = new List<string>();
            Application application = new Application();
            PipelineComponentInfos componentInfos = application.PipelineComponentInfos;
            foreach (PipelineComponentInfo componentInfo in componentInfos)
            {
                result.Add(componentInfo.Name + "\t" + componentInfo.CreationName);
            }
            return result;
        }
    }
}
