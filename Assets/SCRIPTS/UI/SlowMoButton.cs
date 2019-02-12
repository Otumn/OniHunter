using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    public class SlowMoButton : Entity
    {
        [SerializeField] private BulletTime bulletTime;
        [SerializeField] private float timeToSlow = 0.1f;

        private bool canBePressed = false;

        protected override void HeritedUpdate()
        {
            base.HeritedUpdate();
        }

        public void OnEnter()
        {
            canBePressed = true;
            Debug.Log("Entered");
        }

        public void OnExit()
        {
            if (canBePressed) EnableSlowMo();
            canBePressed = false;
            Debug.Log("Exited");
        }

        public void EnableSlowMo()
        {
            if(canBePressed)
            {
                bulletTime.SlowTime(0, timeToSlow);
                Debug.Log("Enabled SlowMo");
            }
        }

        public void DisableSlowMo()
        {
            if (canBePressed)
            {
                bulletTime.SlowTime(1, timeToSlow);
                Debug.Log("Disabled SlowMo");
            }
        }

    }
}
