using System.Collections.Generic;
using UnityEngine;

public class CustomFootstep : MonoBehaviour
{
    [SerializeField] private List<AudioClip> customFootsteps = new List<AudioClip>();
    public AudioClip AudioClip => customFootsteps.GetRandom();
}