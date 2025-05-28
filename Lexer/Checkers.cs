public abstract class Checker : IChecker
{
    protected TokenType type; 

    abstract public bool CanProceed(string chain);

    abstract public bool Check(string chain);

    new public TokenType GetType() {
        return type;
    }
}

public class NumberChecker : Checker
{

    private char[] digits;

    public override bool CanProceed(string chain)
    {
        if (chain.Length == 0) return false;

        for (int i = 0; i < chain.Length; i++)
        {
            if (!digits.Contains(chain[i])) return false;
        }

        return true;
    }

    public override bool Check(string chain)
    {
        return CanProceed(chain);
    }

    public NumberChecker(char[] alphabet)
    {
        digits = alphabet;
        type = TokenType.number;
    }

}

public class OperatorChecker : Checker
{
    private char[] alphabet;

    private string[] valid;
    public override bool CanProceed(string chain)
    {
        if (chain.Length == 0) return false;

        for (int i = 0; i < chain.Length; i++)
        {
            if (!alphabet.Contains(chain[i])) return false;
        }
        return true;
    }


    public override bool Check(string chain)
    {
        return CanProceed(chain) && valid.Contains(chain);
    }

    public OperatorChecker(char[] alphabet, string[] valid)
    {
        this.alphabet = alphabet;
        this.valid = valid;
        type = TokenType.op;
    }
}

public class GroupChecker : Checker
{
    private char[] alphabet;
    public override bool CanProceed(string chain)
    {
        if (chain.Length == 0 || chain.Length > 1) return false;

        return alphabet.Contains(chain[0]);
    }

    public override bool Check(string chain)
    {
        return CanProceed(chain);
    }
    public GroupChecker(char[] alphabet)
    {
        this.alphabet = alphabet;
        type = TokenType.group;  
    }
}

public class WhiteSpaceChecker : Checker
{
    public override bool CanProceed(string chain)
    {
        if (chain.Length == 0) return false;

        foreach (char character in chain)
        {
            if (character != ' ') return false;
        }

        return true;
    }

    public override bool Check(string chain)
    {
        return CanProceed(chain);
    }

    public WhiteSpaceChecker()
    {
        type = TokenType.exclude;
    }
}

public class EndInstructionChecker : Checker
{
    private char end;
    public override bool CanProceed(string chain)
    {
        if (chain.Length == 0 || chain.Length >= 2) return false;
        return chain[0] == end;
    }

    public override bool Check(string chain)
    {
        return CanProceed(chain);
    }

    public EndInstructionChecker(char end)
    {
        this.end = end;
        type = TokenType.end;
    }
}
public class IdChecker : Checker
{

    private string[] keywords;
    public override bool CanProceed(string chain)
    {
        if (chain.Length == 0) return false;
        if (!char.IsLetter(chain[0])) return false;

        for (int i = 0; i < chain.Length; i++)
        {
            if (!(char.IsLetter(chain[i]) || char.IsDigit(chain[i]) || chain[i] == '-')) return false;
        }

        if (keywords.Contains(chain)) type = TokenType.keyword;
        else type = TokenType.id;

        return true;

    }

    public override bool Check(string chain)
    {
        return CanProceed(chain);
    }

    public IdChecker(string[] keywords)
    {
        this.keywords = keywords;
    }
}