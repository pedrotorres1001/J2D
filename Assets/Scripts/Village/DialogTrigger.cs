using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public DialogLine[] dialogLines; // Assign dialog lines in the Inspector
    private DialogManager dialogManager;

    void Start()
    {
        dialogManager = FindObjectOfType<DialogManager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            dialogManager.StartDialog(dialogLines);
        }
    }
}
