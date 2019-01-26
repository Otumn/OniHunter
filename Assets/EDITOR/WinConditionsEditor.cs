using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Pagann.OniHunter
{
    [CustomEditor(typeof(WinConditions))]
    [CanEditMultipleObjects]
    public class WinConditionsEditor : Editor
    {
        private WinConditions win { get => target as WinConditions; }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        }
    }
}
