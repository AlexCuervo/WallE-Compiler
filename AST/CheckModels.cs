public static class FunctionParamsModels
{
    static Dictionary<string, returnType[]> paramModels = new();
    static Dictionary<string, returnType> functionReturn = new();
    public static Dictionary<string, returnType[]> GetModels => paramModels;
    public static Dictionary<string, returnType> GetReturn => functionReturn;
    public static void Init()
    {
        paramModels["GoTo"] = [returnType.boolean];
        paramModels["Spawn"] = [returnType.number, returnType.number];
        paramModels["Color"] = [returnType.text];
        paramModels["Size"] = [returnType.number];
        paramModels["DrawLine"] = [returnType.number, returnType.number, returnType.number];
        paramModels["IsColor"] = [returnType.text, returnType.number, returnType.number];
        paramModels["IsBrushColor"] = [returnType.text];
        paramModels["IsBrushSize"] = [returnType.number];
        paramModels["GetCanvasSize"] = [];
        paramModels["GetColorCount"] = [returnType.text, returnType.number, returnType.number, returnType.number, returnType.number];
        paramModels["GetActualX"] = [];
        paramModels["GetActualY"] = [];
        paramModels["DrawCircle"] = [returnType.number, returnType.number, returnType.number];
        paramModels["DrawRectangle"] = [returnType.number, returnType.number, returnType.number, returnType.number, returnType.number];
        paramModels["Fill"] = [];

        functionReturn["GoTo"] = returnType.none;
        functionReturn["Spawn"] = returnType.none;
        functionReturn["Color"] = returnType.none;
        functionReturn["Size"] = returnType.none;
        functionReturn["DrawLine"] = returnType.none;
        functionReturn["IsColor"] = returnType.number;
        functionReturn["IsBrushColor"] = returnType.number;
        functionReturn["IsBrushSize"] = returnType.number;
        functionReturn["GetCanvasSize"] = returnType.number;
        functionReturn["GetColorCount"] = returnType.number;
        functionReturn["GetActualX"] = returnType.number;
        functionReturn["GetActualY"] = returnType.number;
        functionReturn["DrawCircle"] = returnType.none;
        functionReturn["DrawRectangle"] = returnType.none;
        functionReturn["Fill"] = returnType.none;


    }

}
public enum returnType
{
    boolean,
    number,
    text,
    none
}