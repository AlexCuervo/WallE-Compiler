public static class Scope
{
    static Dictionary<string, AST> scope = [];
    static Dictionary<string, returnType> idMap = [];
    public static Dictionary<string, returnType> GetIdMap => idMap;
    public static Dictionary<string, AST> GetScopeMap => scope;


    public static void AddIdentifierReturn(string id, returnType returnType)
    {
        idMap[id] = returnType;
    }
    public static void AddToScope(string id, AST node)
    {
        scope[id] = node;
    }

}