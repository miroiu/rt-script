﻿namespace RTLang.Expressions
{
    public class IdentifierExpression : Expression
    {
        public IdentifierExpression(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
