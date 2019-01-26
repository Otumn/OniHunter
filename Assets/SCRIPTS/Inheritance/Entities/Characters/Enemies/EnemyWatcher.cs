using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    public class EnemyWatcher : Enemy, IDamageListener
    {
        [Header("Line Of Sight")]
        [SerializeField] private GameObject lineOfSight;
        [SerializeField] private Collider2D lineOfSightCollider;
        [SerializeField] private SpriteRenderer lineOfSightSprite;
        [SerializeField] private Animator lineOfSightAnim;
        [SerializeField] private float losDistance = 0.8f;
        private Vector3 initialLosScale;

        protected override void HeritedStart()
        {
            base.HeritedStart();
            initialLosScale = lineOfSight.transform.localScale;
        }

        protected override void HeritedUpdate()
        {
            base.HeritedUpdate();
            FlipLineOfSight();
        }

        public override void DieInstantly(bool crushed)
        {
            base.DieInstantly(crushed);
            lineOfSightAnim.SetTrigger("Out");
            lineOfSightCollider.enabled = false;
        }

        [ContextMenu("AcquireLosSprite")]
        public void AcquireLineOfSightSprite()
        {
            if(lineOfSightSprite == null)
            {
                lineOfSightSprite = lineOfSight.GetComponent<SpriteRenderer>();
            }
        }

        private void FlipLineOfSight()
        {
            //lineOfSightSprite.flipX = Sprite.flipX;
            switch (Sprite.flipX)
            {
                case true:
                    lineOfSight.transform.localPosition = new Vector3(losDistance, lineOfSight.transform.localPosition.y, lineOfSight.transform.localPosition.z);
                    lineOfSight.transform.localScale = new Vector3(-initialLosScale.x, initialLosScale.y, initialLosScale.z);
                    break;
                case false:
                    lineOfSight.transform.localPosition = new Vector3(-losDistance, lineOfSight.transform.localPosition.y, lineOfSight.transform.localPosition.z);
                    lineOfSight.transform.localScale = new Vector3(initialLosScale.x, initialLosScale.y, initialLosScale.z);
                    break;
            }            
        }

        public new void DamageSelf()
        {
            base.DamageSelf();
            lineOfSightAnim.SetTrigger("Out");
            lineOfSightCollider.enabled = false;
        }
    }
}


