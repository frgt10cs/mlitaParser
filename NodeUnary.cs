﻿using System;
using System.Collections.Generic;

namespace SimpleExpressionEngine
{
    // NodeUnary for unary operations such as Negate
    class NodeUnary : Node
    {
        // Constructor accepts the two nodes to be operated on and function
        // that performs the actual operation
        public NodeUnary(Node rhs, Func<int, int> op)
        {
            _rhs = rhs;
            _op = op;
        }

        Node _rhs;                              // Right hand side of the operation
        Func<int, int> _op;               // The callback operator

        public override int Eval(Dictionary<string, int> vars)
        {
            // Evaluate RHS
            var rhsVal = _rhs.Eval(vars);

            // Evaluate and return
            var result = _op(rhsVal);
            return result;
        }
    }
}
