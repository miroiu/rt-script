namespace RTScript.Language.Parser
{
    public enum OperatorPrecedence
    {
        None = -1,
        Assignment = 1,
        Equality = 2,
        Addition = 3,
        Multiplication = 4,
        Prefix = 5
    }
}
