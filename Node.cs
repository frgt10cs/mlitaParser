using System.Collections.Generic;

namespace SimpleExpressionEngine
{
    // Node - abstract class representing one node in the expression 
    public abstract class Node
    {
        public abstract int Eval(Dictionary<string, int> vars);
    }
}
