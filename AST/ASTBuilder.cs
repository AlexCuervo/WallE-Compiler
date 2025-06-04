public static class ASTBuilder
{
    private static Dictionary<TokenType, Func<DerivationTree, AST>> ASTBuilders = new();

    public static void Init()
    {

        ASTBuilders[TokenType.op] = (DerivationTree node) =>
        {
            var childrens = node.GetChildren;
            var token = node.symbol.token!;
            return new BinaryOp(childrens[0].GetAST(), token, childrens[1].GetAST());
        };

        ASTBuilders[TokenType.boolean] = (DerivationTree node) =>
        {
            var value = bool.Parse(node.symbol.token!.literal);
            return new Boolean(value);
        };

        ASTBuilders[TokenType.number] = (DerivationTree node) =>
        {
            var value = int.Parse(node.symbol.token!.literal);
            return new Number(value);
        };

        ASTBuilders[TokenType.keyword] = (DerivationTree node) =>
        {
            var value = node.symbol.token!.literal;
            return new Key(value);
        };

        ASTBuilders[TokenType.id] = (DerivationTree node) =>
        {
            var value = node.symbol.token!.literal;
            return new Identifier(value);
        };
        ASTBuilders[TokenType.assign] = (DerivationTree node) =>
        {
            var children = node.GetChildren;
            return new Assign(children[0].GetAST(), children[1].GetAST());
        };

        ASTBuilders[TokenType.functionCall] = (DerivationTree node) =>
        {
            var children = node.GetChildren;
            return new FunctionCall(children[0].GetAST(), children[1].GetAST());
        };

        ASTBuilders[TokenType.param] = (DerivationTree node) =>
        {
            var children = node.GetChildren;
            return new Param(children[0].GetAST(), children[1].GetAST());
        };
    }
    public static Func<DerivationTree, AST> GetASTBuilder(DerivationTree node)
    {
        TokenType tokenType = new();
        if (node.symbol.token == null)
        {
            switch (node.symbol.name)
            {
                case "FunctionCall":
                    tokenType = TokenType.functionCall;
                    break;
                case "FunctionCallParam":
                    tokenType = TokenType.param;
                    break;
                case "MoreInstruction":
                    tokenType = TokenType.instructions;
                    break;
                case "Main":
                    tokenType = TokenType.program;
                    break;
            }
        }

        else tokenType = node.symbol.token!.type;
        return ASTBuilders[tokenType];
    }
    
}