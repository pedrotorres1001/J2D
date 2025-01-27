using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class DialogLineFinal
{
    public string characterName;
    public Sprite characterImage;
    public string text;
}

public class FinalDialogue : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject dialogUI;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private Button endDialogButton;
    [SerializeField] private Image characterImage;
    [SerializeField] private Sprite dougImage;
    [SerializeField] private Sprite jeffImage;
    [SerializeField] private DialogLineFinal[] dialogLines;

    private int currentLineIndex = 0;
    private bool isDialogActive = false;
    private bool hasDialogOccurred = false; // Flag to track if dialog has occurred

    void Start()
    {
        if (endDialogButton != null)
        {
            endDialogButton.onClick.AddListener(EndDialog);
            Debug.Log("EndDialog button listener added.");
        }
        else
        {
            Debug.LogError("EndDialog button is not assigned.");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasDialogOccurred)
        {
            if (!isDialogActive)
            {
                StartDialog();
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isDialogActive)
            {
                ContinueDialog();
            }
        }
    }

    void StartDialog()
    {
        if (!isDialogActive)
        {
            isDialogActive = true;
            hasDialogOccurred = true; // Set the flag to true
            dialogUI.SetActive(true);
            currentLineIndex = 0;
            ShowDialogLine();
            player.GetComponent<PlayerMovement>().enabled = false;
            Debug.Log(player.GetComponent<PlayerMovement>().enabled);
            Debug.Log("Dialog started.");
        }
    }

    void ContinueDialog()
    {
        if (isDialogActive)
        {
            currentLineIndex++;
            if (currentLineIndex < dialogLines.Length)
            {
                ShowDialogLine();
            }
            else
            {
                EndDialog();
            }
        }
    }

    void ShowDialogLine()
    {
        if (currentLineIndex < dialogLines.Length)
        {
            DialogLineFinal line = dialogLines[currentLineIndex];
            dialogText.text = line.text;
            characterNameText.text = line.characterName;

            if (line.characterName == "Doug")
            {
                characterImage.sprite = dougImage;
            }
            else if (line.characterName == "Jeff")
            {
                characterImage.sprite = jeffImage;
            }
            Debug.Log("Showing dialog line: " + line.text);
        }
    }

    void EndDialog()
    {
        isDialogActive = false;
        dialogUI.SetActive(false);
        player.GetComponent<PlayerMovement>().enabled = true;
        GetChildObject(player, "Pickaxe").SetActive(true);
        Debug.Log("Dialog ended.");
    }

    void CancelDialog()
    {
        if (isDialogActive)
        {
            isDialogActive = false;
            dialogUI.SetActive(false);
            player.GetComponent<PlayerMovement>().enabled = true;
            Debug.Log("Dialog canceled.");
        }
    }

    GameObject GetChildObject(GameObject parent, string childName)
    {
        Transform[] transforms = parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in transforms)
        {
            if (t.gameObject.name == childName)
            {
                return t.gameObject;
            }
        }
        return null;
    }
}