using UnityEngine;

public enum CardType
{
    Number,         // 数字カード
    Multiplier,     // 倍率カード
    Special         // 特殊カード
}

public class CardDefinition
{
    public string Name { get; }
    public CardType Type { get; }
    public int? Number { get; }

    public CardDefinition(string name, CardType type, int? number = null)
    {
        Name = name;
        Type = type;
        Number = number;
    }
}
