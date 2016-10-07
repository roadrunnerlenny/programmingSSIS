using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALE.SSISComponentWrap.ControlFlow;
using SSIS = Microsoft.SqlServer.Dts.Runtime;

namespace ALE.SSISComponentWrap
{
    public class ConnectionManager : ISSISObjectWrap<SSIS.ConnectionManager>
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public string ConnectionString { get; set; }

        public SSIS.ConnectionManager SSISObject { get; protected set; }
        
        protected void AddComponent(Package package, string type)
        {
            SSISObject = package.SSISObject.Connections.Add(type);
            SSISObject.ConnectionString = ConnectionString;
            SSISObject.Name = Name;
            SSISObject.Description = Description;           
        }       

        protected void AddExpression(Expression expression) {
            SSISObject.SetExpression(expression.Property, expression.Value);
        }
    }
}
