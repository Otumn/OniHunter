using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    public class CameraFX : MonoBehaviour
    {
        [SerializeField] private KeyCode debug = KeyCode.H;

        [Header("Components")]
        [SerializeField] private ParticleSystem explosionFx;        

        public void PlayExplosionFX()
        {
            explosionFx.Play();
        }

        private void Update()
        {
            Debugs();
        }

        private void Debugs()
        {
            if(Input.GetKeyDown(debug))
            {
                PlayExplosionFX();
            }
        }
    }
}
