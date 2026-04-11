using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _cardUIPrefab;

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
}
