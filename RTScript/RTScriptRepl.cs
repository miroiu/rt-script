using RTScript;
using RTScript.Interpreter;
using RTScript.Lexer;
using RTScript.Parser;

namespace RTScript
{
    public class RTScriptRepl
    {
        public RTScriptInterpreter Interpreter;
        public RTScriptConsole Console;

        public RTScriptRepl(RTScriptConsole console)
        {
            Console = console;
            Interpreter = new RTScriptInterpreter(console);
        }

        public void Evaluate(string code)
        {
            try
            {
                var source = new SourceText(code);
                var lexer = new RTScriptLexer(source);
                var parser = new RTScriptParser(lexer);

                Interpreter.Run(parser);
            }
            catch (LexerException lexEx)
            {
                Console.WriteLine($"[{lexEx.Line}, {lexEx.Column}]: {lexEx.Message}");
            }
            catch (ExecutionException rtScriptExx)
            {
                Console.WriteLine($"[{rtScriptExx.Expression.Token.Line}, {rtScriptExx.Expression.Token.Column}]: {rtScriptExx.Message}");
            }
            catch (ParserException rtScriptParserEx)
            {
                Console.WriteLine($"[{rtScriptParserEx.Token.Line}, {rtScriptParserEx.Token.Column}]: {rtScriptParserEx.Message}");
            }
            catch (RTScriptException rtScriptEx)
            {
                Console.WriteLine(rtScriptEx.ToString());
            }
            catch
            {
                Console.WriteLine("Catastrophic failure!");
            }
        }
    }
}
