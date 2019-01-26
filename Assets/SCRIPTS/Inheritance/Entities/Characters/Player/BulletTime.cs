using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    public class BulletTime : MonoBehaviour
    {
        private bool isSlowing = false;
        private float timeSlowCompletion = 0f;
        private float currentTimeBeforeFullSlow = 0f;
        private float currentTargetTimeScale = 1f;

        /// <summary>
        /// Progressively slows time toward a targetted time scale (0 - 1).
        /// </summary>
        /// <param name="targetTimeScale"> Targetted time scale</param>
        /// <param name="slowSpeed"> How fast will time scale reach its target value?</param>
        public void SlowTime(float targetTimeScale, float slowSpeed)
        {
            isSlowing = true;
            currentTimeBeforeFullSlow = slowSpeed;
            currentTargetTimeScale = targetTimeScale;
            timeSlowCompletion = 0f;
        }

        private void TimeManager()
        {
            if (isSlowing)
            {
                //Debug.Log("Time is slowing : current time Scale = " + Time.timeScale);
                timeSlowCompletion += currentTimeBeforeFullSlow * Time.unscaledDeltaTime;
                Time.timeScale = Interpolator.Interpolate(1f, currentTargetTimeScale, 0, 1, timeSlowCompletion);

                if (timeSlowCompletion >= 1)
                {
                    isSlowing = false;
                }
            }
        }

        private void Update()
        {
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
            TimeManager();
        }
    }
}

