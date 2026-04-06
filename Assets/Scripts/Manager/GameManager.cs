using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<Player> _players = new List<Player>();
    private Deck _deck;
    private int _currentPlayerIndex;
    private Dictionary<Player, int> _scores = new Dictionary<Player, int>();
    private const int FLIP7BONUS = 15;

    [SerializeField]
    private int _playerCount = 3;
    [SerializeField]
    private int _flipThreeTargetIndex = 0;
    [SerializeField]
    private int _freezeTargetIndex = 0;
    [SerializeField]
    private int _secondChanceTargetIndex = 0;

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        _deck = new Deck();

        for (int i = 0; i < _playerCount; i++)
        {
            var player = new Player($"Player{i + 1}");
            _players.Add(player);
            _scores[player] = 0;
        }

        _currentPlayerIndex = 0;
    }

    public void DrawCard()
    {
        DrawCardForPlayer(_players[_currentPlayerIndex]);
    }

    public void DrawCardForPlayer(Player player)
    {
        Card drawnCard = _deck.Draw();
        if (drawnCard.Definition.Special == SpecialType.SecondChance
            && player.HasSecondChance())
        {
            // If the currentPlayer already has SecondChance, give away to other player
            Player target = FindSecondChanceTarget();
            if (target != null)
                target.AddCard(drawnCard);
            else
                _deck.Discard(drawnCard);
        }
        else
        {
            player.AddCard(drawnCard);
        }

        if (drawnCard.Type == CardType.Special)
        {
            switch (drawnCard.Definition.Special)
            {
                case SpecialType.FlipThree:
                    for (int i = 0; i < 3; i++)
                    {
                        DrawCardForPlayer(_players[_flipThreeTargetIndex]);
                    }
                    _deck.Discard(player.DiscardLast());
                    break;
                case SpecialType.Freeze:
                    Freeze(_players[_freezeTargetIndex]);
                    _deck.Discard(player.DiscardLast());
                    break;
            }
        }
        else
        {
            CheckBurst(player);
            CheckFlip7(player);
        }
    }

    public void RetirePlayer(Player player)
    {
        _scores[player] += player.HandScore();
        _deck.DiscardRange(player.DiscardHand());
    }

    public void Fold()
    {
        Player player = _players[_currentPlayerIndex];
        player.Fold();
        RetirePlayer(player);
    }

    public void Freeze(Player player)
    {
        player.Freeze();
        RetirePlayer(player);
    }

    private Player FindSecondChanceTarget()
    {
        int index = _secondChanceTargetIndex;
        for (int i = 0; i < _playerCount; i++)
        {
            Player candidate = _players[index % _players.Count];
            if (!candidate.HasSecondChance())
                return candidate;
            index++;
        }
        return null;
    }

    private void CheckBurst(Player player)
    {
        Card lastCard = player.Hand[player.Hand.Count - 1];

        if (lastCard.Type != CardType.Number) return;

        if (player.Hand.Find(c => c != lastCard && c.Number == lastCard.Number) != null)
        {
            if (player.HasSecondChance())
            {
                player.UseSecondChance(lastCard);
                return;
            }
            else
            {
                player.Burst();
                return;
            }
        }
    }

    private void CheckFlip7(Player player)
    {
        if (!player.IsActive)
            return;

        if (player.Hand.FindAll(c => c.Type == CardType.Number).Count < 7)
            return;

        // FLIP7!
        player.Flip7();
        RetirePlayer(player);
        _scores[player] += FLIP7BONUS;

        foreach (Player other in _players.FindAll(p => p.IsActive))
        {
            other.Fold();
            RetirePlayer(other);
        }
        return;
    }
}
