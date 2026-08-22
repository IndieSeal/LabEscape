using UnityEngine;

public class KeycardDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator animator;
    [SerializeField] private DialogueManager.DialogueTarget target;
    private bool inputting = false;
    
    public void OnEnter()
    {
        animator.ResetTrigger("Close");
        animator.SetTrigger("Open");

        PlayerUI.Instance.ShowPrompt();
    }

    public void OnExit()
    {
        animator.ResetTrigger("Open");
        animator.SetTrigger("Close");

        PlayerUI.Instance.HidePrompt();
    }

    public void OnInteract()
    {
        inputting = !inputting;
        
        //This is temporary, will change it for a handler that just inmovilizes the player
        if(inputting) DialogueManager.OnDialogueStarted?.Invoke(target);
        else DialogueManager.OnDialogueEnded?.Invoke();
    }
}