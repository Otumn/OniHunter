using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Pagann.OniHunter
{
    [CustomEditor(typeof(TowerParameters))]
    [CanEditMultipleObjects]
    public class TowerParametersEditor : Editor
    {
        public TowerParameters param { get { return target as TowerParameters; } }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("Update Tower"))
            {
                CreateTowerWizard.UpdateTowerParameters(param);
            }
        }
    }
}
