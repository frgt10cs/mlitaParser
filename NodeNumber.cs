using System.Collections.Generic;

namespace SimpleExpressionEngine
{
    // NodeNumber represents a literal number in the expression
    class NodeNumber : Node
    {
        public NodeNumber(int number)
        {
            _number = number;
        }

        int _number;             // The number

        public override int Eval(Dictionary<string, int> vars)
        {
            // Just return it.  Too easy.
            return _number;
        }
    }
}
