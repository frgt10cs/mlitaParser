using System;
using System.Collections.Generic;
using System.IO;

namespace SimpleExpressionEngine
{
    public class Parser
    {        
        // Constructor - just store the tokenizer
        public Parser(Tokenizer tokenizer, int k)
        {
            _tokenizer = tokenizer;
            _k = k;
        }

        Tokenizer _tokenizer;
        private static int _k;

        // Parse an entire expression and check EOF was reached
        public Node ParseExpression()
        {
            // For the moment, all we understand is add and subtract
            var expr = ParseMinimum();

            // Check everything was consumed
            if (_tokenizer.Token != Token.EOF)
                throw new SyntaxException("Unexpected characters at end of expression");

            return expr;
        }
       
        // Parse an sequence of add/subtract operators
        Node ParseMinimum()
        {
            // Parse the left hand side
            var lhs = ParseUnary();

            while (true)
            {
                // Work out the operator
                Func<int, int, int> op = null;
                if (_tokenizer.Token == Token.Minimum)
                {                    
                    op = Math.Min;
                }                

                // Binary operator found?
                if (op == null)
                    return lhs;             // no

                // Skip the operator
                _tokenizer.NextToken();

                // Parse the right hand side of the expression
                var rhs = ParseUnary();

                // Create a binary node and use it as the left-hand side from now on
                lhs = new NodeBinary(lhs, rhs, op);
            }
        }


        // Parse a unary operator (eg: negative/positive)
        Node ParseUnary()
        {
            while (true)
            {               
                // Negative operator
                if (_tokenizer.Token == Token.Negative)
                {
                    // Skip
                    _tokenizer.NextToken();

                    // Parse RHS 
                    // Note this recurses to self to support negative of a negative
                    var rhs = ParseUnary();

                    // Create unary node
                    return new NodeUnary(rhs, (a) => (a + 1) % _k);
                }

                // No positive/negative operator so parse a leaf node
                return ParseLeaf();
            }
        }

        // Parse a leaf node
        // (For the moment this is just a number)
        Node ParseLeaf()
        {
            // Is it a number?
            if (_tokenizer.Token == Token.Number)
            {
                var node = new NodeNumber(_tokenizer.Number);
                _tokenizer.NextToken();
                return node;
            }

            // Parenthesis?
            if (_tokenizer.Token == Token.OpenParens)
            {
                // Skip '('
                _tokenizer.NextToken();

                // Parse a top-level expression
                var node = ParseMinimum();

                // Check and skip ')'
                if (_tokenizer.Token != Token.CloseParens)
                    throw new SyntaxException("Missing close parenthesis");
                _tokenizer.NextToken();

                // Return
                return node;
            }

            // Variable
            if (_tokenizer.Token == Token.Var)
            {
                // Capture the name and skip it
                var name = _tokenizer.Var;
                _tokenizer.NextToken();
                return new NodeVariable(name);
            }

            // Don't Understand
            throw new SyntaxException($"Unexpect token: {_tokenizer.Token}");
        }


        #region Convenience Helpers
        
        // Static helper to parse a string
        public static Node Parse(string str, int k)
        {
            return Parse(new Tokenizer(new StringReader(str)), k);
        }

        // Static helper to parse from a tokenizer
        public static Node Parse(Tokenizer tokenizer, int k)
        {
            var parser = new Parser(tokenizer, k);
            return parser.ParseExpression();
        }

        #endregion
    }
}
