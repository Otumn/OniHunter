using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Pagann.OniHunter
{
    [CustomEditor(typeof(Tower))]
    [CanEditMultipleObjects]
    public class TowerEditor : Editor
    {
        #region Private Variables

        private Tower tower { get => target as Tower; }

        #endregion

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if(GUILayout.Button("Delete tower"))
            {
                if(EditorUtility.DisplayDialog("WARNING", "By clicking 'OK' you will the prefab and the scriptable object of this tower. This can not be undone.", "OK", "Cancel"))
                {
                    AssetDatabase.DeleteAsset("Assets/SCRIPTABLEOBJECTS/Tower parameters/" + tower.name + "Parameters.asset");
                    AssetDatabase.DeleteAsset("Assets/PREFABS/Main menu/Towers/" + tower.name + ".prefab");
                    DestroyImmediate(tower.gameObject);
                }
            }
        }
    }
}


