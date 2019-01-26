using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFXs : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ParticleSystem slashFX;
    [SerializeField] private ParticleSystem jumpFx;

    public void Slash()
    {
        slashFX.Play();
    }

    public void JumpFX()
    {
        jumpFx.Play();
    }
}
