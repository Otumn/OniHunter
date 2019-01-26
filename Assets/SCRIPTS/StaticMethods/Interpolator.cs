using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    public class Interpolator : MonoBehaviour
    {
        public static float Interpolate(float minValueReturn, float maxValueReturn, float minValueToCheck, float maxValueToCheck, float valueToCheck)
        {
            return Mathf.Lerp(minValueReturn, maxValueReturn, Mathf.InverseLerp(minValueToCheck, maxValueToCheck, valueToCheck));
        }
    }
}