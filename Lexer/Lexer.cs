public class Lexer(List<Checker> Checkers)
{
    private List<Checker> checkers = Checkers;
    private string code = "";
    private int codePointer = 0;
    private Token? currentToken;
    int column = 1;
    int row = 1;
    public void LoadCode(string Code)
    {
        code = Code;
        codePointer = 1;
    }

    public IEnumerable<Token> Tokens
    {
        get
        {
            while (codePointer <= code.Length)
            {
                column++;
                bool proceed = false;

                foreach (var checker in checkers)
                {
                    if (checker.CanProceed(code[0..codePointer])) proceed = true;
                }

                if (!proceed)
                {
                    foreach (var checker in checkers)
                    {
                        if (checker.Check(code[..(codePointer - 1)]))
                        {
                            proceed = true;
                            currentToken = new(checker.GetType(), code[0..(codePointer - 1)], row, column - codePointer);
                        }

                    }

                    if (!proceed)
                    {
                        throw new ErrorDisplay($"invalid expression at ({row},{column - codePointer})");
                        // code = code[1..code.Length];
                        // codePointer = 1;
                        // column--;
                        
                    }
                    else
                    {
                        code = code[(codePointer - 1)..code.Length];
                        codePointer = 1;
                        column--;
                        if (currentToken!.type == TokenType.end)
                        {
                            row++;
                            column = 1;
                        }
                        if (currentToken!.type != TokenType.exclude) yield return currentToken!;
                    }
                }
                else
                {
                    codePointer++;
                }
            }


        }

    }

}