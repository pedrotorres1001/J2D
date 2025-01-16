using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorTransitionCrystal : MonoBehaviour
{
    public Image uiImage; // Componente Image da UI.
    public Color[] colors; // Cores a transitar.
    public float duration = 5f; // Tempo para cada transição.

    private int currentIndex = 0; // Índice da cor atual.
    private int direction = 1; // Direção da transição (1 para frente, -1 para trás).
    private float timer = 0f; // Temporizador para a interpolação.

    void Update()
    {
        if (colors.Length < 2 || uiImage == null) return;

        // Incrementa o timer.
        timer += Time.deltaTime;
        float t = timer / duration;

        // Interpola a cor entre as cores atuais.
        Color newColor = Color.Lerp(colors[currentIndex], colors[(currentIndex + direction + colors.Length) % colors.Length], t);
        newColor.a = 1f; // Garante que o Alpha está 1 (totalmente opaco).

        uiImage.color = newColor;

        // Quando a transição termina, passa para a próxima cor na direção atual.
        if (t >= 1f)
        {
            timer = 0f; // Reseta o timer.
            currentIndex = (currentIndex + direction + colors.Length) % colors.Length; // Avança para a próxima cor.

            // Quando atingir o final ou o começo, inverte a direção.
            if (currentIndex == 0 || currentIndex == colors.Length - 1)
            {
                direction *= -1; // Inverte a direção.
            }
        }
    }
}
