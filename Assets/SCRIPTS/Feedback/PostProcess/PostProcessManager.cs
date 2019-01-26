using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Pagann.OniHunter
{  
    public class PostProcessManager : Entity
    {
        #region Fields

        [Header("Post Process")]
        [SerializeField] private PostProcessAnimation[] postProcessAnimations;

        [Header("Debug")]
        [SerializeField] private KeyCode debug = KeyCode.I;
        [SerializeField] private int ppToTest;
        [SerializeField] private bool testOn = false;
        [SerializeField] private bool testPing = false;

        #endregion

        #region MonoBehaviour Callbacks

        protected override void HeritedUpdate()
        {
            Debugging();
        }

        #endregion

        #region Entity Callbacks

        public override void GhostStrikePreparing()
        {
            base.GhostStrikeStart();
            PlayPostProcessAnim(0, true, false);
        }

        public override void GhostStrikeStart()
        {
            base.GhostStrikeEnd();
            PlayPostProcessAnim(0, false, false);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Play one of the post process weight animation.
        /// </summary>
        /// <param name="postProcess">The index of the PostProcessing Volume to animate. </param>
        public void PlayPostProcessAnim(int pp, bool on, bool ping)
        {
            postProcessAnimations[pp].PlayAnim(on, ping);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Debug Method.
        /// </summary>
        private void Debugging()
        {
            if (Input.GetKeyDown(debug))
            {
                PlayPostProcessAnim(ppToTest, testOn, testPing);
            }
        }

        #endregion
    }
}
