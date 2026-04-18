using System.Collections.Generic;

public enum PlayerStatus
{
    Active,
    Burst,
    Fold,
    Frozen,
    Flip7
}

public class Player
{
    public string Name { get; }
    public PlayerStatus Status { get; private set; }
    public List<Card> Hand { get; } = new List<Card>();

    public Player(string name)
    {
        Name = name;
        Status = PlayerStatus.Active;
    }

    public void AddCard(Card card)
    {
        Hand.Add(card);
    }

    public void Burst()
    {
        Status = PlayerStatus.Burst;
    }

    public void Fold()
    {
        Status = PlayerStatus.Fold;
    }

    public void Freeze()
    {
        Status = PlayerStatus.Frozen;
    }

    public void Flip7()
    {
        Status = PlayerStatus.Flip7;
    }

    public Card DiscardLast()
    {
        Card last = Hand[Hand.Count - 1];
        Hand.RemoveAt(Hand.Count - 1);
        return last;
    }

    public List<Card> DiscardHand()
    {
        var discarded = new List<Card>(Hand);
        Hand.Clear();
        Status = PlayerStatus.Active;
        return discarded;
    }

    public void RemoveCard(Card card)
    {
        Hand.Remove(card);
    }

    public int HandScore()
    {
        int score = 0;
        int additional = 0;
        bool existTimesTwo = false;

        foreach (Card card in Hand)
        {
            if (card.Type == CardType.Special) continue;

            if (card.Type == CardType.Number)
            {
                score += card.Number.Value;
                continue;
            }

            if (card.Type == CardType.Multiplier)
            {
                switch (card.Definition.Multiplier.Value)
                {
                    case MultiplierType.PlusTwo:
                        additional += 2;
                        break;
                    case MultiplierType.PlusFour:
                        additional += 4;
                        break;
                    case MultiplierType.PlusSix:
                        additional += 6;
                        break;
                    case MultiplierType.PlusEight:
                        additional += 8;
                        break;
                    case MultiplierType.PlusTen:
                        additional += 10;
                        break;
                    case MultiplierType.TimesTwo:
                        existTimesTwo = true;
                        break;
                }
                continue;
            }
        }

        if (existTimesTwo) score *= 2;
        score += additional;

        return score;
    }

    public bool HasSecondChance()
    {
        return Hand.Find(c => c.Definition.Special == SpecialType.SecondChance) != null;
    }

    public List<Card> UseSecondChance(Card burstCard)
    {
        Card secondChance = Hand.Find(c => c.Definition.Special == SpecialType.SecondChance);
        Hand.Remove(secondChance);
        Hand.Remove(burstCard);
        return new List<Card> { secondChance, burstCard };
    }

    public void ResetStatus()
    {
        Status = PlayerStatus.Active;
    }

    public bool IsActive => Status == PlayerStatus.Active;
}
