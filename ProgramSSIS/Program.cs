using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALE.ProgramSSIS
{
    /// <summary>
    /// Referenced assemblies in SSISComponentWrap:
    /// Microsoft.SqlServer.DTSPipelineWrap
    /// Microsoft.SqlServer.DTSRuntimeWrap
    /// Microsoft.SqlServer.ManagedDTS
    /// Microsoft.SqlServer.SQLTask
    /// Microsoft.SqlServer.ExpressionTask
    /// 
    /// To compile the SSISComponentWrap project, you need to set the (solution) target platform to x86.
    /// 
    /// 
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            CreateEmptyPackage();
            CreateSimpleControlFlow();
            CreateSimpleDataFlow();
        }

        static void CreateEmptyPackage()
        {
            EmptyPackage pkg = new EmptyPackage();
            pkg.CreatePackage("ExamplePackage");
            pkg.CreatePackageContent();
            pkg.SavePackage(@"D:\GitHub\ProgramSSIS\SSISExamples\EmptyPackage.dtsx");
        }

        static void CreateSimpleControlFlow()
        {
            SimpleControlFlow pkg = new SimpleControlFlow();
            pkg.CreatePackage("SimpleControlFlowPackage");
            pkg.CreatePackageContent();
            pkg.SavePackage(@"D:\GitHub\ProgramSSIS\SSISExamples\SimpleControlFlowPackage.dtsx");
        }

        static void CreateSimpleDataFlow()
        {
            SimpleDataFlow pkg = new SimpleDataFlow();
            pkg.CreatePackage("SimpleDataFlowPackage");
            pkg.CreatePackageContent();
            pkg.SavePackage(@"D:\GitHub\ProgramSSIS\SSISExamples\SimpleDataFlowPackage.dtsx");
        }

    }

    //Use HResultHelper To get some useful error codes for Dataflows!!!!!
    //Copied from here: http://blogs.msdn.com/b/mattm/archive/2009/08/03/looking-up-ssis-hresult-comexception-errorcode.aspx

    /*Usage:
     *  HResultHelper helper = new HResultHelper();

            try
            {
                CreatePackage();
            }
            catch (COMException ex)
            {
                string errorMessage = helper.GetErrorMessage(ex);
                Console.WriteLine(errorMessage);
            }

            helper.Dispose();
     * */
}
