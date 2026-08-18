using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();

    [SerializeField] private float rangeBtwnSteps = 5;
    private Vector3 lastPosition;
    private float recordedRange;

    void Awake()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, lastPosition);
        lastPosition = transform.position;

        if(distance == 0)
        {
            recordedRange = 0;
            return;
        }
        
        recordedRange += distance;
        if(recordedRange >= rangeBtwnSteps)
        {
            recordedRange = 0;
            PlaySound();
        }
    }

    private void PlaySound()
    {
        footstepSource.clip = audioClips.GetRandom();
        footstepSource.pitch = Random.Range(0.8f, 1.2f);
        footstepSource.Play();
    }
}