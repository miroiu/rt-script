using RTLang.Interpreter;
using RTLang.Lexer;
using RTLang.Operators;
using RTLang.Parser;
using System;

namespace RTLang
{
    public class RTScript
    {
        public IExecutionContext Context { get; }

        public RTScript(IExecutionContext context)
            => Context = context;

        public RTScript(IOutputStream outs) : this(NewContext(outs)) { }

        public void Execute(string code)
            => Execute(code, Context);

        public static void Execute(string code, IExecutionContext context)
        {
            try
            {
                var source = new SourceText(code);
                var lexer = new Lexer.Lexer(source);
                var parser = new Parser.Parser(lexer);
                var interpreter = new Interpreter.Interpreter(parser);

                interpreter.Run(context);
            }
            catch (LexerException lexEx)
            {
                context.Print($"[{lexEx.Line}, {lexEx.Column}]: {lexEx.Message}");
            }
            catch (ExecutionException rtScriptExx)
            {
                context.Print($"[{rtScriptExx.Line}, {rtScriptExx.Column}]: {rtScriptExx.Message}");
            }
            catch (ParserException rtScriptParserEx)
            {
                context.Print($"[{rtScriptParserEx.Line}, {rtScriptParserEx.Column}]: {rtScriptParserEx.Message}");
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

        public static IExecutionContext NewContext(IOutputStream output)
            => new ExecutionContext(output);

        public static void LoadOperators(Type hostType)
            => OperatorsCache.LoadOperators(hostType);
    }
}
