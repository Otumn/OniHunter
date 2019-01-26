using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    public class ObjectFollow : Entity
    {

        [Header("Parameters")]
        private bool isFollowing = false;
        [Tooltip("Smoothness of the tracking.")]
        [SerializeField] private float smoothness = 0.5f;
        [SerializeField] private float dezoomTiming = 0.5f;
        private Vector3 currentVel;
        private Vector3 followTarget = Vector3.zero;
        private Vector3 initialPos;

        protected override void HeritedStart()
        {
            base.HeritedStart();
            initialPos = transform.position;
        }

        protected override void HeritedUpdate()
        {
            base.HeritedUpdate();
            if (isFollowing)
            {
                CameraFollow();
            }
        }

        public override void PlayerDetected(Vector3 detectionPosition)
        {
            followTarget = detectionPosition;
            StartCoroutine(Dezoom());
        }

        private void CameraFollow()
        {
            transform.position = Vector3.SmoothDamp(transform.position, followTarget, ref currentVel, smoothness, 5000, Time.unscaledDeltaTime);
            transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        }

        private IEnumerator Dezoom()
        {
            yield return new WaitForSecondsRealtime(dezoomTiming);
            followTarget = initialPos;
        }

        #region Properties       

        public bool IsFollowing
        {
            get
            {
                return isFollowing;
            }

            set
            {
                isFollowing = value;
            }
        }

        #endregion
    }
}