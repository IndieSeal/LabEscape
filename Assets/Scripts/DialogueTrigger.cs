using UnityEngine;

public class DialogueTrigger : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueManager.DialogueTarget target;
    [SerializeField] private DialogueSO myText;

    public void OnEnter()
    {
        PlayerUI.Instance.ShowPrompt();
    }

    public void OnExit()
    {
        PlayerUI.Instance.HidePrompt();
    }

    public void OnInteract()
    {
        DialogueManager.Instance.TriggerDialogue(myText, target);
    }
}