using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<Player> _players = new List<Player>();
    private Deck _deck;
    private int _currentPlayerIndex;
    private Dictionary<Player, int> _scores = new Dictionary<Player, int>();
    private const int Flip7Bonus = 15;
    private const int ClearScore = 200;

    [SerializeField]
    private int _playerCount = 3;
    [SerializeField]
    private int _flipThreeTargetIndex = 0;
    [SerializeField]
    private int _freezeTargetIndex = 0;
    [SerializeField]
    private int _secondChanceTargetIndex = 0;

    [SerializeField]
    private UIManager _uiManager;

    private void Start()
    {
        InitializeGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log($"CurrentPlayer: {_players[_currentPlayerIndex].Name}");
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            foreach (Player player in _players)
            {
                Debug.Log($"--{player.Name}:{player.Status}---[{_scores[player]}]-----");
                Debug.Log($"DECK: {string.Join(" ", player.Hand.Select(c => c.Name))}");
            }
        }
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
        for (int i = 0; i < _players.Count; i++)
        {
            DrawCard();
            _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;
        }

        _uiManager.UpdateAllStatuses(_players);
        _uiManager.UpdateAllHands(_players);
    }

    public void DrawCard()
    {
        DrawCardForPlayer(_players[_currentPlayerIndex]);
    }

    private void DrawCardForPlayer(Player player)
    {
        Card drawnCard = _deck.Draw();
        Debug.Log($"DrawCardForPlayer: {player.Name} drew {drawnCard.Name}");
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
            ProcessSpecialCard(drawnCard, player);
        }
        else
        {
            CheckBurst(player);
            CheckFlip7(player);
        }
    }

    private void ProcessSpecialCard(Card card, Player player)
    {
        switch (card.Definition.Special)
        {
            case SpecialType.FlipThree:
                List<Card> pendingSpecials = new List<Card>();
                Player flipTarget = _players[_flipThreeTargetIndex];

                for (int i = 0; i < 3; i++)
                {
                    if (!flipTarget.IsActive) break;

                    Card flipCard = _deck.Draw();
                    flipTarget.AddCard(flipCard);

                    if (flipCard.Type == CardType.Special)
                    {
                        pendingSpecials.Add(flipCard);
                    }
                    else
                    {
                        CheckBurst(flipTarget);
                        CheckFlip7(flipTarget);
                    }
                }

                if (flipTarget.IsActive)
                {
                    foreach (Card specialCard in pendingSpecials)
                    {
                        ProcessSpecialCard(specialCard, flipTarget);
                    }
                }
                break;
            case SpecialType.Freeze:
                if (_players[_freezeTargetIndex].IsActive)
                    Freeze(_players[_freezeTargetIndex]);
                break;
        }
    }

    private void RetirePlayer(Player player)
    {
        _scores[player] += player.HandScore();
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

    [ContextMenu("DRAW CARD")]
    public void DebugDraw()
    {
        Debug.Log("DEBUG DRAW CALLED");
        DrawCard();
        _uiManager.UpdateAllStatuses(_players);
        _uiManager.UpdateAllHands(_players);
        Debug.Log("TURN FLOW CALLED");
        TurnFlow();
    }

    [ContextMenu("FOLD")]
    public void DebugFold()
    {
        Fold();
        TurnFlow();
    }

    [ContextMenu("DEBUG FLIP THREE")]
    public void DebugFlipThree()
    {
        ProcessSpecialCard(new Card(new CardDefinition("FLIP THREE", SpecialType.FlipThree)), _players[_currentPlayerIndex]);
        _uiManager.UpdateAllStatuses(_players);
        _uiManager.UpdateAllHands(_players);
        TurnFlow();
    }

    private void TurnFlow()
    {
        Debug.Log($"CHECK ROUND END: {CheckRoundEnd()}");
        if (CheckRoundEnd())
            EndRound();
        else
            NextTurn();
    }

    private void CheckBurst(Player player)
    {
        Card lastCard = player.Hand[player.Hand.Count - 1];

        if (lastCard.Type != CardType.Number) return;

        if (player.Hand.Find(c => c != lastCard && c.Number == lastCard.Number) != null)
        {
            if (player.HasSecondChance())
            {
                Debug.Log($"****{player.Name} use SecondChance****");
                player.UseSecondChance(lastCard);
                return;
            }
            else
            {
                Debug.Log($"****{player.Name} BURST****");
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
        Debug.Log($"****{player.Name} FLIP 7!****");
        player.Flip7();
        RetirePlayer(player);
        _scores[player] += Flip7Bonus;

        foreach (Player other in _players.FindAll(p => p.IsActive))
        {
            other.Fold();
            RetirePlayer(other);
        }
        return;
    }

    private bool CheckRoundEnd()
    {
        return _players.All(p => !p.IsActive);
    }

    private bool CheckGameEnd()
    {
        return _scores.Any(p => p.Value >= ClearScore);
    }

    private void NextTurn()
    {
        do
        {
            _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;
        } while (!_players[_currentPlayerIndex].IsActive);
    }

    private void EndRound()
    {
        if (CheckGameEnd())
        {
            // End Game Process
            Debug.Log("Game Over!");
        }
        else
        {
            // Continue Round Process
            foreach (Player player in _players)
            {
                _deck.DiscardRange(player.DiscardHand());
                player.ResetStatus();
            }

            for (int i = 0; i < _players.Count; i++)
            {
                _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;
                DrawCard();
            }

            _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;

            _uiManager.UpdateAllStatuses(_players);
            _uiManager.UpdateAllHands(_players);
        }
    }
}
