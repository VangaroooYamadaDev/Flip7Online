using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _cardUIPrefab;

    [SerializeField]
    private Transform _player1CardContainer;
    [SerializeField]
    private Transform _player2CardContainer;
    [SerializeField]
    private Transform _player3CardContainer;

    [SerializeField]
    private TextMeshProUGUI _player1StatusText;
    [SerializeField]
    private TextMeshProUGUI _player2StatusText;
    [SerializeField]
    private TextMeshProUGUI _player3StatusText;

    [SerializeField]
    private TextMeshProUGUI _player1ScoreText;
    [SerializeField]
    private TextMeshProUGUI _player2ScoreText;
    [SerializeField]
    private TextMeshProUGUI _player3ScoreText;

    [SerializeField]
    private GameObject _player1TurnIndicator;
    [SerializeField]
    private GameObject _player2TurnIndicator;
    [SerializeField]
    private GameObject _player3TurnIndicator;


    public void UpdatePlayerStatus(Player player, TextMeshProUGUI statusText)
    {
        statusText.text = $"{player.Name}: {player.Status}";
    }

    public void UpdatePlayerScore(Player player, int score, TextMeshProUGUI scoreText)
    {
        scoreText.text = $"{score}pt";
    }

    public void UpdatePlayerHand(Player player, Transform cardContainer)
    {
        // Clear player's hand area
        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }

        // Recreate hand area
        foreach (Card card in player.Hand)
        {
            GameObject cardObj = Instantiate(_cardUIPrefab, cardContainer);
            cardObj.GetComponent<CardUIView>().Setup(card);
        }
    }

    public void UpdateTurnIndicator(int currentPlayerIndex)
    {
        _player1TurnIndicator.SetActive(currentPlayerIndex==0);
        _player2TurnIndicator.SetActive(currentPlayerIndex==1);
        _player3TurnIndicator.SetActive(currentPlayerIndex==2);
    }

    public void UpdateAllStatuses(List<Player> players)
    {
        UpdatePlayerStatus(players[0], _player1StatusText);
        UpdatePlayerStatus(players[1], _player2StatusText);
        UpdatePlayerStatus(players[2], _player3StatusText);
    }

    public void UpdateAllScores(List<Player> players, Dictionary<Player, int> scores)
    {
        UpdatePlayerScore(players[0], scores[players[0]], _player1ScoreText);
        UpdatePlayerScore(players[1], scores[players[1]], _player2ScoreText);
        UpdatePlayerScore(players[2], scores[players[2]], _player3ScoreText);
    }

    public void UpdateAllHands(List<Player> players)
    {
        UpdatePlayerHand(players[0], _player1CardContainer);
        UpdatePlayerHand(players[1], _player2CardContainer);
        UpdatePlayerHand(players[2], _player3CardContainer);
    }
}
