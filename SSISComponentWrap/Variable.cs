using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALE.SSISComponentWrap.ControlFlow;
using SSIS = Microsoft.SqlServer.Dts.Runtime;

namespace ALE.SSISComponentWrap
{
    public class Variable : ISSISObjectWrap<SSIS.Variable>
    {
        public const string NameSpace = "User";
        public string Name { get; set; }
        public bool IsReadOnly { get; set; }
        public object DefaultValue { get; set; }
        public SSIS.Variable SSISObject { get; private set; }
        public DataType DataType { get; set; }
      
        public string QualifiedName
        {
            get
            {
                return string.Format("{0}::{1}", NameSpace, Name);
            }
        }

        public string QualifiedNameInBrackets
        {
            get
            {
                return string.Format("@[{0}::{1}]", NameSpace, Name);
            }
        }

        public Variable()
        {

        }

        public Variable(string name)
            : this()
        {
            this.Name = name;
        }

        public Variable(string name, object defaultValue)
            : this(name)
        {
            this.DefaultValue = defaultValue;
        }

        public Variable(string name, object defaultValue, DataType dataType)
            : this(name, defaultValue)
        {
            this.DataType = dataType;
        }

        public Variable AddComponent(Package package)
        {
            SSISObject = package.SSISObject.Variables.Add(Name,IsReadOnly,NameSpace,DefaultValue);           
            return this;
        }
    }

}
