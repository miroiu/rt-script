using RTLang.CodeAnalysis.Analyzers;
using RTLang.CodeAnalysis.Syntax;
using RTLang.Lexer;
using System;
using System.Collections.Generic;

namespace RTLang.CodeAnalysis
{
    public sealed class RTLangService
    {
        private readonly IAnalysisContext _context;

        private RTLangService(IAnalysisContext context)
            => _context = context;

        public static RTLangService Create(IExecutionContext context)
            => new RTLangService(new AnalysisContext(context));

        public IReadOnlyList<CompletionItem> GetCompletions(string code, int position)
        {
            if (position >= 0 && position <= code.Length)
            {
                using (var text = new SourceText(code.Substring(0, position)))
                {
                    var lexer = new Lexer.Lexer(text);
                    var parser = new SyntaxParser(lexer, false);

                    try
                    {
                        var analyzer = new AnalyzerService(_context, AnalyzerOptions.Completions, position);

                        if (lexer.Peek().Type == TokenType.EndOfCode)
                        {
                            var expr = parser.Next();
                            analyzer.Visit(expr);
                        }
                        else
                        {
                            while (parser.HasNext)
                            {
                                var expr = parser.Next();
                                analyzer.Visit(expr);
                            }
                        }

                        return analyzer.Completions;
                    }
                    catch (SyntaxException)
                    {
                        return new List<CompletionItem>();
                    }
                    catch (LexerException)
                    {
                        return new List<CompletionItem>();
                    }
                    catch (Exception ex)
                    {
                        throw new AnalysisException(ex.Message);
                    }
                }
            }

            return new List<CompletionItem>();
        }

        public IReadOnlyList<Diagnostic> GetDiagnostics(string code)
        {
            using (var text = new SourceText(code))
            {
                var lexer = new Lexer.Lexer(text);
                var parser = new SyntaxParser(lexer, true);

                var analyzer = new AnalyzerService(_context, AnalyzerOptions.Diagnostics);

                try
                {
                    _context.ClearMetadata();

                    while (parser.HasNext)
                    {
                        var expr = parser.Next();
                        analyzer.Visit(expr);
                    }

                    return analyzer.Diagnostics;
                }
                catch (SyntaxException ex)
                {
                    var result = new List<Diagnostic>(analyzer.Diagnostics.Count + 1);
                    result.AddRange(analyzer.Diagnostics);

                    var token = ex.Current;
                    var length = token.Text.Length;
                    var position = token.Position;

                    result.Add(new Diagnostic
                    {
                        Type = DiagnosticType.Error,
                        Position = position > code.Length ? code.Length : position,
                        Length = position + length > code.Length ? 0 : length,
                        Message = ex.Message
                    });

                    return result;
                }
                catch (LexerException ex)
                {
                    return new List<Diagnostic>
                    {
                        new Diagnostic
                        {
                            Message = ex.Message,
                            Position = text.Position,
                            Length = 1,
                            Type = DiagnosticType.Error
                        }
                    };
                }
                catch (Exception ex)
                {
                    throw new AnalysisException(ex.Message);
                }
            }
        }
    }
}
