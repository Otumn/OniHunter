using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    public class ElectricProp : Props
    {
        #region Protected Variables
        [Header("Electric Prop Parameters")]
        [SerializeField] protected bool isOn = true;

        #endregion

        #region Public Method

        [ContextMenu("Activate")]
        public virtual void Activate()
        {
            isOn = true;
        }

        [ContextMenu("Deactivate")]
        public virtual void Deactivate()
        {
            isOn = false;
        }

        #endregion

        #region Accessors

        public bool IsOn
        {
            get
            {
                return isOn;
            }
        }

        #endregion

    }
}
