using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    public class TrailerLauncher : MonoBehaviour
    {

        [SerializeField] private UnityEngine.Video.VideoPlayer player;
        [SerializeField] private AudioSource sound;

        // Start is called before the first frame update
        void Start()
        {
            player.Play();
            sound.Play();
            
        }

        private void Update()
        {
            Debug.Log(sound.isPlaying);
        }
    }
}
