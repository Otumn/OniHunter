using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Pagann.OniHunter
{
    [CustomEditor(typeof(PrefabBrush))]
    public class PrefabBrushEditor : GridBrushEditorBase
    {
        private PrefabBrush prefabBrush { get { return target as PrefabBrush;  } }

        private SerializedProperty prefabs;
        private SerializedObject serialized;

        protected void OnEnable()
        {
            serialized = new SerializedObject(target);
            prefabs = serialized.FindProperty("prefabs");
        }

        public override void OnPaintInspectorGUI()
        {
            serialized.UpdateIfRequiredOrScript();
            prefabBrush.chosenGameObject = EditorGUILayout.IntField("Prefab index", prefabBrush.chosenGameObject);
            prefabBrush.zPos = EditorGUILayout.IntField("Z Positon", prefabBrush.zPos);

            EditorGUILayout.PropertyField(prefabs, true);
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}
