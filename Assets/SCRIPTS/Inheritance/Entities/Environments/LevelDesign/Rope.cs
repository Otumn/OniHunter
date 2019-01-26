using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    public class Rope : Platform
    {
        #region Private Variables

        [SerializeField] private HingeJoint2D joint;
        [SerializeField] private Props attachedProp;
        [SerializeField] [HideInInspector] private List<RopePart> parts;

        private bool cut = false;

        #endregion

        #region Public Methods

        /// <summary>
        /// Break the rope at the joint of brokePart.
        /// </summary>
        /// <param name="brokePart"></param>
        public void BreakRope(RopePart brokePart)
        {

            if (cut) return;
            StartCoroutine(Burning());
            for (int i = 0; i < parts.Count; i++)
            {
                parts[i].Hitbox.enabled = false;
            }
            brokePart.Joint.enabled = false;
            attachedProp.SetMovable();
            cut = true;
        }

        private IEnumerator Burning()
        {
            for (int i = 0; i < parts.Count; i++)
            {
                parts[i].DieInstantly(false);
                yield return new WaitForSeconds(0.05f);
            }
        }

        #endregion

        #region Accessors

        public HingeJoint2D Joint
        {
            get
            {
                return joint;
            }
        }

        /// <summary>
        /// Returns an array of RopePart that make the rope itself, the last object on the list is the lowest in space.
        /// </summary>
        public List<RopePart> Parts
        {
            get
            {
                return parts;
            }
        }

        /// <summary>
        /// Returns the entity this rope is attached to
        /// </summary>
        public Props AttachedProp
        {
            get
            {
                return attachedProp;
            }
        }

        #endregion 

    }
}
