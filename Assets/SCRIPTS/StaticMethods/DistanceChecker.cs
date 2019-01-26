using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    public class DistanceChecker : MonoBehaviour
    {

        public static bool DistanceCheck(Vector3 vector1, Vector3 vector2, float precision)
        {
            if (Vector3.Distance(vector1, vector2) < precision)
            {
                return true;
            }

            else
            {
                return false;
            }
        }
    }
}
