public abstract class AST
{
    public returnType returnType;
    public abstract bool Check();
    public abstract void Print();
}
public class BinaryOp(AST left, Token op, AST right) : AST
{
    public AST left = left;
    public Token op = op;
    public AST right = right;
    string[] booleans = ["||", "&&"];
    string[] comparations = ["==", "!=", ">=", "<=", "<", ">"];
    public override bool Check()
    {
        left.Check();
        right.Check();
        if (booleans.Contains(op.literal))
        {
            returnType = returnType.boolean;
            if (left.returnType != returnType.boolean || right.returnType != returnType.boolean) throw new ErrorDisplay($"({op.row},{op.column})This operation can only proceed between boolean values");
        }
        else if (comparations.Contains(op.literal))
        {
            returnType = returnType.boolean;
            if (op.literal == "!=" || op.literal == "==")
            {
                if (left.returnType != right.returnType) throw new ErrorDisplay($"({op.row},{op.column}) This operation can only proceed between same type of values");
            }
            else if (left.returnType != returnType.number || right.returnType != returnType.number) throw new ErrorDisplay($"({op.row},{op.column})This operation can only proceed between numeric values");
        }
        else
        {
            returnType = returnType.number;
            if (left.returnType != returnType.number || right.returnType != returnType.number) throw new ErrorDisplay($"({op.row},{op.column})This operation can only proceed between numeric values");
        }
        return true;
    }
    public override void Print()
    {
        System.Console.WriteLine(GetType() + "    " + op.literal);
        left.Print();
        right.Print();
    }
}
public class Number : AST
{
    public int value;

    public Number(int value)
    {
        this.value = value;
        returnType = returnType.number;
    }

    public override bool Check() => true;
    public override void Print()
    {
        System.Console.WriteLine(GetType() + "   " + value);
    }
}
public class Boolean : AST 
{
    public bool value;
    public Boolean(bool value)
    {
        this.value = value;
        returnType = returnType.boolean;
    }
    public override bool Check() => true;
    public override void Print()
    {
        System.Console.WriteLine(GetType() + "   " + value);
    }
}
public class Identifier(Token key) : AST 
{
    public Token key = key;
    public override bool Check()
    {
        if (Scope.GetIdMap.ContainsKey(key.literal)) returnType = Scope.GetIdMap[key.literal];
        else throw new ErrorDisplay($"({key.row},{key.column}) The local variable is undefined.");
        return true;
    }
    public override void Print()
    {
        System.Console.WriteLine(GetType() + "   " + key.literal);
    }
}
public class Text : AST
{
    string value;

    public Text(string value)
    {
        this.value = value[1..(value.Length - 1)];
        returnType = returnType.text;
    }
    public override bool Check() => true;
    public override void Print()
    {
        System.Console.WriteLine(GetType() + "  " + value);
    }
}
public class Key(string name) : AST
{
    public string name = name;

    public override bool Check()
    {
        throw new NotImplementedException();
    }
    public override void Print()
    {
        System.Console.WriteLine(GetType());
    }
}
public class Assign(Token id, AST value) : AST
{
    public Token id = id;
    public AST value = value;

    public override bool Check()
    {
        if (Scope.GetScopeMap.ContainsKey(id.literal) && !Scope.GetIdMap.ContainsKey(id.literal)) throw new ErrorDisplay($"({id.row},{id.column}) The variable is already existing as a label");
        else
        {
            Scope.AddToScope(id.literal, value);
            value.Check();
            Scope.AddIdentifierReturn(id.literal, value.returnType);
        }
        return true;
    }
    public override void Print()
    {
        System.Console.WriteLine(GetType());
        System.Console.WriteLine(id.literal);
        value.Print();
    }
}
public class FunctionCall : AST
{
    public Token key;

    public List<AST> parameters = [];

    public FunctionCall(Token key, AST? param)
    {
        this.key = key;
        if (param is Param p) parameters = p.parameters;
        
    }

    public override bool Check()
    {
        returnType = FunctionParamsModels.GetReturn[key.literal];
        int paramCount = FunctionParamsModels.GetModels[key.literal].Length;
        if (paramCount != parameters.Count) throw new ErrorDisplay($"({key.row},{key.column}) {key.literal} method must recieve {paramCount} parameter/s instead of {parameters.Count}");
        else
        {
            for (int i = 0; i < paramCount; i++)
            {
                parameters[i].Check();
                if (parameters[i].returnType != FunctionParamsModels.GetModels[key.literal][i]) throw new ErrorDisplay($"({key.row},{key.column}) Parameter number {i + 1} must have {FunctionParamsModels.GetModels[key.literal][i]} type");
            }
            return true;
        }
    }
    public override void Print()
    {
        System.Console.WriteLine(GetType());
        System.Console.WriteLine(key.literal);
        foreach (var i in parameters) i.Print();
    }
}
public class FunctionCallGoTo : AST
{
    public Token label;

    public List<AST> parameters = [];

    public FunctionCallGoTo(Token label, AST? param)
    {
        this.label = label;

        if (param is Param p) parameters = p.parameters;
    }

    public override bool Check()
    {
        if (Scope.GetScopeMap.ContainsKey(label.literal))
        {
            if (Scope.GetIdMap.ContainsKey(label.literal)) throw new ErrorDisplay($"({label.row},{label.column}) This label is already defined as a variable");
            else
            {
                int paramCount = FunctionParamsModels.GetModels["GoTo"].Length;
                if (paramCount != parameters.Count) throw new ErrorDisplay($"({label.row},{label.column}) GoTo method must recieve {paramCount} parameter/s instead of {parameters.Count}");
                else
                {
                    for (int i = 0; i < paramCount; i++)
                    {
                        parameters[i].Check();
                        if (parameters[i].returnType != FunctionParamsModels.GetModels["GoTo"][i]) throw new ErrorDisplay($"({label.row},{label.column}) Parameter number {i + 1} must have {FunctionParamsModels.GetModels["GoTo"][i]} type");
                    }
                    return true;
                }
            }
        }
        else throw new ErrorDisplay($"({label.row},{label.column}) This label doesn't exist");
    }
    public override void Print()
    {
        System.Console.WriteLine(GetType());
        System.Console.WriteLine(label.literal);
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

    public override bool Check()
    {
        throw new NotImplementedException();
    }
    public void GetParamsList(Param node, List<AST> accumulator)
    {
        accumulator.Add(node.element);
        if (node.elements is Param param) GetParamsList(param, accumulator);
        else if (node.elements is not null) accumulator.Add(node.elements);
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
        foreach (var inst in instructions) inst.Print();
    }
    public override bool Check()
    {
        foreach (var child in instructions)
        {
            if (child is Identifier id)
            {
                Scope.AddToScope(id.key.literal, id);
            }
        }

        for(int i = 0; i < instructions.Count; i++)
        {
            if (instructions[i] is not Identifier)
            {
                if (instructions[i] is FunctionCall functionCall && functionCall.key.literal == "Spawn" && i != 0)
                {
                    throw new ErrorDisplay($"({functionCall.key.row},{functionCall.key.column}) Only one call to 'Spawn' function is supported");
                }
                else if ((!(instructions[i] is FunctionCall) || (instructions[i] is FunctionCall function && function.key.literal != "Spawn")) && i == 0)
                {
                    throw new ErrorDisplay($"Code must start with 'Spawn' method");
                }
                instructions[i].Check();
            }
        }

        return true;
    }
}