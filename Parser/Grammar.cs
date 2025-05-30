public class EAGrammar(List<Production> productions) : IGrammar
{
    List<Production> productions = productions;

    public List<Production> GetProductions => productions;
    public List<GrammarSymbol> GetFIRST(GrammarSymbol symbol)
    {
        List<GrammarSymbol> first = [];

        if (symbol.token != null) //if it is a terminal
        {
            first.Add(symbol);
            return first;
        }

        foreach (var production in productions)
        {
            if (production.GetSymbol.name == symbol.name)
            {
                foreach (var derivation in production.GetDerivations)
                {
                    var discard = GetFIRST(derivation[0]).ToList();
                    foreach (var element in discard) if (!first.Contains(element)) first.Add(element);
                }
            }
        }

        return first;
    }

    public List<GrammarSymbol> GetFOLLOW(GrammarSymbol symbol)
    {
        List<GrammarSymbol> follow = [];

        if (symbol.token != null) throw new Exception("FOLLOW group of a terminal is not allowed");

        foreach (var production in productions)
        {
            foreach (var derivation in production.GetDerivations)
            {
                for (int i = 0; i < derivation.Length; i++)
                {
                    if ( derivation[i].name == symbol.name)
                    {
                        if (i != derivation.Length - 1)
                        {
                            int index = i;
                            bool firstHasEpsilon = false;
                            var discard = GetFIRST(derivation[index + 1]).ToList();
                            do
                            {

                                foreach (var element in discard)
                                {
                                    if (element.token != null && element.token.type == TokenType.epsilon)
                                    {
                                        firstHasEpsilon = true;
                                        discard.Remove(element);
                                        break;
                                    }
                                    firstHasEpsilon = false;
                                }

                                index++;

                                if (firstHasEpsilon)
                                {
                                    if (index != derivation.Length - 1)
                                    {
                                        discard = discard.Concat(GetFIRST(derivation[index])).ToList();
                                    }
                                    else
                                    {
                                        discard = discard.Concat(GetFOLLOW(production.GetSymbol)).ToList();
                                    }
                                } 
                            } while (firstHasEpsilon);

                            
                            foreach (var element in discard) if (!follow.Contains(element)) follow.Add(element);
                        }
                        else if (production.GetSymbol.name != symbol.name)
                        {
                            var discard = GetFOLLOW(production.GetSymbol).ToList();
                            foreach (var element in discard) if (!follow.Contains(element)) follow.Add(element);
                        }
                    }
                }
            }
        }
        
        return follow;
    }
}

public class Production(GrammarSymbol symbol, List<GrammarSymbol[]> derivations)
{
    GrammarSymbol symbol = symbol;
    List<GrammarSymbol[]> derivations = derivations;

    public GrammarSymbol GetSymbol => symbol;
    public List<GrammarSymbol[]> GetDerivations => derivations;

}

public class GrammarSymbol(string name, Token? token)
{
    public string name = name; 
    public Token? token = token;  // if token is null then the symbol is non-terminal...
}
public interface IGrammar
{
    List<GrammarSymbol> GetFIRST(GrammarSymbol symbol);
    List<GrammarSymbol> GetFOLLOW(GrammarSymbol symbol);

}