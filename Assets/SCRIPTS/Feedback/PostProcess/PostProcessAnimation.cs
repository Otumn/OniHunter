using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Pagann.OniHunter
{
    public class PostProcessAnimation : MonoBehaviour
    {
        #region Fields

        [SerializeField] private PostProcessVolume volume;
        [SerializeField] private float speed = 1f;
        private bool isPlaying = false;
        private float target = 0f;
        private float progress = 0f;
        private bool turnsOn = false;
        private bool isPingPong = false;

        #endregion

        #region MonoBehaviour Callbacks

        private void Update()
        {
            AnimationManager();
        }

        #endregion
        
        #region Public Methods

        /// <summary>
        /// Turns on or off (or both) the assigned Volume.
        /// </summary>
        /// <param name="on"> Should the volume turn on or off? </param>
        /// <param name="pingPong"> After finishing turning on/off, should it go back to initial state?</param>
        public void PlayAnim(bool on, bool pingPong)
        {
            isPlaying = true;
            turnsOn = on;
            isPingPong = pingPong;
            if(on)
            {
                progress = 0f;
            }

            else
            {
                progress = 1f;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Manages the whole animation
        /// </summary>
        private void AnimationManager()
        {
            if (turnsOn)
            {
                if (isPlaying)
                {
                    if (progress < 1f)
                    {
                        progress += Time.unscaledDeltaTime * speed;
                        volume.weight = progress;
                    }

                    else
                    {
                        isPlaying = false;
                        volume.weight = 1f;
                        if (isPingPong)
                        {
                            PlayAnim(false, false);
                        }
                    }
                }
            }

            else
            {
                if (isPlaying)
                {
                    if (progress > 0f)
                    {
                        progress -= Time.unscaledDeltaTime * speed;
                        volume.weight = progress;
                    }

                    else
                    {
                        isPlaying = false;
                        volume.weight = 0f;
                        if (isPingPong)
                        {
                            PlayAnim(true, false);
                        }
                    }
                }
            }
        }

        #endregion
    }
}
