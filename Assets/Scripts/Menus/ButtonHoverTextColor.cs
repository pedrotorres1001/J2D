using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverTMPColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text buttonText; // Reference to the TMP_Text component
    public Color normalColor = Color.white; // Default text color
    public Color hoverColor = Color.red; // Hover text color

    private void Reset()
    {
        // Automatically find the TMP_Text component on the button
        buttonText = GetComponentInChildren<TMP_Text>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonText != null)
            buttonText.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonText != null)
            buttonText.color = normalColor;
    }
}
