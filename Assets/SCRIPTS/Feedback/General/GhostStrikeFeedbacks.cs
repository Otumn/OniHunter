using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    public class GhostStrikeFeedbacks : Entity
    {
        #region Fields

        [SerializeField] private GameObject camShake;
        [SerializeField] private Camera cam;
        [SerializeField] private Animator vignette;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            camShake.SetActive(false);
        }

        #endregion

        #region Entity Callbacks

        public override void LevelStart()
        {
            camShake.SetActive(false);
        }

        public override void GhostStrikeStartDash()
        {
            camShake.SetActive(true);
        }

        public override void GhostStrikeEndDash()
        {
            camShake.SetActive(false);
            cam.transform.localPosition = new Vector3(-0.5f, 0, -10);
        }

        public override void GhostStrikePreparing()
        {
            base.GhostStrikePreparing();
            vignette.SetTrigger("In");
        }

        public override void GhostStrikeEnd()
        {
            base.GhostStrikeEnd();
            vignette.SetTrigger("Out");
        }

        public override void PlayerDetected(Vector3 detectionPosition)
        {
            camShake.SetActive(false);
        }

        #endregion
    }
}