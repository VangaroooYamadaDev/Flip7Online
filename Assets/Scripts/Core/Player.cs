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

    public void ResetStatus()
    {
        Status = PlayerStatus.Active;
    }

    public bool IsActive => Status == PlayerStatus.Active;
}
