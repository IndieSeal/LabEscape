using UnityEngine;

public class Anim_Particles : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles;

    public void PlayParticles()
    {
        particles.Play();
    }

    public void EmitParticles(int amount)
    {
        particles.Emit(amount);
    }
}