using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    public class BlinkManager : MonoBehaviour
    {
        #region Fields

        [SerializeField] private int blinkNumber;
        [SerializeField] private float blinkDelay;
        [SerializeField] private float blinkTime;

        #endregion

        #region Properties

        public int BlinkNumber { get => blinkNumber; set => blinkNumber = value; }
        public float BlinkDelay { get => blinkDelay; set => blinkDelay = value; }
        public float BlinkTime { get => blinkTime; set => blinkTime = value; }

        #endregion
    }
}
