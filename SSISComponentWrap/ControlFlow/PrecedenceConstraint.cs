using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSIS = Microsoft.SqlServer.Dts.Runtime;

namespace ALE.SSISComponentWrap.ControlFlow
{
    public class PrecedenceConstraint : ISSISObjectWrap<SSIS.PrecedenceConstraint>
    {
        public SSIS.Executable Predecessor { get; set; }
        public SSIS.Executable Ancestor { get; set; }
        public string Expression { get; set; }
        public SSIS.PrecedenceConstraint SSISObject { get; private set; }

        public bool HasExpression
        {
            get
            {
                return Expression != null;
            }
        }

        public PrecedenceConstraint()
        { }

        public PrecedenceConstraint(IExecutable predecessor, IExecutable ancestor) : this()
        {
            this.Predecessor = predecessor.Executable;
            this.Ancestor = ancestor.Executable;
        }

        public PrecedenceConstraint(IExecutable predecessor, IExecutable ancestor, string expression)
            : this(predecessor, ancestor) 
        {
            this.Expression = expression;
        }

        public PrecedenceConstraint AddComponent(ISequence sequence)
        {
            SSISObject = sequence.PrecedenceConstraints.Add(Predecessor, Ancestor);
            if (HasExpression)
            {
                SSISObject.EvalOp = Microsoft.SqlServer.Dts.Runtime.DTSPrecedenceEvalOp.ExpressionAndConstraint;
                SSISObject.Expression = Expression;                
            }
            return this;
        }
    }
}
