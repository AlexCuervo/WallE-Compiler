public abstract class AST
{
    public abstract void Print();
}
public class BinaryOp(AST left, Token op, AST right) : AST
{
    public AST left = left;
    public Token op = op;
    public AST right = right;

    public override void Print()
    {
        System.Console.WriteLine(GetType());
        left.Print();
        right.Print();
    }
}
public class Number(int value) : AST
{
    public int value = value;

    public override void Print()
    {
        System.Console.WriteLine(GetType());
    }
}
public class Boolean(bool value) : AST
{
    public bool value = value;

    public override void Print()
    {
        System.Console.WriteLine(GetType());
    }
}
public class Identifier(string key) : AST
{
    public string key = key;
    public override void Print()
    {
        System.Console.WriteLine(GetType());
    }
}

public class Text(string value) : AST
{
    string value = value;
    public override void Print()
    {
        System.Console.WriteLine(GetType());
    }
}
public class Key(string name) : AST
{
    public string name = name;

    public override void Print()
    {
        System.Console.WriteLine(GetType());
    }
}
public class Assign(AST id, AST value) : AST
{
    public AST id = id;
    public AST value = value;

    public override void Print()
    {
        System.Console.WriteLine(GetType());
        id.Print();
        value.Print();
    }
}
public class FunctionCall : AST
{
    public AST key;

    public List<AST> parameters = [];

    public FunctionCall(AST key, AST? param)
    {
        this.key = key;
        if (param is Param p)parameters = p.parameters;
    }
    public override void Print()
    {
        System.Console.WriteLine(GetType());
        key.Print();
        foreach (var i in parameters) i.Print();
    }
}
public class FunctionCallGoTo : AST
{
    public AST key;
    public AST label;

    public List<AST> parameters = [];

    public FunctionCallGoTo(AST key, AST label, AST? param)
    {
        this.key = key;
        this.label = label;

        if (param is Param p) parameters = p.parameters;
    }
    public override void Print()
    {
        System.Console.WriteLine(GetType());
        key.Print();
        label.Print();
        foreach (var p in parameters) p.Print();
    }
}

public class Param : AST
{
    public AST element;
    public AST? elements;

    public List<AST> parameters = [];

    public Param(AST element, AST? elements)
    {
        this.element = element;
        this.elements = elements;

        GetParamsList(this, parameters);
    }

    public void GetParamsList(Param node, List<AST> accumulator)
    {
        accumulator.Add(node.element);
        if (node.elements is Param param) GetParamsList(param, accumulator);
        else if(node.elements is not null) accumulator.Add(node.elements);
    }
    public override void Print()
    {
        System.Console.WriteLine(GetType());
        foreach (var param in parameters) param.Print();
    }
}
public class Instructions : AST
{
    public AST first;
    public AST after;

    public List<AST> instructions = [];

    public Instructions(AST first, AST after)
    {
        this.first = first;
        this.after = after;

        GetInstructionsList(this, instructions);
    }

    private void GetInstructionsList(Instructions node, List<AST> accumulator)
    {
        accumulator.Add(node.first);
        if (node.after is Instructions inst) GetInstructionsList(inst, accumulator);
        else accumulator.Add(node.after);
    }
    public override void Print()
    {
        System.Console.WriteLine(GetType() + "   " + instructions.Count());
        foreach(var inst in instructions) inst.Print();
    }
}