public enum CardType
{
    Number,         // number card
    Multiplier,     // multiplier card
    Special         // special card
}

public enum MultiplierType
{
    PlusTwo,
    PlusFour,
    PlusSix,
    PlusEight,
    PlusTen,
    TimesTwo
}

public enum SpecialType
{
    Freeze,
    FlipThree,
    SecondChance
}

public class CardDefinition
{
    public string Name { get; }
    public CardType Type { get; }
    public int? Number { get; }
    public MultiplierType? Multiplier { get; }
    public SpecialType? Special { get; }

    // constructor for number cards
    public CardDefinition(string name, int? number = null)
    {
        Name = name;
        Type = CardType.Number;
        Number = number;
    }

    // constructor for multiplier cards
    public CardDefinition(string name, MultiplierType multiplier)
    {
        Name = name;
        Type = CardType.Multiplier;
        Multiplier = multiplier;
    }

    // constructor for special cards
    public CardDefinition(string name, SpecialType special)
    {
        Name = name;
        Type = CardType.Special;
        Special = special;
    }
}
