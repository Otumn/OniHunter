using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    public class ElectricCamBlink : MonoBehaviour
    {
        #region Private Variables

        [SerializeField] private Animator anim;

        #endregion

        #region Monobehaviour callbacks

        private void Start()
        {

        }

        private void Update()
        {

        }

        #endregion

        #region Private Methods



        #endregion


        #region Public Methods

        public void ChooseBlinking()
        {
            int c = Random.Range(0, 4);
            if(c == 0)
            {
                anim.SetBool("blink", true);
            }
        }

        public void ResetBlink()
        {
            anim.SetBool("blink", false);
        }


        #endregion
    }
}
