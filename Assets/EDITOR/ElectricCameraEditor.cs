using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Pagann.OniHunter
{
    [CustomEditor(typeof(ElectricCamera))]
    [CanEditMultipleObjects]
    public class ElectricCameraEditor : Editor
    {
        private ElectricCamera ElecCam { get { return target as ElectricCamera; } }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if(!EditorApplication.isPlaying)
            {
                EditorGUI.BeginChangeCheck();
                int state = ElecCam.StartState;
                bool isOn = ElecCam.IsOn;
                ElecCam.Pivot.transform.rotation = Quaternion.Euler(0, 0, 90f * state);
                ElecCam.LineOfSightColl.enabled = isOn;
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "Changed start state");
                }
            }
        }
    }
}
