using RTScript.Language;
using RTScript.Language.Interpreter;
using RTScript.Language.Lexer;
using RTScript.Language.Parser;
using System.Windows.Input;

namespace RTScript.Editor
{
    public class RTScriptApp : RTScriptObservable
    {
        public RTScriptApp()
        {
            RunCommand = new RTScriptCommand(Run);
            Console = new RTScriptConsole();
        }

        public RTScriptConsole Console { get; }
        public ICommand RunCommand { get; }

        private void Run()
        {
            try
            {
                var code = Console.ReadLine();

                var source = new SourceText(code);
                var lexer = new RTScriptLexer(source);
                var parser = new RTScriptParser(lexer);

                var interpreter = new RTScriptInterpreter(Console);
                interpreter.Run(parser);
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
