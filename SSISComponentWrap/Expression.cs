using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSIS = Microsoft.SqlServer.Dts.Runtime;

namespace ALE.SSISComponentWrap
{
    public class Expression
    {
        public static string ConnectionStringProperty = "ConnectionString";

        public string Property { get; set; }
        public string Value { get; set; }

        public Expression(string property, string expression)
        {
            this.Property = property;
            this.Value = expression;
        }
    }
}
