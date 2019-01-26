using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    public class RopePart : MonoBehaviour, IDamageListener
    {
        #region Private Variables

        [SerializeField] private Rigidbody2D body;
        [SerializeField] private HingeJoint2D joint;
        [SerializeField] private Collider2D hitbox;
        [SerializeField] private Animator anim;
        [SerializeField] [HideInInspector] private Rope attachedRope;

        #endregion

        #region Monobehaviour Callbacks

        private void OnEnable()
        {
            if(joint.connectedBody == null)
            {
                joint.enabled = false;
            }
        }

        #endregion


        public void DamageSelf()
        {
            hitbox.enabled = false;
            attachedRope.BreakRope(this);            
        }

        public void DieInstantly(bool crushed)
        {
            anim.SetTrigger("Out");
        }

        public void ShowInteraction()
        {
            
        }

        public void HideInteraction()
        {
            
        }

        #region Accessors

        public Rigidbody2D Body
        {
            get
            {
                return body;
            }
        }

        /// <summary>
        /// Return the HingeJoint2D of the ropePart.
        /// </summary>
        public HingeJoint2D Joint
        {
            get
            {
                return joint;
            }
        }

        /// <summary>
        /// Return the Rope that this part is attached to;
        /// </summary>
        public Rope AttachedRope
        {
            get
            {
                return attachedRope;
            }
            set
            {
                attachedRope = value;
            }
        }

        public Collider2D Hitbox { get => hitbox; }

        #endregion
    }
}
