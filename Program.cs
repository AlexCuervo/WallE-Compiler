#region AlphabetDeclaration

NumberChecker noChecker = new(['0', '1', '2', '3', '4', '5', '6', '7', '8', '9']);
OperatorChecker opChecker = new(['+', '-', '*', '/', '=', '<', '>', '%', '|', '&', '!'], ["||", "&&", "==", "!=", "**", "<=", ">=", "+", "-", "<", ">", "*", "/", "%"]);
GroupChecker groupChecker = new(['(', ')', ',', '[', ']']);
EndInstructionChecker endInstructionChecker = new('\n');
WhiteSpaceChecker whiteSpaceChecker = new();
IdChecker idChecker = new(["Spawn", "Size", "DrawLine", "Color", "IsColor", "IsBrushColor", "IsBrushSize", "IsCanvasColor", "DrawCircle", "DrawRectangle", "GetActualX", "GetActualY", "GetCanvasSize", "GetColorCount"]);
TextChecker textChecker = new();
#endregion 

Lexer lexer = new([noChecker, opChecker, groupChecker, whiteSpaceChecker, endInstructionChecker, idChecker, textChecker]);
#region UI

string code = $"{'\n'}Spawn(0,2){'\n'} Spawn(1,2){'\n'}Color({'"'}Black{'"'}){'\n'} n <- 5{'\n'} k <- 3+3 * 10{'\n'} n <- k - n * 2{'\n'} actual-x <- GetActualX(){'\n'} i <- 0{'\n'} loop-1{'\n'} DrawLine(1,0,1){'\n'} i <- i + 1 {'\n'} is-brush-color-blue <- IsBrushColor({'"'}Blue{'"'}){'\n'} GoTo[loop-ends-here](is-brush-color-blue == 1){'\n'}{'\n'}{'\n'} GoTo[loop-1](i < 10){'\n'} Color({'"'}Blue{'"'}){'\n'} GoTo[loop-1](true){'\n'}{'\n'}{'\n'}loop-ends-here{'\n'}";
int size = 5;
string[,] canvas = new string[size, size];
int[] wallE = new int[2];

for (int i = 0; i < size; i++)
{
    for (int j = 0; j < size; j++)
    {
        canvas[i, j] = "White";
    }
}

#endregion

lexer.LoadCode(code + " ");

#region GrammarDeclaration

#region Non-Terminals

GrammarSymbol Program = new("Program", null);
GrammarSymbol I = new("Instruction", null);
GrammarSymbol MoreInst = new("MoreInstruction", null);
GrammarSymbol FC = new("FunctionCall", null);
GrammarSymbol FCGoTo = new("FunctionCallGoTo", null);
GrammarSymbol Param = new("FunctionCallParam", null);
GrammarSymbol Params = new("FunctionCallParams", null);
GrammarSymbol A = new("Assign", null);
GrammarSymbol Q = new("Q", null);
GrammarSymbol O = new("O", null);
GrammarSymbol Z = new("Z", null);
GrammarSymbol C = new("C", null);
GrammarSymbol V = new("V", null);
GrammarSymbol B = new("B", null);
GrammarSymbol N = new("N", null);
GrammarSymbol E = new("E", null);
GrammarSymbol T = new("T", null);
GrammarSymbol X = new("X", null);
GrammarSymbol F = new("F", null);
GrammarSymbol Y = new("Y", null);
GrammarSymbol P = new("P", null);
GrammarSymbol M = new("M", null);
#endregion

#region Terminals
GrammarSymbol mult = new("*", new(TokenType.op, "*", 0, 0));
GrammarSymbol div = new("/", new(TokenType.op, "/", 0, 0));
GrammarSymbol mod = new("%", new(TokenType.op, "%", 0, 0));
GrammarSymbol sum = new("+", new(TokenType.op, "+", 0, 0));
GrammarSymbol diff = new("-", new(TokenType.op, "-", 0, 0));
GrammarSymbol pow = new("**", new(TokenType.op, "**", 0, 0));
GrammarSymbol groupOpn = new("(", new(TokenType.group, "(", 0, 0));
GrammarSymbol keyOpn = new("[", new(TokenType.group, "[", 0, 0));
GrammarSymbol groupCls = new(")", new(TokenType.group, ")", 0, 0));
GrammarSymbol keyCls = new("]", new(TokenType.group, "]", 0, 0));
GrammarSymbol epsilon = new("/e", new(TokenType.epsilon, "epsilon", 0, 0));
GrammarSymbol id = new("id", new(TokenType.id, "id", 0, 0));
GrammarSymbol number = new("number", new(TokenType.number, "number", 0, 0));
GrammarSymbol keyword = new("key", new(TokenType.keyword, "key", 0, 0));
GrammarSymbol goTo = new("GoTo", new(TokenType.goTo, "GoTo", 0, 0));
GrammarSymbol text = new("text", new(TokenType.text, "text", 0, 0));
GrammarSymbol and = new("&&", new(TokenType.op, "&&", 0, 0));
GrammarSymbol or = new("||", new(TokenType.op, "||", 0, 0));
GrammarSymbol boolean = new("bool", new(TokenType.boolean, "bool", 0, 0));
GrammarSymbol equal = new("==", new(TokenType.op, "==", 0, 0));
GrammarSymbol notEqual = new("!=", new(TokenType.op, "!=", 0, 0));
GrammarSymbol eqBig = new(">=", new(TokenType.op, ">=", 0, 0));
GrammarSymbol eqLess = new("<=", new(TokenType.op, "<=", 0, 0));
GrammarSymbol lesser = new("<", new(TokenType.op, ">", 0, 0));
GrammarSymbol bigger = new(">", new(TokenType.op, "<", 0, 0));
GrammarSymbol endLine = new($"{'\n'}", new(TokenType.op, $"{'\n'}", 0, 0));
GrammarSymbol assign = new("<-", new(TokenType.op, "<-", 0, 0));
GrammarSymbol comma = new(",", new(TokenType.group, ",", 0, 0));
#endregion

#region Productions
Production Main = new(Program, [[I, MoreInst]]);
Production Instruction = new(I, [[FC, endLine], [FCGoTo, endLine], [A, endLine], [endLine]]);
Production MoreInstructions = new(MoreInst, [[I, MoreInst], [epsilon]]);
Production FunctionCall = new(FC, [[keyword, groupOpn, Param, groupCls]]);
Production FunctionCallGoTo = new(FCGoTo, [[goTo, keyOpn, id, keyCls, groupOpn, Param, groupCls]]);
Production PFunctionCall = new(Param, [[C, Params], [epsilon]]);
Production MoreParams = new(Params, [[comma, Param], [epsilon]]);
Production Assign = new(A, [[id, Q]]);
Production QAssign = new(Q, [[assign, C], [epsilon]]);
Production AndComp = new(C, [[O, V]]);
Production VAndComp = new(V, [[and, O, V], [epsilon]]);
Production OrComp = new(O, [[B, Z]]);
Production ZOrComp = new(Z, [[or, B, Z], [epsilon]]);
Production Bool = new(B, [[E, N]]);
Production NBool = new(N, [[equal, E, N], [notEqual, E, N], [bigger, E, N], [lesser, E, N], [eqBig, E, N], [eqLess, E, N], [epsilon]]);
Production EA = new(E, [[T, X]]);
Production Term = new(T, [[F, Y]]);
Production XTerm = new(X, [[sum, T, X], [diff, T, X], [epsilon]]);
Production Factor = new(F, [[P, M]]);
Production YFactor = new(Y, [[mult, F, Y], [div, F, Y], [mod, F, Y], [epsilon]]);
Production Power = new(P, [[groupOpn, C, groupCls], [number], [id], [boolean], [FC], [text]]);
Production MPower = new(M, [[pow, P, M], [epsilon]]);
#endregion

EAGrammar grammar = new([Main, Instruction, MoreInstructions, FunctionCall, FunctionCallGoTo, PFunctionCall, MoreParams, Assign, QAssign, AndComp, VAndComp, OrComp, ZOrComp, Bool, NBool, EA, Term, XTerm, Factor, YFactor, Power, MPower]);
#endregion

Parser parser = new(grammar, lexer);

DerivationTree program = new([], new("", null));
try
{
    program = parser.Parse(grammar.GetProductions[0].GetSymbol);
}
catch (ErrorDisplay error)
{
    throw new Exception(error.getMessage);
}

// program.Print();

// System.Console.WriteLine();
// System.Console.WriteLine();
// System.Console.WriteLine();

program = program.Optimize(program);

// program.Print();

// System.Console.WriteLine();
// System.Console.WriteLine();
// System.Console.WriteLine();

ASTBuilder.Init();

var programAST = program.GetAST();


// System.Console.WriteLine();
// System.Console.WriteLine();
// System.Console.WriteLine();

FunctionParamsModels.Init();
bool check = false;
try
{
    check = programAST.Check();
}
catch (ErrorDisplay error)
{
    throw new Exception(error.getMessage);
}
programAST.Print();

System.Console.WriteLine();
System.Console.WriteLine();
System.Console.WriteLine(check);