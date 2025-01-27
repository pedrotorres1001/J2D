using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

[System.Serializable]
public class DialogLine
{
    public string characterName;
    public Sprite characterImage;
    public string text;
}

public class DialogManager : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;
    public float dialogRange = 2.0f;
    public GameObject dialogUI;
    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI characterNameText;
    public Button endDialogButton;
    public Image characterImage;
    public Sprite dougImage;
    public Sprite gregImage;
    public GameObject speechBubble;
    public DialogLine[] dialogLines;
    private int currentLineIndex = 0;
    private bool isDialogActive = false;
    private bool kingDialog = false;
    public GameObject door;

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

    void Update()
    {
        if (Input.GetKeyDown(KeyManager.KM.interact))
        {
            float distance = Vector3.Distance(player.transform.position, enemy.transform.position);
            if (distance <= dialogRange)
            {
                if (!isDialogActive)
                {
                    StartDialog();
                }
            }
            else
            {
                CancelDialog();
            }
        }

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
            dialogUI.SetActive(true);
            currentLineIndex = 0;
            ShowDialogLine();
            player.GetComponent<PlayerMovement>().enabled = false;
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
            DialogLine line = dialogLines[currentLineIndex];
            dialogText.text = line.text;
            characterNameText.text = line.characterName;

            if (line.characterName == "Doug")
            {
                characterImage.sprite = dougImage;
            }
            else if (line.characterName == "Greg")
            {
                characterImage.sprite = gregImage;
            }
            else if (line.characterName == "King")
            {
                characterImage.sprite = gregImage;
                kingDialog = true;
            }
            Debug.Log("Showing dialog line: " + line.text);
        }
    }

    void EndDialog()
    {
        isDialogActive = false;
        dialogUI.SetActive(false);
        player.GetComponent<PlayerMovement>().enabled = true; // Enable player movement
        speechBubble.SetActive(false);
        GetChildObject(player, "Pickaxe").SetActive(true);
        Debug.Log("Dialog ended.");

        if(kingDialog)
            door.SetActive(false);
    }

    void CancelDialog()
    {
        if (isDialogActive)
        {
            isDialogActive = false;
            dialogUI.SetActive(false);
            player.GetComponent<PlayerMovement>().enabled = true; // Enable player movement 
            speechBubble.SetActive(false);
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