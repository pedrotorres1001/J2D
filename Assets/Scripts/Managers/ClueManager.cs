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
    public GameObject artifact3rd;
    public GameObject artifact4th;
    public GameObject crazyJoe1st;
    public GameObject crazyJoe2nd;
    public GameObject crazyJoe3rd;
    public GameObject crazyJoe4th;
    public float dialogRange = 2.0f;
    public GameObject dialogUI;
    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI dialogCloseText;
    private bool isDialogActive = false;

    private Dictionary<GameObject, string> dialogTexts;

    void Start()
    {
        dialogTexts = new Dictionary<GameObject, string>
        {
            { artifact1st, "There's somethin' strange about this volcano. The way it shakes, it's almost as if it's breathin'. \n\n We've all heard the old legend about the mountain havin' a heart. Who'd believe such nonsense?\n \n Still, the tale's been told for over a hundred years. A madman by the name o' \"Crazy Joe\" claimed he stumbled on some sort o' artifact deep in the mountain, a thing shaped like a heart. Said it could command the fire itself, directin' the lava and the fury o' the earth to his will. That's how he earned his name, after all." },
            { artifact2nd, "Joe was always mutterin', \"The one who wields the heart becomes king.\" Bold talk from a loon. But he also swore it was buried in the deepest veins o' the volcano. 'Course, no dwarf has ever dug that far, or dared to try." },
            { artifact3rd, "Now that we've started diggin' and settin' up the new exploration outpost, we've come across some curious beasties. Strange things they are, made of rock and shiny crystals. They weren't much trouble, gave us a bit of a scrap, but nothin' a sturdy axe couldn't handle. We dealt with 'em well enough. Still no sign o' the artifact in this layer, though. It's clear as a mountain stream, we need to dig deeper. The heart o' the mountain ain't gonna give up its secrets so easily!" },
            { artifact4th, "We've yet to lay hands on the artifact, but we've come up with a curious theory: seems the crystals on those beasties shift color dependin' on their strength. The hotter the color, the fiercer the fight they put up. Things are growin' rougher with every step down, but there's no turnin' back now. If the heart o' the mountain's truly down there, we'll find it. We're dwarves, after all, diggin' deeper is what we do best!" },
            { crazyJoe1st, "It seems these stone lads've got crystals sproutin' from their bodies. Turns out, if ye strike those crystals with enough force, it'll put the brutes down quicker than an ale barrel at a feast!" },
            { crazyJoe2nd, "I've been wanderin' these blasted caves alone for days now, and I'll tell ye this, rock and stone might be fine for buildin' a hall, but they make for a miserable meal. Ach, how I long for a proper feast, a tankard o' good ale in one hand and a slab o' roasted meat in the other. Now that's livin'!" },
            { crazyJoe3rd, "I've no clue how deep I've dug meself. Feels like I'm wanderin' in circles, losin' me wits. These caves, they play tricks on the mind, and the beasts I've run into, they're gettin' harder to put down. Perhaps it's wiser to slip past 'em when I can, keep me strength for what's ahead." },
            { crazyJoe4th, "I'll admit, I'm startin' to question the wisdom o' comin' down here. The heat's risin', hotter than a forge at full roar, and I've neither good food nor a drop o' pure water left. Bah, what madness drove me to take on this fool's venture?" }
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
    }
    
    GameObject GetClosestObject()
    {
        GameObject[] objects = { artifact1st, artifact2nd, artifact3rd, artifact4th, crazyJoe1st, crazyJoe2nd, crazyJoe3rd, crazyJoe4th };
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
            dialogCloseText.text = "Press " + KeyManager.KM.interact.ToString() + " to close";
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
    void CancelDialog()
    {
        dialogUI.SetActive(false);
        isDialogActive = false;
        Time.timeScale = 1;
    }
}