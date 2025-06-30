

public static class ASTExecutor
{
    public static CanvasManager canvas = new();
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

    public static object Execute(AST node)
    {
        if (node is Number number) return number.value;

        else if (node is Boolean boolean) return boolean.value;

        else if (node is Text text) return text.value;

        else if (node is Identifier id) return variables[id.key.literal];

        else if (node is BinaryOp op)
        {
            switch (op.op.literal)
            {
                case "||":
                    return (bool)Execute(op.left) || (bool)Execute(op.right);
                case "&&":
                    return (bool)Execute(op.left) && (bool)Execute(op.right);
                case "==":
                    bool value = Equals(Execute(op.left), Execute(op.right));
                    return value;
                case "!=":
                    value = !Equals(Execute(op.left), Execute(op.right));
                    return value;
                case "<=":
                    return (int)Execute(op.left) <= (int)Execute(op.right);
                case ">=":
                    return (int)Execute(op.left) >= (int)Execute(op.right);
                case "<":
                    return (int)Execute(op.left) < (int)Execute(op.right);
                case ">":
                    return (int)Execute(op.left) > (int)Execute(op.right);
                case "+":
                    return (int)Execute(op.left) + (int)Execute(op.right);
                case "-":
                    return (int)Execute(op.left) - (int)Execute(op.right);
                case "*":
                    return (int)Execute(op.left) * (int)Execute(op.right);
                case "/":
                    if ((int)Execute(op.right) == 0) throw new ErrorDisplay($"({op.op.row}, {op.op.column}) Invalid operation. Attempted to divide by zero.");
                    return (int)Execute(op.left) / (int)Execute(op.right);
                case "%":
                    if ((int)Execute(op.right) == 0) throw new ErrorDisplay($"({op.op.row}, {op.op.column}) Invalid operation. Attempted to divide by zero.");
                    return (int)Execute(op.left) % (int)Execute(op.right);
                case "**":
                    return Math.Pow((int)Execute(op.left), (int)Execute(op.right));
                default: break;
            }
        }

        else if (node is Assign assign) variables[assign.id.literal] = Execute(assign.value);

        else if (node is FunctionCall function)
        {
            switch (function.key.literal)
            {
                case "Spawn":
                    var indexX = (int)Execute(function.parameters[0]);
                    var indexY = (int)Execute(function.parameters[1]);
                    if (indexX < 0 || indexX >= canvas.gridSize ||
                        indexY < 0 || indexY >= canvas.gridSize) throw new ErrorDisplay($"({function.key.row}, {function.key.column}) Index out of bounds of the canvas grid. Index must not be lesser than zero or bigger than the size of the collection.");
                    canvas.actualX = indexX;
                    canvas.actualY = indexY;
                    return "none";
                case "GetActualX":
                    return canvas.actualX;
                case "GetActualY":
                    return canvas.actualY;
                case "GetCanvasSize":
                    return canvas.gridSize;
                case "Size":
                    int size = (int)Execute(function.parameters[0]);
                    canvas.brushSize = size % 2 != 0 ? size : size - 1;
                    return "none";
                case "IsBrushSize":
                    if ((int)Execute(function.parameters[0]) == canvas.brushSize)
                        return 1;
                    else return 0;
                case "Color":
                    if (function.parameters[0] is Text color)
                    {
                        switch (color.value)
                        {
                            case "White":
                                canvas.actualColor = Color.White;
                                return "none";
                            case "Black":
                                canvas.actualColor = Color.Black;
                                return "none";
                            case "Green":
                                canvas.actualColor = Color.Green;
                                return "none";
                            case "Blue":
                                canvas.actualColor = Color.Blue;
                                return "none";
                            case "Yellow":
                                canvas.actualColor = Color.Yellow;
                                return "none";
                            case "Red":
                                canvas.actualColor = Color.Red;
                                return "none";
                            case "Violet":
                                canvas.actualColor = Color.Violet;
                                return "none";
                            case "Orange":
                                canvas.actualColor = Color.Orange;
                                return "none";
                            case "Pink":
                                canvas.actualColor = Color.Pink;
                                return "none";
                            case "DarkGreen":
                                canvas.actualColor = Color.DarkGreen;
                                return "none";
                            case "DarkViolet":
                                canvas.actualColor = Color.DarkViolet;
                                return "none";
                            case "DarkRed":
                                canvas.actualColor = Color.DarkRed;
                                return "none";
                            case "DarkBlue":
                                canvas.actualColor = Color.DarkBlue;
                                return "none";
                            case "Brown":
                                canvas.actualColor = Color.Brown;
                                return "none";
                            default: throw new ErrorDisplay($"({function.key.row}, {function.key.column}) Invalid Color parameter");
                        }
                    }
                    else if (function.parameters[0] is Identifier identifier)
                    {
                        switch (variables[identifier.key.literal])
                        {
                            case "White":
                                canvas.actualColor = Color.White;
                                return "none";
                            case "Black":
                                canvas.actualColor = Color.Black;
                                return "none";
                            case "Green":
                                canvas.actualColor = Color.Green;
                                return "none";
                            case "Blue":
                                canvas.actualColor = Color.Blue;
                                return "none";
                            case "Yellow":
                                canvas.actualColor = Color.Yellow;
                                return "none";
                            case "Red":
                                canvas.actualColor = Color.Red;
                                return "none";
                            case "Violet":
                                canvas.actualColor = Color.Violet;
                                return "none";
                            case "Orange":
                                canvas.actualColor = Color.Orange;
                                return "none";
                            case "Pink":
                                canvas.actualColor = Color.Pink;
                                return "none";
                            case "DarkGreen":
                                canvas.actualColor = Color.DarkGreen;
                                return "none";
                            case "DarkViolet":
                                canvas.actualColor = Color.DarkViolet;
                                return "none";
                            case "DarkRed":
                                canvas.actualColor = Color.DarkRed;
                                return "none";
                            case "DarkBlue":
                                canvas.actualColor = Color.DarkBlue;
                                return "none";
                            case "Brown":
                                canvas.actualColor = Color.Brown;
                                return "none";
                            default: throw new ErrorDisplay($"({function.key.row}, {function.key.column}) Invalid Color parameter");
                        }
                    }
                    else return "none";

                case "IsCanvasColor":
                    if (function.parameters[0] is Text key)
                    {
                        return key.value switch
                        {
                            "White" => canvas.IsCellColor(Color.White, (int)Execute(function.parameters[1]), (int)Execute(function.parameters[2])) ? 1 : 0,
                            "Black" => canvas.IsCellColor(Color.Black, (int)Execute(function.parameters[1]), (int)Execute(function.parameters[2])) ? 1 : 0,
                            "Green" => canvas.IsCellColor(Color.Green, (int)Execute(function.parameters[1]), (int)Execute(function.parameters[2])) ? 1 : 0,
                            "Blue" => canvas.IsCellColor(Color.Blue, (int)Execute(function.parameters[1]), (int)Execute(function.parameters[2])) ? 1 : 0,
                            "Yellow" => canvas.IsCellColor(Color.Yellow, (int)Execute(function.parameters[1]), (int)Execute(function.parameters[2])) ? 1 : 0,
                            "Red" => canvas.IsCellColor(Color.Red, (int)Execute(function.parameters[1]), (int)Execute(function.parameters[2])) ? 1 : 0,
                            "Violet" => canvas.IsCellColor(Color.Violet, (int)Execute(function.parameters[1]), (int)Execute(function.parameters[2])) ? 1 : 0,
                            "Orange" => canvas.IsCellColor(Color.Orange, (int)Execute(function.parameters[1]), (int)Execute(function.parameters[2])) ? 1 : 0,
                            "Pink" => canvas.IsCellColor(Color.Pink, (int)Execute(function.parameters[1]), (int)Execute(function.parameters[2])) ? 1 : 0,
                            "DarkGreen" => canvas.IsCellColor(Color.DarkGreen, (int)Execute(function.parameters[1]), (int)Execute(function.parameters[2])) ? 1 : 0,
                            "DarkViolet" => canvas.IsCellColor(Color.DarkViolet, (int)Execute(function.parameters[1]), (int)Execute(function.parameters[2])) ? 1 : 0,
                            "DarkRed" => canvas.IsCellColor(Color.DarkRed, (int)Execute(function.parameters[1]), (int)Execute(function.parameters[2])) ? 1 : 0,
                            "DarkBlue" => canvas.IsCellColor(Color.DarkBlue, (int)Execute(function.parameters[1]), (int)Execute(function.parameters[2])) ? 1 : 0,
                            "Brown" => (object)(canvas.IsCellColor(Color.Brown, (int)Execute(function.parameters[1]), (int)Execute(function.parameters[2])) ? 1 : 0),
                            _ => throw new ErrorDisplay($"({function.key.row}, {function.key.column}) Invalid Color parameter"),
                        };
                    }
                    else return "none";
                case "IsBrushColor":
                    if (function.parameters[0] is Text text1)
                    {
                        return text1.value switch
                        {
                            "White" => canvas.actualColor == Color.White ? 1 : 0,
                            "Black" => canvas.actualColor == Color.Black ? 1 : 0,
                            "Green" => canvas.actualColor == Color.Green ? 1 : 0,
                            "Blue" => canvas.actualColor == Color.Blue ? 1 : 0,
                            "Yellow" => canvas.actualColor == Color.Yellow ? 1 : 0,
                            "Red" => canvas.actualColor == Color.Red ? 1 : 0,
                            "Violet" => canvas.actualColor == Color.Violet ? 1 : 0,
                            "Orange" => canvas.actualColor == Color.Orange ? 1 : 0,
                            "Pink" => canvas.actualColor == Color.Pink ? 1 : 0,
                            "DarkGreen" => canvas.actualColor == Color.DarkGreen ? 1 : 0,
                            "DarkViolet" => canvas.actualColor == Color.DarkViolet ? 1 : 0,
                            "DarkRed" => canvas.actualColor == Color.DarkRed ? 1 : 0,
                            "DarkBlue" => canvas.actualColor == Color.DarkBlue ? 1 : 0,
                            "Brown" => (object)(canvas.actualColor == Color.Brown ? 1 : 0),
                            _ => throw new ErrorDisplay($"({function.key.row}, {function.key.column}) Invalid Color parameter"),
                        };
                    }
                    else return "none";
                case "GetColorCount":
                    if (function.parameters[0] is Text text2)
                    {
                        return text2.value switch
                        {
                            "White" => canvas.CountCellsInRectangle(Color.White, (int)Execute(function.parameters[1]), (int)Execute(function.parameters[2]), (int)Execute(function.parameters[3]), (int)Execute(function.parameters[4])),
                            "Black" => canvas.CountCellsInRectangle(Color.Black, (int)Execute(function.parameters[1]), (int)Execute(function.parameters[2]), (int)Execute(function.parameters[3]), (int)Execute(function.parameters[4])),
                            "Green" => canvas.CountCellsInRectangle(Color.Green, (int)Execute(function.parameters[1]), (int)Execute(function.parameters[2]), (int)Execute(function.parameters[3]), (int)Execute(function.parameters[4])),
                            "Blue" => canvas.CountCellsInRectangle(Color.Blue, (int)Execute(function.parameters[1]), (int)Execute(function.parameters[2]), (int)Execute(function.parameters[3]), (int)Execute(function.parameters[4])),
                            "Yellow" => canvas.CountCellsInRectangle(Color.Yellow, (int)Execute(function.parameters[1]), (int)Execute(function.parameters[2]), (int)Execute(function.parameters[3]), (int)Execute(function.parameters[4])),
                            "Red" => canvas.CountCellsInRectangle(Color.Red, (int)Execute(function.parameters[1]), (int)Execute(function.parameters[2]), (int)Execute(function.parameters[3]), (int)Execute(function.parameters[4])),
                            "Violet" => canvas.CountCellsInRectangle(Color.Violet, (int)Execute(function.parameters[1]), (int)Execute(function.parameters[2]), (int)Execute(function.parameters[3]), (int)Execute(function.parameters[4])),
                            "Orange" => canvas.CountCellsInRectangle(Color.Orange, (int)Execute(function.parameters[1]), (int)Execute(function.parameters[2]), (int)Execute(function.parameters[3]), (int)Execute(function.parameters[4])),
                            "Pink" => canvas.CountCellsInRectangle(Color.Pink, (int)Execute(function.parameters[1]), (int)Execute(function.parameters[2]), (int)Execute(function.parameters[3]), (int)Execute(function.parameters[4])),
                            "DarkGreen" => canvas.CountCellsInRectangle(Color.DarkGreen, (int)Execute(function.parameters[1]), (int)Execute(function.parameters[2]), (int)Execute(function.parameters[3]), (int)Execute(function.parameters[4])),
                            "DarkViolet" => canvas.CountCellsInRectangle(Color.DarkViolet, (int)Execute(function.parameters[1]), (int)Execute(function.parameters[2]), (int)Execute(function.parameters[3]), (int)Execute(function.parameters[4])),
                            "DarkRed" => canvas.CountCellsInRectangle(Color.DarkRed, (int)Execute(function.parameters[1]), (int)Execute(function.parameters[2]), (int)Execute(function.parameters[3]), (int)Execute(function.parameters[4])),
                            "DarkBlue" => canvas.CountCellsInRectangle(Color.DarkBlue, (int)Execute(function.parameters[1]), (int)Execute(function.parameters[2]), (int)Execute(function.parameters[3]), (int)Execute(function.parameters[4])),
                            "Brown" => (object)canvas.CountCellsInRectangle(Color.Brown, (int)Execute(function.parameters[1]), (int)Execute(function.parameters[2]), (int)Execute(function.parameters[3]), (int)Execute(function.parameters[4])),
                            _ => throw new ErrorDisplay($"({function.key.row}, {function.key.column}) Invalid Color parameter")
                        };
                    }
                    else return "none";
                case "Fill":
                    canvas.FloodFillRegion();
                    return "none";
                case "DrawLine":
                    canvas.PaintAlongDirection((int)Execute(function.parameters[0]), (int)Execute(function.parameters[1]), (int)Execute(function.parameters[2]));
                    return "none";
                case "DrawCircle":
                    int distance = (int)Execute(function.parameters[2]);
                    if (distance <= 0) throw new ErrorDisplay($"({function.key.row}, {function.key.column}) The circle radius can not be less than or equal to zero.");
                    canvas.DrawCircle((int)Execute(function.parameters[0]), (int)Execute(function.parameters[1]), distance);
                    return "none";
                case "DrawRectangle":
                    try
                    {
                        canvas.MoveAndDrawRectangle((int)Execute(function.parameters[0]), (int)Execute(function.parameters[1]), (int)Execute(function.parameters[2]), (int)Execute(function.parameters[3]), (int)Execute(function.parameters[4]));
                    }
                    catch (ErrorDisplay error)
                    {
                        throw new ErrorDisplay($"({function.key.row}, {function.key.column}) " + error.Message);
                    }
                    return "none";
                case "Move":
                    var x = (int)Execute(function.parameters[0]);
                    var y = (int)Execute(function.parameters[1]);
                    if (x < 0 || x >= canvas.gridSize ||
                        y < 0 || y >= canvas.gridSize) throw new ErrorDisplay($"({function.key.row}, {function.key.column}) Index out of bounds of the canvas grid. Index must not be lesser than zero or bigger than the size of the collection.");
                    canvas.actualX = x;
                    canvas.actualY = y;
                    return "none";
            }
        }

        else if (node is Instructions program)
        {
            for (int i = 0; i < program.instructions.Count; i++)
            {
                canvas.UpdateHighlightedCell();
                if (program.instructions[i] is Identifier) continue;

                else if (program.instructions[i] is FunctionCallGoTo callGoTo)
                {
                    if ((bool)Execute(callGoTo.parameters[0])) i = program.instructions.IndexOf(labels[callGoTo.label.literal]) - 1;
                    continue;
                }

                else Execute(program.instructions[i]);
            }
        }

        return "done";
    }
}