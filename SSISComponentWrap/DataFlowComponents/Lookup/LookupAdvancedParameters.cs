using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALE.SSISComponentWrap.DataFlowComponents
{
    public class LookupAdvancedParameters
    {
        public string SqlCommandParam { get; set; }
        public bool HasSqlCommandParam
        {
            get
            {
                return !string.IsNullOrWhiteSpace(SqlCommandParam);
            }
        }

        public List<string> Parameters { get; private set; }

        public bool HasParameters
        {
            get
            {
                return Parameters != null && Parameters.Count > 0;
            }
        }

        public int? CacheSize32Bit { get; set; }
        public bool HasCacheSize32Bit
        {
            get
            {
                return CacheSize32Bit != null;
            }
        }
        public int? CacheSize64Bit { get; set; }
        public bool HasCacheSize64Bit
        {
            get
            {
                return CacheSize64Bit != null;
            }
        }

        public int? NoMatchCachePercentage { get; set; }
        public bool HasNoMatchCachePercentage
        {
            get
            {
                return NoMatchCachePercentage != null;
            }
        }

        public LookupAdvancedParameters()
        {
            this.Parameters = new List<string>();
        }

        public LookupAdvancedParameters(string customQuery) : this()
        {
            this.SqlCommandParam = customQuery;
        }

        public LookupAdvancedParameters AddParameter(string inputColumnName)
        {
            this.Parameters.Add(inputColumnName);
            return this;
        }

    }
}
