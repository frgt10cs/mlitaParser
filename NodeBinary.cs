﻿using System;
using System.Collections.Generic;

namespace SimpleExpressionEngine
{
    // NodeBinary for binary operations such as Add, Subtract etc...
    class NodeBinary : Node
    {
        // Constructor accepts the two nodes to be operated on and function
        // that performs the actual operation
        public NodeBinary(Node lhs, Node rhs, Func<int, int, int> op)
        {
            _lhs = lhs;
            _rhs = rhs;
            _op = op;
        }

        Node _lhs;                              // Left hand side of the operation
        Node _rhs;                              // Right hand side of the operation
        Func<int, int, int> _op;       // The callback operator

        public override int Eval(Dictionary<string, int> vars)
        {
            // Evaluate both sides
            var lhsVal = _lhs.Eval(vars);
            var rhsVal = _rhs.Eval(vars);

            // Evaluate and return
            var result = _op(lhsVal, rhsVal);
            return result;
        }
    }
}
