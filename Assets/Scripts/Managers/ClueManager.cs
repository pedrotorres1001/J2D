using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class ClueManager : MonoBehaviour
{
    public GameObject player;
    public GameObject artifact1st;
    public GameObject artifact2nd;
    public GameObject crazyJoe1st;
    public GameObject crazyJoe2nd;
    public float dialogRange = 2.0f;
    public GameObject dialogUI;
    public TextMeshProUGUI dialogText;
    private bool isDialogActive = false;

    private Dictionary<GameObject, string> dialogTexts;

    void Start()
    {
        dialogTexts = new Dictionary<GameObject, string>
        {
            { artifact1st, "There's somethin' strange about this volcano. The way it shakes, it's almost as if it's breathin'. \n\n We've all heard the old legend about the mountain havin' a heart. Who'd believe such nonsense?\n \n Still, the tale's been told for over a hundred years. A madman by the name o' \"Crazy Joe\" claimed he stumbled on some sort o' artifact deep in the mountain—a thing shaped like a heart. Said it could command the fire itself, directin' the lava and the fury o' the earth to his will. That's how he earned his name, after all." },
            { artifact2nd, "Joe was always mutterin', \"The one who wields the heart becomes king.\" Bold talk from a loon. But he also swore it was buried in the deepest veins o' the volcano. 'Course, no dwarf has ever dug that far—or dared to try." },
            { crazyJoe1st, "It seems these stone lads've got crystals sproutin' from their bodies. Turns out, if ye strike those crystals with enough force, it'll put the brutes down quicker than an ale barrel at a feast!" },
            { crazyJoe2nd, "I've been wanderin' these blasted caves alone for days now, and I'll tell ye this, rock and stone might be fine for buildin' a hall, but they make for a miserable meal. Ach, how I long for a proper feast, a tankard o' good ale in one hand and a slab o' roasted meat in the other. Now that's livin'!" }
        };
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyManager.KM.interact))
        {
            GameObject closestObject = GetClosestObject();
            if (closestObject != null && !isDialogActive)
            {
                StartDialog(closestObject);
            }
            else
            {
                CancelDialog();
            }
        }
    
        if (Input.GetKeyDown(KeyManager.KM.jump))
        {
            if (isDialogActive)
            {
                ContinueDialog();
            }
        }
    }
    
    GameObject GetClosestObject()
    {
        GameObject[] objects = { artifact1st, artifact2nd, crazyJoe1st, crazyJoe2nd };
        GameObject closestObject = null;
        float closestDistance = dialogRange;
    
        foreach (GameObject obj in objects)
        {
            float distance = Vector3.Distance(player.transform.position, obj.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestObject = obj;
            }
        }
    
        return closestObject;
    }
    
    void StartDialog(GameObject closestObject)
    {
        if (!isDialogActive)
        {
            isDialogActive = true;
            dialogUI.SetActive(true);
            Time.timeScale = 0;
    
            if (dialogTexts.TryGetValue(closestObject, out string dialog))
            {
                dialogText.text = dialog;
            }
            else
            {
                dialogText.text = "No dialog available for this object.";
            }
        }
    }

    void ContinueDialog()
    {
        dialogUI.SetActive(false);
        isDialogActive = false;
        Time.timeScale = 1;
    }

    void CancelDialog()
    {
        dialogUI.SetActive(false);
        isDialogActive = false;
        Time.timeScale = 1;
    }
}