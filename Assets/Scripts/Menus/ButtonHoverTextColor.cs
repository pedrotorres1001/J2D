using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverTMPColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text buttonText; // Reference to the TMP_Text component
    public float normalScale = 1f; // Escala padrão do texto
    public float hoverScale = 1.2f; // Escala do texto ao passar o mouse

    private void Reset()
    {
        // Automatically find the TMP_Text component on the button
        buttonText = GetComponentInChildren<TMP_Text>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonText != null)
            buttonText.transform.localScale = Vector3.one * hoverScale; // Aumenta a escala
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonText != null)
            buttonText.transform.localScale = Vector3.one * normalScale;
    }
}
