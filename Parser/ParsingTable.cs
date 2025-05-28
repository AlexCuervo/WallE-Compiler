

public class Table : ITable<GrammarSymbol, string, Production>
{
    Dictionary<GrammarSymbol, Dictionary<string, Production>> map = new();

    public void Print(GrammarSymbol nonTerminal, string terminal)
    {
        foreach (var value in map[nonTerminal][terminal].GetDerivations[0]) System.Console.WriteLine(map[nonTerminal][terminal].GetSymbol + "   " + value.name);

    }

    public Production this[GrammarSymbol nonTerminal, string terminal]
    {
        get
        {
            return map[nonTerminal][terminal];
        }
        set
        {
            if (!map.Keys.Contains(nonTerminal))
                map[nonTerminal] = new Dictionary<string, Production>();
            map[nonTerminal][terminal] = value;
        }
    }
}


public interface ITable<T, K, M>
{
    public M this[T index1, K index2] { get; set; }
}

