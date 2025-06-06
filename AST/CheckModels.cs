public static class FunctionParamsModels
{
    public static Dictionary<string, returnType[]> paramModels = new();

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
    }
}
public enum returnType
{
    boolean,
    number,
    text,
}