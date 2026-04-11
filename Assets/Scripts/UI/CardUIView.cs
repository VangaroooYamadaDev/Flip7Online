using UnityEngine;
using TMPro;

public class CardUIView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _cardNameText;

    public void Setup(Card card)
    {
        _cardNameText.text = card.Name;
    }
}
