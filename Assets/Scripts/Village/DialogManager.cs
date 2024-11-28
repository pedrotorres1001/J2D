using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public Image characterImage;
    public DialogLine[] dialogLines;
    private int currentLineIndex = 0;
    private bool isDialogActive = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
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
            player.GetComponent<Animator>().enabled = false;
            Debug.Log("Dialog started between player and enemy.");
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
            characterImage.sprite = line.characterImage;
        }
    }

    void EndDialog()
    {
        isDialogActive = false;
        dialogUI.SetActive(false);
        player.GetComponent<PlayerMovement>().enabled = true; // Enable player movement
        player.GetComponent<Animator>().enabled = true;
        // when the dialog finishes, its supposed to activate the pickaxe
        GetChildObject(player, "Pickaxe").SetActive(true);
    }

    void CancelDialog()
    {
        if (isDialogActive)
        {
            isDialogActive = false;
            dialogUI.SetActive(false);
            player.GetComponent<PlayerMovement>().enabled = true; // Enable player movement 
            player.GetComponent<Animator>().enabled = true;
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