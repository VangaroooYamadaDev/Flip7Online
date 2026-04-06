using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    private List<Card> _cards = new List<Card>();
    private List<Card> _discardPile = new List<Card>();
    private System.Random _random = new System.Random();

    public Deck()
    {
        Initialize();
        Shuffle();
    }

    private void Initialize()
    {
        // number cards
        for (int number = 0; number <= 12; number++)
        {
            int count = (number == 0) ? 1 : number;
            for (int i = 0; i < count; i++)
            {
                var def = new CardDefinition(number.ToString(), number);
                _cards.Add(new Card(def));
            }
        }

        // multiplier cards
        _cards.Add(new Card(new CardDefinition("+2", MultiplierType.PlusTwo)));
        _cards.Add(new Card(new CardDefinition("+4", MultiplierType.PlusFour)));
        _cards.Add(new Card(new CardDefinition("+6", MultiplierType.PlusSix)));
        _cards.Add(new Card(new CardDefinition("+8", MultiplierType.PlusEight)));
        _cards.Add(new Card(new CardDefinition("+10", MultiplierType.PlusTen)));
        _cards.Add(new Card(new CardDefinition("×2", MultiplierType.TimesTwo)));

        // special cards
        for (int i=0; i < 3; i++)
        {
            _cards.Add(new Card(new CardDefinition("flip three", SpecialType.FlipThree)));
            _cards.Add(new Card(new CardDefinition("freeze", SpecialType.Freeze)));
            _cards.Add(new Card(new CardDefinition("second chance", SpecialType.SecondChance)));
        }
    }

    private void Shuffle()
    {
        for (int i = _cards.Count - 1; i > 0; i--)
        {
            int j = _random.Next(0, i + 1);
            (_cards[i], _cards[j]) = (_cards[j], _cards[i]);
        }
    }

    public void Discard(Card card)
    {
        _discardPile.Add(card);
    }

    public void DiscardRange(List<Card> cards)
    {
        _discardPile.AddRange(cards);
    }

    private void RefillFromDiscard()
    {
        _cards.AddRange(_discardPile);
        _discardPile.Clear();
        Shuffle();
    }

    public Card Draw()
    {
        if (_cards.Count == 0)
        {
            if (_discardPile.Count == 0) return null;
            RefillFromDiscard();
        }

        Card top = _cards[0];
        _cards.RemoveAt(0);
        return top;
    }

    public int Count => _cards.Count;
}
