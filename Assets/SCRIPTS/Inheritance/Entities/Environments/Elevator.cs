using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    public class Elevator : Entity
    {

        #region Serialized Field

        [SerializeField] private Animator anim;

        #endregion

        private void Start()
        {
            
        }

        #region Entity Callbacks

        public override void LevelStart()
        {
            anim.SetInteger("state", 1);
        }

        #endregion

        #region Public Methods

        public void CallElevatorDing()
        {
            GameManager.gameState.CallElevatorDing();
            anim.SetInteger("state", 2);
        }

        #endregion

    }
}
