public abstract class AST
{
    public abstract void Print();
}

public abstract class Expression : AST
{

}

public class BinaryOp(AST left, Token op, AST right) : Expression
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
public class Number(int value) : Expression
{
    public int value = value;

    public override void Print()
    {
        System.Console.WriteLine(GetType());
    }
}
public class Boolean(bool value) : Expression
{
    public bool value = value;

    public override void Print()
    {
        System.Console.WriteLine(GetType());
    }
}
public class Identifier(string key) : Expression
{
    public string key = key;
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
public class Assign(AST id, AST value) : Expression
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
public class FunctionCall(AST key, AST param) : AST
{
    public AST key = key;
    public AST param = param;

    public override void Print()
    {
        System.Console.WriteLine(GetType());
        key.Print();
        param.Print();
    }
}

public class Param(AST element, AST elements) : AST
{
    public AST element = element;
    public AST elements = elements;



    public override void Print()
    {
        System.Console.WriteLine(GetType());
        element.Print();
        elements.Print();
    }
}