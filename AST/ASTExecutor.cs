
public static class ASTExecutor
{
    static Dictionary<string, AST> labels = [];
    static Dictionary<string, object> variables = [];
    
    public static Dictionary<string, AST> GetLabels => labels;
    public static Dictionary<string, object> GetVariables => variables;

    public static void AddLabel(string literal, Identifier id)
    {
        labels[literal] = id;
    }
    public static void AddVariable(string literal, object id)
    {
        variables[literal] = id;
    }
}