using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _cardUIPrefab;

    [SerializeField]
    private Transform _player1HandArea;
    [SerializeField]
    private Transform _player2HandArea;
    [SerializeField]
    private Transform _player3HandArea;

    [SerializeField]
    private TextMeshProUGUI _player1StatusText;
    [SerializeField]
    private TextMeshProUGUI _player2StatusText;
    [SerializeField]
    private TextMeshProUGUI _player3StatusText;

    public void UpdatePlayerStatus(Player player, TextMeshProUGUI statusText)
    {
        statusText.text = $"{player.Name}: {player.Status}";
    }

    public void UpdatePlayerHand(Player player, Transform handArea)
    {
        // Clear player's hand area
        foreach (Transform child in handArea)
        {
            Destroy(child.gameObject);
        }

        // Recreate hand area
        foreach (Card card in player.Hand)
        {
            GameObject cardObj = Instantiate(_cardUIPrefab, handArea);
            cardObj.GetComponent<CardUIView>().Setup(card);
        }
    }

    public void UpdateAllStatuses(List<Player> players)
    {
        UpdatePlayerStatus(players[0], _player1StatusText);
        UpdatePlayerStatus(players[1], _player2StatusText);
        UpdatePlayerStatus(players[2], _player3StatusText);
    }

    public void UpdateAllHands(List<Player> players)
    {
        UpdatePlayerHand(players[0], _player1HandArea);
        UpdatePlayerHand(players[1], _player2HandArea);
        UpdatePlayerHand(players[2], _player3HandArea);
    }
}
