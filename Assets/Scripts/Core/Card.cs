using UnityEngine;

public class Card
{
    public CardDefinition Definition { get; }

    public Card(CardDefinition definition)
    {
        Definition = definition;
    }

    public string Name => Definition.Name;
    public CardType Type => Definition.Type;
    public int? Number => Definition.Number;
}
