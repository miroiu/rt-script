﻿using RTLang.Expressions;

namespace RTLang.Parser
{
    public interface IParslet
    {
        Expression Accept(Parser parser);
    }
}
