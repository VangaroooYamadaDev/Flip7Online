using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<Player> _players = new List<Player>();
    private Deck _deck;
    private int _currentPlayerIndex;
    private Dictionary<Player, int> _scores = new Dictionary<Player, int>();

    [SerializeField]
    private int _playerCount = 3;

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        _deck = new Deck();

        for (int i = 0; i < _playerCount; i++)
        {
            var Player = new Player($"Player{i + 1}");
            _players.Add(Player);
            _scores[Player] = 0;
        }

        _currentPlayerIndex = 0;
    }

    void Update()
    {

    }
}
