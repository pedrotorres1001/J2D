using UnityEngine;

[CreateAssetMenu (fileName = "New Dialog", menuName = "Dialog/DialogData")]
public class DialogData : ScriptableObject
{
    public DialogLine[] dialogLines;
}
