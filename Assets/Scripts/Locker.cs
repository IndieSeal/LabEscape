using System;
using UnityEngine;

public class Locker : MonoBehaviour, IInteractable
{
    public static event Action<DialogueManager.DialogueTarget> OnHidingStart;
    public static event Action OnHidingEnded;
    
    [SerializeField] private DialogueManager.DialogueTarget cameraTarget;
    private bool insideLocker = false;
    
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
        insideLocker = !insideLocker;

        if (insideLocker)
        {
            OnHidingStart?.Invoke(cameraTarget);
            Cursor.lockState = CursorLockMode.None;
        }
        else StopInteraction();
    }

    private void StopInteraction()
    {
        OnHidingEnded?.Invoke();
        Cursor.lockState = CursorLockMode.Locked;

        PlayerUI.Instance.HidePrompt();
    }
}