#region AlphabetDeclaration
NumberChecker noChecker = new(['0', '1', '2', '3', '4', '5', '6', '7', '8', '9']);
OperatorChecker opChecker = new(['+', '-', '*', '/', '=', '<', '>', '%'], ["==", "+=", "-=", "**", "<=", ">=", "=", "+", "++", "-", "*", "/", "%"]);
GroupChecker groupChecker = new(['(', ')', '[', ']', '{', '}']);
EndInstructionChecker endInstructionChecker = new(';');
WhiteSpaceChecker whiteSpaceChecker = new();
IdChecker idChecker = new(["if", "else", "true", "false"]);
#endregion 

Lexer lexer = new([noChecker, opChecker, groupChecker, whiteSpaceChecker, endInstructionChecker, idChecker]);


string code = $"25000 / (45 * 3 + 15)";

lexer.LoadCode(code + " ");

#region GrammarDeclaration

#region Non-Terminals
GrammarSymbol Main = new("Main", null);
GrammarSymbol I = new("Instruction", null);

GrammarSymbol B = new("Bool", null);

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
GrammarSymbol equal = new("==", new(TokenType.op, "==", 0, 0));
#endregion
#region Productions
Production EA = new(E, [[T, X]]);
Production Term = new(T, [[F, Y]]);
Production XTerm = new(X, [[sum, T, X], [diff, T, X], [epsilon]]);
Production Factor = new(F, [[P, M]]);
Production YFactor = new(Y, [[mult, F, Y], [div, F, Y], [epsilon]]);
Production Power = new(P, [[groupOpn, E, groupCls], [number], [id]]);
Production MPower = new(M, [[pow, P, M], [epsilon]]);
#endregion

EAGrammar grammar = new([EA, Term, XTerm, Factor, YFactor, Power, MPower]);
#endregion

Parser parser = new(grammar, lexer);

AST program = parser.Parse(E);

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