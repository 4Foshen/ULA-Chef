using UnityEngine;
using UnityEngine.EventSystems;
using TMPro; 

public class ButtonTextColorChange : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public TextMeshProUGUI buttonText;
    public Color normalColor = Color.white;
    public Color pressedColor = Color.gray;

    private void Start()
    {
        if (buttonText == null)
            buttonText = GetComponentInChildren<TextMeshProUGUI>();

        buttonText.color = normalColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonText.color = pressedColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonText.color = normalColor;
    }
}
