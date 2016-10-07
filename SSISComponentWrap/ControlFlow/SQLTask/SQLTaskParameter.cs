using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ALE.SSISComponentWrap.ControlFlow
{
    public class SQLTaskParameter
    {
        public string Name { get; set; }
        public ParameterDirections ParameterDirection { get; set; }
        public DataType DataType { get; set; }
        public string VariableName { get; set; }

        public SQLTaskParameter()
        { }

        public SQLTaskParameter(string name, ParameterDirections parameterDirection, DataType dataType, string variableName) : this()
        {
            this.Name = name;
            this.ParameterDirection = parameterDirection;
            this.DataType = dataType;
            this.VariableName = variableName;
        }
    }

    [Flags]
    public enum ParameterDirections
    {
        Input = 1,
        Output = 2,
        ReturnValue = 4,
    }
}
