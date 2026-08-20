using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "IndieSeal/Scriptables/DialogueSO")]
public class DialogueSO : ScriptableObject
{
    [TextArea] public List<string> dialogues = new List<string>();
}