using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lib = ALE.SSISComponentWrap;
using LibCF = ALE.SSISComponentWrap.ControlFlow;
using LibDF = ALE.SSISComponentWrap.DataFlowComponents;

namespace ALE.ProgramSSIS
{
    class EmptyPackage
    {
        public LibCF.Package _Package { get; set; }       

        public void CreatePackage(string packageName)
        {
            _Package = new LibCF.Package(packageName, LibCF.ProtectionLevel.EncryptSensitiveWithUserKey);
            _Package.CreatePackage();
        }

        public void SavePackage(string fileName)
        {
            _Package.SavePackage(fileName);
        }

        public void CreatePackageContent()
        {
            CreateVariables();
            CreateParameters();
            //CreateConnectionManagers();
            //CreateExecutables();
        }

        private void CreateVariables()
        {
            new Lib.Variable("ExampleVar1", "SomeValue", Lib.DataType.WSTR).AddComponent(_Package);
            new Lib.Variable("ExampleVar2", 0, Lib.DataType.I4).AddComponent(_Package);
        }

        private void CreateParameters()
        {
            new Lib.PackageParameter("ExamplePar1", TypeCode.Int32).AddComponent(_Package);
            new Lib.PackageParameter("ExamplePar2", TypeCode.String).AddComponent(_Package); 
        }
    }
}
