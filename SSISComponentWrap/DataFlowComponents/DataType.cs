using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALE.SSISComponentWrap
{
    public enum DataType
    {
        EMPTY = 0,
        NULL = 1,
        I2 = 2,
        I4 = 3,
        R4 = 4,
        R8 = 5,
        CY = 6,
        DATE = 7,
        BOOL = 11,
        DECIMAL = 14,
        I1 = 16,
        UI1 = 17,
        UI2 = 18,
        UI4 = 19,
        I8 = 20,
        UI8 = 21,
        FILETIME = 64,
        GUID = 72,
        BYTES = 128,
        STR = 129,
        WSTR = 130,
        NUMERIC = 131,
        DBDATE = 133,
        DBTIME = 134,
        DBTIMESTAMP = 135,
        DBTIME2 = 145,
        DBTIMESTAMPOFFSET = 146,
        IMAGE = 301,
        TEXT = 302,
        NTEXT = 303,
        DBTIMESTAMP2 = 304
    }    
}
