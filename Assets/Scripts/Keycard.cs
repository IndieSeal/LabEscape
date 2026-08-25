using System;
using UnityEngine;

public class Keycard : MonoBehaviour, IInteractable
{
    public static event Action<ECollectible> OnCollectiblePicked;
    
    [SerializeField] private ECollectible collectible;
    
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
        gameObject.SetActive(false);

        OnCollectiblePicked?.Invoke(collectible);
    }
}