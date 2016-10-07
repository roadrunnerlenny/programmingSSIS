using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALE.SSISComponentWrap.ControlFlow
{
    public class SQLResultSet
    {
        public string ResultName { get; set; }
        public Variable Variable { get; set; }

        public SQLResultSet()
        { }

        public SQLResultSet(string resultName, Variable variable)
        {
            this.ResultName = resultName;
            this.Variable = variable;
        }
    }
}
