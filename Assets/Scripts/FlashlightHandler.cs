using System;
using UnityEngine;

public class FlashlightHandler : MonoBehaviour
{
    protected PlayerInputHandler Input => PlayerInputHandler.Instance;
    
    [SerializeField] private GameObject flashlightLight;
    [SerializeField] private AudioSource toggleFlashlightSource;
    private bool isOn = true;
    private bool wasOn;
    private bool forced = false;

    void Awake()
    {
        ToggleFlashlight();
    }

    void OnEnable()
    {
        DialogueManager.OnDialogueStarted += DisableFlashlight;
        DialogueManager.OnDialogueEnded += ForceEnable;
    }

    void OnDisable()
    {
        DialogueManager.OnDialogueStarted -= DisableFlashlight;
        DialogueManager.OnDialogueEnded -= ForceEnable;
    }

    void Update()
    {
        if(Input.WasFlashlightPressed && !forced) ToggleFlashlight();
    }

    private void ToggleFlashlight()
    {
        if(isOn) DisableFlashlight();
        else EnableFlashlight();

        toggleFlashlightSource.Play();
    }

    private void DisableFlashlight(DialogueManager.DialogueTarget target = null)
    {
        if(target != null)
        {
            forced = true;
            wasOn = isOn;
        }
        
        flashlightLight.SetActive(false);
        isOn = false;
    }

    private void ForceEnable()
    {
        forced = false;
        isOn = !wasOn;

        ToggleFlashlight();
    }

    private void EnableFlashlight()
    {
        flashlightLight.SetActive(true);
        isOn = true;
    }
}