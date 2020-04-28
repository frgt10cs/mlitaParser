﻿using System.Globalization;
using System.IO;
using System.Text;

namespace SimpleExpressionEngine
{
    public class Tokenizer
    {
        public Tokenizer(TextReader reader)
        {
            _reader = reader;
            NextChar();
            NextToken();
        }

        TextReader _reader;
        char _currentChar;
        Token _currentToken;       
        string _var;
        int _number;

        public Token Token
        {
            get { return _currentToken; }
        }
       
        public string Var
        {
            get { return _var; }
        }

        public int Number
        {
            get { return _number; }
        }

        // Read the next character from the input strem
        // and store it in _currentChar, or load '\0' if EOF
        void NextChar()
        {
            int ch = _reader.Read();
            _currentChar = ch < 0 ? '\0' : (char)ch;
        }

        // Read the next token from the input stream
        public void NextToken()
        {
            // Skip whitespace
            while (char.IsWhiteSpace(_currentChar))
            {
                NextChar();
            }

            // Special characters
            switch (_currentChar)
            {
                case '\0':
                    _currentToken = Token.EOF;
                    return;

                case '&':
                    NextChar();
                    _currentToken = Token.Minimum;
                    return;

                case '!':
                    NextChar();
                    _currentToken = Token.Negative;
                    return;               

                case '(':
                    NextChar();
                    _currentToken = Token.OpenParens;
                    return;

                case ')':
                    NextChar();
                    _currentToken = Token.CloseParens;
                    return;              
            }           

            // Identifier - starts with letter or underscore
            if (char.IsLetter(_currentChar))
            {
                var sb = new StringBuilder();

                // Accept letter, digit or underscore
                while (char.IsLetterOrDigit(_currentChar))
                {
                    sb.Append(_currentChar);
                    NextChar();
                }

                // Setup token
                _var = sb.ToString();
                _currentToken = Token.Var;
                return;
            }

            // Number?
            if (char.IsDigit(_currentChar) || _currentChar == '.')
            {
                // Capture digits/decimal point
                var sb = new StringBuilder();
                bool haveDecimalPoint = false;
                while (char.IsDigit(_currentChar) || (!haveDecimalPoint && _currentChar == '.'))
                {
                    sb.Append(_currentChar);
                    haveDecimalPoint = _currentChar == '.';
                    NextChar();
                }

                // Parse it
                _number = int.Parse(sb.ToString(), CultureInfo.InvariantCulture);
                _currentToken = Token.Number;
                return;
            }
        }
    }
}
