using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Pagann.OniHunter {

    [CustomEditor(typeof(SpriteOutline))]
    [ExecuteInEditMode]

    public class SpriteOutlineEditor : Editor
    {
        private SpriteOutline sprOutline;
        private SerializedObject GetTarget;

        void OnEnable()
        {
            sprOutline = (SpriteOutline)target;
            GetTarget = new SerializedObject(sprOutline);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();   // Serializes Script Fields
            GetTarget.Update();

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (GUILayout.Button("Add Outline"))
            {
                sprOutline.InstantiateOutline();
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Remove Outline"))
            {
                sprOutline.RemoveOutline();
            }


            GetTarget.ApplyModifiedProperties();
        }
    }
}
