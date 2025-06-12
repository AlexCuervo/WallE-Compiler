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
            var value = node.symbol.token!;
            return new Identifier(value);
        };
        ASTBuilders[TokenType.text] = (DerivationTree node) =>
        {
            var value = node.symbol.token!.literal;
            return new Text(value);
        };
        ASTBuilders[TokenType.assign] = (DerivationTree node) =>
        {
            var children = node.GetChildren;
            return new Assign(children[0].symbol.token!, children[1].GetAST());
        };

        ASTBuilders[TokenType.functionCall] = (DerivationTree node) =>
        {
            var children = node.GetChildren;
            if (node.GetChildren.Count() == 1)
                return new FunctionCall(children[0].symbol.token!, null);
            else
                return new FunctionCall(children[0].symbol.token!, children[1].GetAST());
        };

        ASTBuilders[TokenType.goTo] = (DerivationTree node) =>
        {
            var children = node.GetChildren;
            if (children.Count() == 2)
                return new FunctionCallGoTo(children[1].symbol.token!, null);
            else if (children.Count() == 3)
                return new FunctionCallGoTo(children[1].symbol.token!, children[2].GetAST());
            else throw new Exception();
        };

        ASTBuilders[TokenType.param] = (DerivationTree node) =>
        {
            var children = node.GetChildren;
            if (children.Count() != 1)
                return new Param(children[0].GetAST(), children[1].GetAST());
            else
                return new Param(children[0].GetAST(), null);
        };
        ASTBuilders[TokenType.instructions] = (DerivationTree node) =>
        {
            var children = node.GetChildren;

            if (children.Count == 1)
                return new Instructions(children[0].GetAST(), null);
            else if (children.Count > 1)
                return new Instructions(children[0].GetAST(), children[1].GetAST());
            else return null!;    
            
        };
    }
    public static Func<DerivationTree, AST> GetASTBuilder(DerivationTree node)
    {
        TokenType tokenType = new();
        if (node.symbol.token == null || node.symbol.token.type == TokenType.goTo)
        {
            switch (node.symbol.name)
            {
                case "FunctionCallGoTo":
                    tokenType = TokenType.goTo;
                    break;
                case "GoTo":
                    tokenType = TokenType.keyword;
                    break;
                case "FunctionCall":
                    tokenType = TokenType.functionCall;
                    break;
                case "FunctionCallParam":
                    tokenType = TokenType.param;
                    break;
                case "MoreInstruction":
                    tokenType = TokenType.instructions;
                    break;
                case "Program":
                    tokenType = TokenType.instructions;
                    break;
            }
        }

        else tokenType = node.symbol.token!.type;
        return ASTBuilders[tokenType];
    }

}