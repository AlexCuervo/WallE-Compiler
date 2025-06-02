public class AST(List<AST> children, GrammarSymbol symbol)
{
    List<AST> children = children;
    public GrammarSymbol symbol = symbol;
    bool changed = true;

    public void Optimize(AST derivationNode)
    {
        do
        {
            changed = false;
            OptimizeNode(derivationNode);

        } while (changed);
    }

    void OptimizeNode(AST derivationNode)
    {
        if (derivationNode.children.Count == 0) return;
        if (derivationNode.children.Count == 1 && derivationNode.symbol.token == null)
        {
            derivationNode.symbol = new(derivationNode.children[0].symbol.name, derivationNode.children[0].symbol.token);
            derivationNode.children = derivationNode.children.Concat(derivationNode.children[0].children).ToList();
            derivationNode.children[0].symbol.name = "delete";
        }

        foreach (var child in derivationNode.children)
        {
            if (child.symbol.token != null)
            {
                if (child.symbol.token.type == TokenType.group || child.symbol.token.type == TokenType.end) child.symbol.name = "delete";

                if (derivationNode.symbol.token == null && child.symbol.token.type == TokenType.op)
                {
                    derivationNode.symbol = new(child.symbol.name, child.symbol.token);
                    child.symbol.name = "delete";
                    if (child.children.Count != 0) derivationNode.children = derivationNode.children.Concat(child.children).ToList();
                }
            }

            else
            {
                if (child.children.Count == 0) child.symbol.name = "delete";

            }
        }
        for (int i = derivationNode.children.Count - 1; i >= 0; i--)
            if (derivationNode.children[i].symbol.name == "delete")
            {
                derivationNode.children.Remove(derivationNode.children[i]);
                changed = true;

            }
        foreach (var child in derivationNode.children) OptimizeNode(child);


    }
    public void Print()
    {
        if (symbol.token != null) System.Console.WriteLine(symbol.token.literal + "      " + symbol.token.row + "      " + symbol.token.column);
        else System.Console.WriteLine(symbol.name);

        foreach (var child in children) child.Print();
    }

    public void AddChild(AST child)
    {
        children.Add(child);
    }

    public void Execute(AST Node){}
}

