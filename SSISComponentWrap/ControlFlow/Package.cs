using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSIS = Microsoft.SqlServer.Dts.Runtime;

namespace ALE.SSISComponentWrap.ControlFlow
{
    public class Package : SequenceBase, ISSISObjectWrap<SSIS.Package>
    {
        public SSIS.DTSProtectionLevel ProtectionLevel { get; set; }
                
        public SSIS.Package SSISObject { get; private set; }

        internal SSIS.Application App { get; private set; }

        public Package() : base()
        { }

        public Package(string name) : this()
        {
            this.Name = name;
            this.Description = name;
        }

        public Package(string name, ProtectionLevel protectionLevel) : this(name) 
        {
            this.ProtectionLevel = (SSIS.DTSProtectionLevel)protectionLevel;
        }

        public void CreatePackage()
        {
            App = new SSIS.Application();
            SSISObject = new SSIS.Package();
            SSISObject.ProtectionLevel = ProtectionLevel;
            base.SetSSISObjectBaseProperties(SSISObject);
            base.SetBaseProperties(SSISObject, SSISObject);
            
        }

        public void SavePackage(string fileName)
        {
            App.SaveToXml(fileName, SSISObject, null);
        }
    }

    public enum ProtectionLevel
    {
        DontSaveSensitive = 0,
        EncryptSensitiveWithUserKey = 1,
        EncryptSensitiveWithPassword = 2,
        EncryptAllWithPassword = 3,
        EncryptAllWithUserKey = 4,
        ServerStorage = 5,
    }
}
