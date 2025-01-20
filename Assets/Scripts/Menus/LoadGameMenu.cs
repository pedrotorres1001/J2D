using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadGameMenu : MonoBehaviour
{
    public GameObject LoadingScreen;
    public Image LoadingBarFill;
    private string filePath;

    private void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, PlayerPrefs.GetString("Filename"));
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
}
