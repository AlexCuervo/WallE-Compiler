public class Lexer(List<Checker> Checkers)
{
    private List<Checker> checkers = Checkers;
    private string code = "";
    private int codePointer = 0;
    private Token? currentToken; 
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
                            currentToken = new(checker.GetType(), code[0..(codePointer - 1)], 0, 0);
                        }

                    }

                    if (!proceed)
                    {
                        System.Console.WriteLine("invalid expression");
                        code = code[1..code.Length];
                        codePointer = 1;
                        
                    }
                    else
                    {
                        code = code[(codePointer - 1)..code.Length]; // no se si quitarle solo uno o quitarle codePointer
                        codePointer = 1;
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