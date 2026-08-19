using UnityEngine;

public class DialogueTrigger : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueManager.DialogueTarget target;
    [SerializeField, TextArea] private string myText = "This is an example dialogue";

    public void OnEnter()
    {
        DialogueManager.Instance.ShowPrompt();
    }

    public void OnExit()
    {
        DialogueManager.Instance.HidePrompt();
    }

    public void OnInteract()
    {
        DialogueManager.Instance.TriggerDialogue(myText, target);
    }
}