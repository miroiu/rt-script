using RTLang.Interpreter;
using RTLang.Lexer;
using RTLang.Operators;
using RTLang.Parser;
using System;

namespace RTLang
{
    public static class RTScript
    {
        public static void Execute(string code, IExecutionContext context)
        {
            try
            {
                var interpreter = new Interpreter.Interpreter(context);

                var source = new SourceText(code);
                var lexer = new Lexer.Lexer(source);
                var parser = new Parser.Parser(lexer);

                interpreter.Run(parser);
            }
            catch (LexerException lexEx)
            {
                context.Print($"[{lexEx.Line}, {lexEx.Column}]: {lexEx.Message}");
            }
            catch (ExecutionException rtScriptExx)
            {
                context.Print($"[{rtScriptExx.Expression.Token.Line}, {rtScriptExx.Expression.Token.Column}]: {rtScriptExx.Message}");
            }
            catch (ParserException rtScriptParserEx)
            {
                context.Print($"[{rtScriptParserEx.Token.Line}, {rtScriptParserEx.Token.Column}]: {rtScriptParserEx.Message}");
            }
            catch (RTLangException rtScriptEx)
            {
                context.Print(rtScriptEx.ToString());
            }
            catch
            {
                context.Print("Catastrophic failure!");
            }
        }

        public static void LoadOperators(Type hostType)
            => OperatorsCache.LoadOperators(hostType);
    }
}
