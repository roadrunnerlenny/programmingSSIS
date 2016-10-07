using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALE.SSISComponentWrap
{
    interface ISSISObjectWrap<T>
    {
        T SSISObject { get; }
    }
}
