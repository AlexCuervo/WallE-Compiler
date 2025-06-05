public class Parser
{
    EAGrammar grammar;
    Lexer lexer;
    public Table parsingTable = new();
    Token? currentToken;
    Production currentProduction;
    GrammarSymbol? currentTerminal;
    bool isCodeRemaining = true;

    public Parser(EAGrammar grammar, Lexer lexer)
    {
        this.grammar = grammar;
        this.lexer = lexer;
        UpdateTerminal();
        currentProduction = grammar.GetProductions[0];
        GenerateParsingTable();
    }
    void GenerateParsingTable()
    {
        foreach (var production in grammar.GetProductions)
        {
            foreach (var element in grammar.GetFIRST(production.GetSymbol))
            {
                GrammarSymbol[] derivation = [new("discard", null)];

                if (element.token!.type == TokenType.epsilon)
                {
                    foreach (var item in grammar.GetFOLLOW(production.GetSymbol))
                    {
                        parsingTable[production.GetSymbol, item.name] = new Production(production.GetSymbol, [[new("epsilon", new(TokenType.epsilon, "epsilon", 0, 0))]] );
                    }
                }

                else
                {
                    if (production.GetDerivations.Count > 1)
                    {
                        foreach (var discard in production.GetDerivations)
                        {
                            foreach(var item in grammar.GetFIRST(discard[0]))
                            {
                                if (item.name == element.name)
                                {
                                    derivation = discard;
                                    break;
                                }
                            }
                        }
                        parsingTable[production.GetSymbol, element.name] = new(production.GetSymbol, [derivation]);
                    }
                    else
                    {
                        parsingTable[production.GetSymbol, element.name] = new(production.GetSymbol, [production.GetDerivations[0]]);
                    }
                }
            }
        }
    }
    void UpdateTerminal()
    {
        try
        {
            currentToken = lexer.Tokens.First();
        }
        catch (System.InvalidOperationException)
        {
            isCodeRemaining = false;
        }
        currentTerminal = new(currentToken!.literal, currentToken);

        switch (currentToken.type)
        {

            case TokenType.number:
                currentTerminal.name = "number";
                break;
            case TokenType.id:
                currentTerminal.name = "id";
                break;
            case TokenType.keyword:
                currentTerminal.name = "key";
                break;
            case TokenType.boolean:
                currentTerminal.name = "bool";
                break;
            case TokenType.text:
                currentTerminal.name = "text";
                break;
            default: break;
        }

    }
    public DerivationTree Parse(GrammarSymbol Expression)
    {
        DerivationTree program = new([], new(Expression.name, null));
        try
        {
            currentProduction = parsingTable[Expression, currentTerminal!.name];
        }
        catch (KeyNotFoundException)
        {
            throw new Exception($"syntax error at ({currentToken!.row},{currentToken.column})");
            // System.Console.WriteLine($"syntax error at ({currentToken!.row},{currentToken.column})");
            // while (currentTerminal!.token!.type != TokenType.end)
            // {
            //     UpdateTerminal();
            // }
        }



        foreach (var symbol in currentProduction.GetDerivations[0])
        {
            DerivationTree child = new([], new(symbol.name, symbol.token));
            if (symbol.token == null) child = Parse(symbol);
            else if (symbol.name == currentTerminal!.name)
            {
                if (child.symbol.token!.literal.Length != 0) child.symbol.token = new(currentTerminal.token!.type, currentTerminal.token.literal, currentTerminal.token.row, currentTerminal.token.column);
                UpdateTerminal();
            }
            else if (symbol.token.type == TokenType.epsilon) break;
            else throw new Exception("syntax error");

            program.AddChild(child);
            if (!isCodeRemaining) return program;
        }
        

        return program;
        
    }
}