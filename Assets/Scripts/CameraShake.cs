using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera; // Referência para a câmera virtual
    private float shakeTimer; // Duração do shake
    private CinemachineBasicMultiChannelPerlin noise; // Componente de ruído da câmera

    private void Start()
    {
        // Inicializa o componente de ruído da câmera
        noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (noise != null)
        {
            noise.m_AmplitudeGain = 0f; // Garante que o shake esteja desativado no início
        }
    }

    public void Shake(float intensity, float time)
    {
        if (noise == null) return;

        noise.m_AmplitudeGain = intensity; // Ajusta a intensidade do shake
        shakeTimer = time; // Define a duração
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime; // Reduz o timer com o tempo

            if (shakeTimer <= 0)
            {
                // Quando o timer acaba, reseta o shake
                if (noise != null)
                {
                    noise.m_AmplitudeGain = 0f;
                }
            }
        }
    }
}