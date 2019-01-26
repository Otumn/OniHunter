using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    public class EnemyFX : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private ParticleSystem crushedFx;

        public void PlayCrushedFX()
        {
            crushedFx.Play();
        }
    }
}
