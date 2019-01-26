using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    public class ButtonFX : MonoBehaviour
    {
        [SerializeField] private ParticleSystem activationFx;
        [SerializeField] private ParticleSystem explosionFx;

        public void PlayActivationFX()
        {
            activationFx.Play();
        }

        public void PlayExplosionFX()
        {
            explosionFx.Play();
        }
    }
}
