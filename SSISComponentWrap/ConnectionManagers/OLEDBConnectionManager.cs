using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALE.SSISComponentWrap.ControlFlow;
using SSIS = Microsoft.SqlServer.Dts.Runtime;


namespace ALE.SSISComponentWrap
{
    public class OLEDBConnectionManager : ConnectionManager
    {
        public OLEDBConnectionManager()
        { }

        public OLEDBConnectionManager(string name, string connectionString)
            : this()
        {
            this.Name = name;
            this.Description = name;
            this.ConnectionString = connectionString;
        }

        public OLEDBConnectionManager AddComponent(Package package)
        {
            base.AddComponent(package, "OLEDB");                  
            return this;
        }

        public new OLEDBConnectionManager AddExpression(Expression expression)
        {
            base.AddExpression(expression);
            return this;
        }
    }
}
