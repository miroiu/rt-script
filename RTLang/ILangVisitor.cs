namespace RTLang
{
    public interface ILangVisitor<T>
    {
        void Visit(T host);
    }
}
