using System.Collections.Generic;

namespace RTLang.CodeCompletion
{
    public class CompletionServiceResult
    {
        public CompletionServiceResult(IEnumerable<Completion> completions, IEnumerable<EvaluationError> errors)
        {
            Completions = completions;
            Errors = errors;
        }

        private CompletionServiceResult()
        {
            Completions = new List<Completion>();
            Errors = new List<EvaluationError>();
        }

        public static CompletionServiceResult Empty { get; } = new CompletionServiceResult();
        public IEnumerable<Completion> Completions { get; }
        public IEnumerable<EvaluationError> Errors { get; }
    }
}
