using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALE.SSISComponentWrap.DataFlowComponents
{
    public enum DataAccessMode
    {
        TableOrView = 0,
        TableOrViewFromVariable = 1,
        SqlCommand = 2,
        SqlCommandFromVariable = 3
    }
}
