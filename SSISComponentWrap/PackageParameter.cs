using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALE.SSISComponentWrap.ControlFlow;
using SSIS = Microsoft.SqlServer.Dts.Runtime;


namespace ALE.SSISComponentWrap
{
    public class PackageParameter : ISSISObjectWrap<SSIS.Parameter>
    {
        public string NameSpace = "Package";
        public string Name { get; set; }
        public TypeCode DataType { get; set; }
        public SSIS.Parameter SSISObject { get; private set; }

        public DataType SSISDataType
        {
            get
            {
                switch (DataType)
                {
                    case TypeCode.Int16: return ALE.SSISComponentWrap.DataType.I2;
                    case TypeCode.Int32: return ALE.SSISComponentWrap.DataType.I4;
                    case TypeCode.Int64: return ALE.SSISComponentWrap.DataType.I8;
                    case TypeCode.String: return ALE.SSISComponentWrap.DataType.WSTR;
                    default: return ALE.SSISComponentWrap.DataType.EMPTY;
                }
            }
        }
        public string QualifiedName
        {
            get
            {
                return string.Format("${0}::{1}", NameSpace, Name);
            }
        }

        public string QualifiedNameInBrackets
        {
            get
            {
                return string.Format("@[${0}::{1}]", NameSpace, Name);
            }
        }

        public object Value
        {
            get
            {
                return SSISObject.Value;
            }
            set {
                SSISObject.Value = value;
            }
        }

        public PackageParameter()
        { }

        public PackageParameter(string name, TypeCode dataType) : this()
        {
            this.Name = name;
            this.DataType = dataType;
        }      

        public PackageParameter AddComponent(Package package)
        {
            SSISObject = package.SSISObject.Parameters.Add(Name,DataType);
            return this;
        }
    }
}
