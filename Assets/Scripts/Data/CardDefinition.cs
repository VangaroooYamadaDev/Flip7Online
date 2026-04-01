using UnityEngine;

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
    public MultiplierType MultiplierType { get; }
    public SpecialType SpecialType { get; }

    // constructor for number cards
    public CardDefinition(string name, CardType type, int? number = null)
    {
        Name = name;
        Type = type;
        Number = number;
    }

    // constructor for multiplier cards
    public CardDefinition(string name, MultiplierType multiplier)
    {
        Name = name;
        Type = CardType.Multiplier;
        MultiplierType = multiplier;
    }

    // constructor for number cards
    public CardDefinition(string name, SpecialType special)
    {
        Name = name;
        Type = CardType.Special;
        SpecialType = special;
    }
}
