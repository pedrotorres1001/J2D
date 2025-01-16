using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorTransitionCrystal : MonoBehaviour
{
    public Image uiImage; // Componente Image da UI.
    public Color[] colors; // Cores a transitar.
    public float duration = 5f; // Tempo para cada transi��o.

    private int currentIndex = 0; // �ndice da cor atual.
    private int direction = 1; // Dire��o da transi��o (1 para frente, -1 para tr�s).
    private float timer = 0f; // Temporizador para a interpola��o.

    void Update()
    {
        if (colors.Length < 2 || uiImage == null) return;

        // Incrementa o timer.
        timer += Time.deltaTime;
        float t = timer / duration;

        // Interpola a cor entre as cores atuais.
        Color newColor = Color.Lerp(colors[currentIndex], colors[(currentIndex + direction + colors.Length) % colors.Length], t);
        newColor.a = 1f; // Garante que o Alpha est� 1 (totalmente opaco).

        uiImage.color = newColor;

        // Quando a transi��o termina, passa para a pr�xima cor na dire��o atual.
        if (t >= 1f)
        {
            timer = 0f; // Reseta o timer.
            currentIndex = (currentIndex + direction + colors.Length) % colors.Length; // Avan�a para a pr�xima cor.

            // Quando atingir o final ou o come�o, inverte a dire��o.
            if (currentIndex == 0 || currentIndex == colors.Length - 1)
            {
                direction *= -1; // Inverte a dire��o.
            }
        }
    }
}
