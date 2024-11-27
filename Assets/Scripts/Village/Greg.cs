using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Greg : MonoBehaviour
{
    public GameObject DialogPanel;
    public TMP_Text DialogText;
    public string[] DialogLines;
    public int currentLine;
    public float wordSpeed;
    public bool playerIsClose;
    public GameObject continueButton;

    // Update is called once per frame
    void Update()
    {
        if (playerIsClose && Input.GetKeyDown(KeyCode.F))
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); // default cursor
            if (DialogPanel.activeInHierarchy)
            {
                zeroText();
            }
            else
            {
                DialogPanel.SetActive(true);
                StartCoroutine(ShowDialog());
            }
        }

        if (DialogText.text == DialogLines[currentLine])
        {
            continueButton.SetActive(true);
        }
    }

    public IEnumerator ShowDialog()
    {
        foreach (char letter in DialogLines[currentLine].ToCharArray())
        {
            DialogText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
        currentLine++;
    }

    public void nextLine()
    {

        continueButton.SetActive(false);

        if (currentLine < DialogLines.Length - 1)
        {
            currentLine++;
            DialogText.text = "";
            StartCoroutine(ShowDialog());
        }
        else
        {
            zeroText();
        }
    }

    public void zeroText()
    {
        DialogText.text = "";
        currentLine = 0;
        DialogPanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            zeroText();
            StopAllCoroutines();
        }
    }
}
