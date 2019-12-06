using System.Collections.Generic;

namespace RTLang.CodeCompletion
{
    public class EvaluationBag
    {
        public EvaluationBag(ICompletionContext context, int completionPosition)
        {
            Completions = new List<Completion>();
            Errors = new List<EvaluationError>();
            Context = context;
            CompletionPosition = completionPosition;
        }

        public List<Completion> Completions { get; }
        public List<EvaluationError> Errors { get; }
        public ICompletionContext Context { get; }
        public int CompletionPosition { get; }
    }
}
