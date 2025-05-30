#region AlphabetDeclaration
NumberChecker noChecker = new(['0', '1', '2', '3', '4', '5', '6', '7', '8', '9']);
OperatorChecker opChecker = new(['+', '-', '*', '/', '=', '<', '>', '%', '|', '&'], ["||", "&&","==", "+=", "-=", "**", "<=", ">=", "->", "+", "++", "-", "<", ">", "*", "/", "%"]);
GroupChecker groupChecker = new(['(', ')', '[', ']']);
EndInstructionChecker endInstructionChecker = new('\n');
WhiteSpaceChecker whiteSpaceChecker = new();
IdChecker idChecker = new(["if", "else"]);
#endregion 

Lexer lexer = new([noChecker, opChecker, groupChecker, whiteSpaceChecker, endInstructionChecker, idChecker]);


string code = $" true && (45 < 200 || 62 > 20)";

lexer.LoadCode(code + " ");

#region GrammarDeclaration

#region Non-Terminals
GrammarSymbol Main = new("Main", null);
GrammarSymbol I = new("Instruction", null);

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
GrammarSymbol sum = new("+", new(TokenType.op, "+", 0, 0));
GrammarSymbol diff = new("-", new(TokenType.op, "-", 0, 0));
GrammarSymbol pow = new("**", new(TokenType.op, "**", 0, 0));
GrammarSymbol groupOpn = new("(", new(TokenType.op, "(", 0, 0));
GrammarSymbol groupCls = new(")", new(TokenType.op, ")", 0, 0));
GrammarSymbol epsilon = new("/e", new(TokenType.epsilon, "epsilon", 0, 0));
GrammarSymbol id = new("id", new(TokenType.id, "id", 0, 0));
GrammarSymbol number = new("number", new(TokenType.number, "number", 0, 0));
GrammarSymbol keyword = new("key", new(TokenType.keyword, "key", 0, 0));
GrammarSymbol and = new("&&", new(TokenType.op, "&&", 0, 0));
GrammarSymbol or = new("||", new(TokenType.op, "||", 0, 0));

GrammarSymbol boolean = new("bool", new(TokenType.boolean, "bool", 0, 0));
GrammarSymbol equal = new("==", new(TokenType.op, "==", 0, 0));
GrammarSymbol lesser = new("<", new(TokenType.op, ">", 0, 0));
GrammarSymbol bigger = new(">", new(TokenType.op, "<", 0, 0));
GrammarSymbol endLine = new($"{'\n'}", new(TokenType.op, $"{'\n'}", 0, 0));
GrammarSymbol assign = new("->", new(TokenType.op, "->", 0, 0));


#endregion
#region Productions

Production Asign = new(I, [[id, assign, B, endLine], [id, assign, E, endLine], [epsilon]]); /// corregir, convertir a LL(1)


Production OrComp = new(O, [[C, Z]]);
Production ZOrComp = new(Z, [[or, C, Z], [epsilon]]);
Production AndComp = new(C, [[B, V]]);
Production VAndComp = new(V, [[and, B, V], [epsilon]]);

Production Bool = new(B, [[E, N]]);
Production NBool = new(N, [[equal, E, N], [bigger, E, N], [lesser, E, N], [epsilon]]); 
Production EA = new(E, [[T, X]]);
Production Term = new(T, [[F, Y]]);
Production XTerm = new(X, [[sum, T, X], [diff, T, X], [epsilon]]);
Production Factor = new(F, [[P, M]]);
Production YFactor = new(Y, [[mult, F, Y], [div, F, Y], [epsilon]]);
Production Power = new(P, [[groupOpn, O, groupCls], [number], [id], [boolean]]);
Production MPower = new(M, [[pow, P, M], [epsilon]]);
#endregion

EAGrammar grammar = new([OrComp, ZOrComp, AndComp, VAndComp,Bool, NBool, EA, Term, XTerm, Factor, YFactor, Power, MPower]);
#endregion

Parser parser = new(grammar, lexer);

AST program = parser.Parse(O);

program.Print();

System.Console.WriteLine();
System.Console.WriteLine();
System.Console.WriteLine();

program.Optimize(program);

program.Print();


// var table = parser.parsingTable;
// GrammarSymbol[] terminals = [sum, diff, mult, div, groupOpn, groupCls, number, numberId];

// foreach (var production in grammar.GetProductions)
//     foreach (var terminal in terminals)
//     {
//         System.Console.Write("      " + production.GetSymbol.name + "    " + terminal.name + "   :  " + production.GetSymbol.name + "  ->  ");
//         table.Print(production.GetSymbol, terminal);
//     }