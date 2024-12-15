using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Collections.Generic;

public class AnalyticsManager : MonoBehaviour
{
    private float sessionStartTime;

    public static AnalyticsManager Instance;

    private async void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        // Inicializar os serviços do Unity Gaming
        try
        {
            await UnityServices.InitializeAsync();
            AnalyticsService.Instance.StartDataCollection();
            Debug.Log("Unity Gaming Services iniciado com sucesso.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Erro ao inicializar Unity Gaming Services: " + ex.Message);
        }
    }

    private void Start()
    {
        sessionStartTime = Time.time;
    }

    private void OnApplicationQuit()
    {
        // Calcular o tempo de jogo da sessão atual
        float sessionTime = Time.time - sessionStartTime;

        // Enviar evento para Unity Analytics
        AnalyticsService.Instance.CustomData("session_time", new Dictionary<string, object>
        {
            { "time_played", sessionTime }
        });

        // Garantir que os dados sejam enviados
        AnalyticsService.Instance.Flush();

        Debug.Log("Tempo jogado enviado para Analytics: " + sessionTime + " segundos.");
    }
}