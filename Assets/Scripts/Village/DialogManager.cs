using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DialogManager : MonoBehaviour
{
    public TextMeshProUGUI speakerNameText; // Reference to speaker name UI
    public TextMeshProUGUI dialogContentText; // Reference to dialog text UI
    public Image portraitImage; // Optional: character portrait
    public GameObject dialogBox; // The dialog panel

    private Queue<DialogLine> dialogLines; // Stores the dialog lines
    private bool isDialogActive = false;

    void Start()
    {
        dialogLines = new Queue<DialogLine>();
        dialogBox.SetActive(false);
    }

    public void StartDialog(DialogLine[] lines)
    {
        dialogLines.Clear();
        foreach (var line in lines)
        {
            dialogLines.Enqueue(line);
        }

        dialogBox.SetActive(true);
        isDialogActive = true;
        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (dialogLines.Count == 0)
        {
            EndDialog();
            return;
        }

        var line = dialogLines.Dequeue();
        speakerNameText.text = line.speaker;
        dialogContentText.text = line.content;

        if (portraitImage != null && line.portrait != null)
        {
            portraitImage.sprite = line.portrait;
            portraitImage.gameObject.SetActive(true);
        }
        else if (portraitImage != null)
        {
            portraitImage.gameObject.SetActive(false);
        }
    }

    public void EndDialog()
    {
        dialogBox.SetActive(false);
        isDialogActive = false;
    }

    void Update()
    {
        if (isDialogActive && Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextLine();
        }
    }
}
