using System;
using UnityEngine;

public class FlashlightHandler : MonoBehaviour
{
    protected PlayerInputHandler Input => PlayerInputHandler.Instance;

    [SerializeField] private Animator handAnimator;    
    
    [SerializeField] private GameObject flashlightLight;
    [SerializeField] private AudioSource toggleFlashlightSource;
    private bool isOn = true;
    private bool wasOn;
    private bool forced = false;

    void Awake()
    {
        // There's definitely better ways to code this, but nothing comes to mind atm
        isOn = !isOn;
        ToggleFlashlight();
    }

    void OnEnable()
    {
        CameraHandler.OnStopPlayer += ForceDisableFlashlight;
        CameraHandler.OnResumePlayer += ForceEnable;

        Keycard.OnCollectiblePicked += GrabCollectible;
    }

    void OnDisable()
    {
        CameraHandler.OnStopPlayer -= ForceDisableFlashlight;
        CameraHandler.OnResumePlayer -= ForceEnable;

        Keycard.OnCollectiblePicked -= GrabCollectible;
    }

    void Update()
    {
        if(Input.WasFlashlightPressed && !forced) ToggleFlashlight(true);
    }

    private void GrabCollectible(ECollectible collectible)
    {
        handAnimator.SetTrigger("Grab");
    }

    private void ToggleFlashlight(bool manual = false)
    {
        if(isOn) DisableFlashlight();
        else EnableFlashlight();

        if(manual) toggleFlashlightSource.Play();
    }

    private void ForceDisableFlashlight()
    {
        forced = true;
        wasOn = isOn;
        DisableFlashlight();   
    }

    private void DisableFlashlight()
    {
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