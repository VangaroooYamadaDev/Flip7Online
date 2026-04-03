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

    public List<Card> DiscardHand()
    {
        var discarded = new List<Card>(Hand);
        Hand.Clear();
        Status = PlayerStatus.Active;
        return discarded;
    }

    public int HandScore()
    {
        int score = 0;
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
                switch (card.Definition.MultiplierType)
                {
                    case MultiplierType.PlusTwo:
                        score += 2;
                        break;
                    case MultiplierType.PlusFour:
                        score += 4;
                        break;
                    case MultiplierType.PlusSix:
                        score += 6;
                        break;
                    case MultiplierType.PlusEight:
                        score += 8;
                        break;
                    case MultiplierType.PlusTen:
                        score += 10;
                        break;
                    case MultiplierType.TimesTwo:
                        existTimesTwo = true;
                        break;
                }
                continue;
            }
        }

        return (existTimesTwo) ? score * 2 : score;
    }

    public void ResetStatus()
    {
        Status = PlayerStatus.Active;
    }

    public bool IsActive => Status == PlayerStatus.Active;
}
