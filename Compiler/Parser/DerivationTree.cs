public class DerivationTree(List<DerivationTree> children, GrammarSymbol symbol)
{
    List<DerivationTree> children = children;
    public GrammarSymbol symbol = symbol;
    bool changed = true;

    public List<DerivationTree> GetChildren => children;

    public AST GetAST()
    {
        var func = ASTBuilder.GetASTBuilder(this);
        return func(this);
    }
    public DerivationTree Optimize(DerivationTree root)
    {
        do
        {
            changed = false;
            root = OptimizeNode(root);
        } while (changed);

        do
        {
            changed = false;
            root = ReRoot(root);
        } while (changed);

        return root;
    }
    DerivationTree ReRoot(DerivationTree node)
    {
        var newNode = new DerivationTree([], node.symbol);
        DerivationTree? reRoot = null;

        for (int i = 0; i < node.children.Count - 1; ++i)
        {
            var child = node.children[i];
            newNode.children.Add(ReRoot(child));
        }

        if (node.children.Count > 0)
        {
            var child = node.children.Last();
            if (node.symbol.token != null && (node.symbol.token.type == TokenType.op || node.symbol.token.type == TokenType.assign) &&
                child.symbol.token != null && (child.symbol.token.type == TokenType.op || child.symbol.token.type == TokenType.assign)
                && node.children.Count > 2)
                reRoot = child;
            else
                newNode.children.Add(ReRoot(child));
        }

        if (reRoot is not null)
        {
            changed = true;
            reRoot.children.Insert(0, newNode);
            return reRoot;
        }

        return newNode;
    }
    DerivationTree OptimizeNode(DerivationTree node)
    {
        if (node.children.Count == 0)
            return node;
        if (node.children.Count == 1 && node.symbol.token == null && node.symbol.name != "FunctionCall" && node.symbol.name != "FunctionCallParam" && node.symbol.name != "Program")
        {
            changed = true;
            return OptimizeNode(node.children[0]);
        }

        var newNode = new DerivationTree([], node.symbol);

        foreach (var child in node.children)
        {
            if (child.symbol.token != null)
            {
                if (child.symbol.token.type == TokenType.group || child.symbol.token.type == TokenType.end)
                    continue;

                var newChild = OptimizeNode(child);

                if (newChild.symbol.token!.type == TokenType.op || newChild.symbol.token!.type == TokenType.assign)
                {
                    if (node.symbol.token == null && node.symbol.name != "MoreInstruction" && node.symbol.name != "Program" && node.symbol.name != "FunctionCall" && node.symbol.name != "FunctionCallParam" && node.symbol.name != "FunctionCallGoTo")
                    {
                        newNode.symbol = new(child.symbol.name, child.symbol.token);
                        newNode.children = [.. newNode.children, .. child.children];
                        changed = true;
                    }
                    else
                    {
                        newNode.children.Add(OptimizeNode(child));
                    }
                }
                else
                {
                    newNode.children.Add(OptimizeNode(child));
                }
            }
            else
            {
                if (child.children.Count != 0)
                    newNode.children.Add(OptimizeNode(child));
                else
                    changed = true;
            }
        }

        return newNode;
    }
    public void Print()
    {
        if (symbol.token != null) System.Console.WriteLine(symbol.token.literal + "      " + symbol.token.row + "      " + symbol.token.column);
        else System.Console.WriteLine(symbol.name);

        System.Console.WriteLine(children.Count);

        foreach (var child in children) child.Print();
    }
    public void AddChild(DerivationTree child)
    {
        children.Add(child);
    }
}

