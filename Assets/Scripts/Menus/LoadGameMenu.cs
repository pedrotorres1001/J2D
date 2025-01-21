using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadGameMenu : MonoBehaviour
{
    public GameObject LoadingScreen;
    public Image LoadingBarFill;
    private string filePath;

    [SerializeField] private TextMeshProUGUI lastSaveText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI playTimeText;


    private void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "saveData.json");
    }

    private void OnEnable()
    {
        LoadSaveSummary();
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false; // Impede a ativação imediata da cena

        while (operation.progress < 0.9f)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            LoadingBarFill.fillAmount = progressValue;
            yield return null;
        }

        // Simular a transição para o 100%
        float fakeProgress = 0.9f;
        while (fakeProgress < 1f)
        {
            fakeProgress += Time.deltaTime * 0.1f; // Ajuste a velocidade aqui
            LoadingBarFill.fillAmount = fakeProgress;
            yield return null;
        }

        // Ativar a cena quando estiver pronto
        operation.allowSceneActivation = true;
    }

    public void DeleteSave()
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("Save file deleted: " + filePath);
        }
        else
        {
            Debug.Log("No save file found to delete.");
        }
    }

    public void LoadSaveSummary()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);

            // Carregar apenas o campo 'lastSaveTime'
            SaveSummary summary = JsonUtility.FromJson<SaveSummary>(json);

            if (summary != null && !string.IsNullOrEmpty(summary.lastSaveTime))
            {
                Debug.Log("Last save time: " + summary.lastSaveTime);

                if (lastSaveText != null)
                    lastSaveText.text = summary.lastSaveTime;

                if(levelText != null)
                    levelText.text = summary.level.ToString();

                if (playTimeText != null)
                    playTimeText.text = FormatPlayTime(summary.totalPlayTime);

            }
            else
            {
                lastSaveText.text = "No data available.";
                levelText.text = "No data available.";
                playTimeText.text = "No data available.";
            }
        }
        else
        {
            Debug.Log("No save file found!");
            if (lastSaveText != null)
            {
                lastSaveText.text = "No available.";
                levelText.text = "No available.";
                playTimeText.text = "No available.";
            }
        }
    }

    [System.Serializable]
    public class SaveSummary
    {
        public string lastSaveTime;
        public int level;
        public float totalPlayTime;

    }

    private string FormatPlayTime(float playTime)
    {
        int hours = Mathf.FloorToInt(playTime / 3600);
        int minutes = Mathf.FloorToInt((playTime % 3600) / 60);
        int seconds = Mathf.FloorToInt(playTime % 60);

        return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
    }
}
