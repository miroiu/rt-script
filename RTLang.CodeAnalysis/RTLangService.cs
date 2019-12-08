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

        public static RTLangService Create(IAnalysisContext context)
            => new RTLangService(context);

        public static AnalysisContext NewContext(IExecutionContext context)
            => new AnalysisContext(context);

        public IReadOnlyList<Completion> GetCompletions(string code, int position)
        {
            if (position >= 0 && position <= code.Length)
            {
                using (var text = new SourceText(code.Substring(0, position)))
                {
                    var lexer = new Lexer.Lexer(text);
                    var parser = new SyntaxParser(lexer, false);
                    var analyzer = new AnalyzerService(_context, AnalyzerOptions.Completions, position);

                    try
                    {
                        _context.ClearMetadata();

                        do
                        {
                            var expr = parser.Next();
                            analyzer.Visit(expr);
                        }
                        while (parser.HasNext);

                        return analyzer.Completions;
                    }
                    catch (SyntaxException)
                    {
                        return new List<Completion>();
                    }
                    catch (LexerException)
                    {
                        return new List<Completion>();
                    }
                    catch (Exception ex)
                    {
                        throw new AnalysisException(ex.Message);
                    }
                }
            }

            return new List<Completion>();
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
