using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [SerializeField] private PlayerMovHandler playerMovHandler;
    
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();

    [SerializeField] private float footstepDelay = 0.75f;
    private bool enabledFootsteps = true;
    private float timer;

    void OnEnable()
    {
        DialogueManager.OnDialogueStarted += DisableFootsteps;
        DialogueManager.OnDialogueEnded += EnableFootsteps;
    }

    void Update()
    {
        if(!enabledFootsteps) return;

        if(PlayerInputHandler.Instance.Movement != Vector2.zero) timer += Time.deltaTime;

        if(timer >= footstepDelay)
        {
            timer = 0;
            PlaySound();
        }
    }

    private void PlaySound()
    {
        AudioClip clip = audioClips.GetRandom();
        if(playerMovHandler.IsGrounded(out Collider col) && col.TryGetComponent(out CustomFootstep custom)) clip = custom.AudioClip;
        
        footstepSource.clip = clip;
        footstepSource.pitch = Random.Range(0.8f, 1.2f);
        footstepSource.Play();
    }

    private void DisableFootsteps(DialogueManager.DialogueTarget target) => enabledFootsteps = false;
    private void EnableFootsteps() => enabledFootsteps = true;
}