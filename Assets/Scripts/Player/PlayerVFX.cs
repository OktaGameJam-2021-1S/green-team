using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem stepParticle;
    public void PlayStepVFX()
    {
        if (stepParticle)
        {
            stepParticle.Play();
        }
    }
}
