using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    public class Marker : MonoBehaviour
    {
        #region Private Variables

        [SerializeField] private GameObject trajectory;
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private Animator anim;
        [SerializeField] private float widthRatio = 0.9f;
        private Transform castTarget;
        private float castWidth = 0.5f;
        private bool shouldCast = false;

        #endregion

        #region Monobehaviour callbacks

        private void Update()
        {
            if(shouldCast)
            {
                Cast(castTarget, castWidth);
            }
        }

        #endregion

        #region Private Methods

        private void Cast(Transform target, float width)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, target.position);
            float zRot = Vector3.SignedAngle(target.up, direction, target.forward) + 90;
            trajectory.transform.localScale = new Vector3(distance, width, trajectory.transform.localScale.z);
            trajectory.transform.position = Vector3.Lerp(transform.position, target.position, 0.5f);
            trajectory.transform.rotation = Quaternion.Euler(0, 0, zRot);
        }

        #endregion

        #region Public Methods

        public void InitiateCast(Transform target, float width)
        {
            castTarget = target;
            castWidth = width * widthRatio;
            shouldCast = true;
        }

        #endregion

        #region Properties

        public bool ShouldCast
        {
            get
            {
                return shouldCast;
            }

            set
            {
                shouldCast = value;
            }
        }

        public SpriteRenderer Sprite { get => sprite; set => sprite = value; }
        public Animator Anim { get => anim; set => anim = value; }

        #endregion
    }
}
