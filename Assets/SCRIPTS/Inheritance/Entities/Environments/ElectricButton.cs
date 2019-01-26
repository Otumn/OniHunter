using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    public class ElectricButton : Props, IDamageListener
    {
        #region Private Variables
        [Header("FXs")]
        [SerializeField] private ButtonFX fx;

        [Header("Electric Camera")]
        [SerializeField] private Animator anim;
        [SerializeField] private GameObject sprite;
        [SerializeField] private Collider2D hitbox;
        [SerializeField] private ElectricProp[] electricProps;
        [SerializeField] private Color testingLinkColor;
        [SerializeField] private CableManager cableManager;

        private bool activated = false;
        #endregion

        #region MonoBehaviour CallBacks

        protected override void HeritedUpdate()
        {
            base.HeritedUpdate();
            if (electricProps.Length > 0)
            {
                for (int i = 0; i < electricProps.Length; i++)
                {
                    Debug.DrawLine(transform.position, electricProps[i].transform.position, testingLinkColor);
                }
            }
        }

        #endregion

        #region IDamageListener

        [ContextMenu("Click")]
        public void DamageSelf()
        {
            if (electricProps.Length > 0)
            {

                for (int i = 0; i < electricProps.Length; i++)
                {
                    if (electricProps[i].IsOn)
                    {
                        electricProps[i].Deactivate();
                    }
                    else
                    {
                        electricProps[i].Activate();
                    }
                }

                cableManager.SwitchCables();
                activated = !activated;
                anim.SetBool("Activate", activated);
                fx.PlayActivationFX();

            }
        }

        public void DieInstantly(bool crushed)
        {
            cableManager.MeltCables();
            fx.PlayExplosionFX();
            sprite.SetActive(false);
            hitbox.enabled = false;
            StartCoroutine(Deactivate());
            //BRULER LES KABLS
        }

        private IEnumerator Deactivate()
        {
            yield return new WaitForSeconds(1.5f);
            gameObject.SetActive(false);
        }

        public void ShowInteraction()
        {

        }

        public void HideInteraction()
        {

        }

        #endregion

        #region Accessors

        /// <summary>
        /// Returns all the electricProps listening to this button.
        /// </summary>
        public ElectricProp[] ElectricProps
        {
            get
            {
                return electricProps;
            }
        }

        #endregion
    }
}
