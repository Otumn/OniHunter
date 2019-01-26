using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    public class SelectedLevelHolder : MonoBehaviour
    {
        public static SelectedLevelHolder instanceRef;
        public static string selectedLevel = "";
        public static Vector3 lastMainMenuPos;
        private void Awake()
        {
            if(SelectedLevelHolder.instanceRef == null)
            {
                SelectedLevelHolder.instanceRef = this;
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }
    }
}