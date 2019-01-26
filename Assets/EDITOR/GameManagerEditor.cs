using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Pagann.OniHunter
{
    [CustomEditor(typeof(GameManager))]
    [CanEditMultipleObjects]
    public class GameManagerEditor : Editor
    {
        public GameManager manager { get { return target as GameManager; } }

        public override void OnInspectorGUI()
        {
            if(manager.TowerParameters == null)
                GUILayout.Label("This level doesn't have an assigned tower");
            else
                GUILayout.Label("Assigned tower : " + manager.TowerParameters.TowerName);
            DrawDefaultInspector();


            if(!manager.ReferencesState)
                GUILayout.Label("This Manager is missing references, please look for references");
            else
                GUILayout.Label("All the references are assigned");


            if(GUILayout.Button("Link all enemies to platforms"))
            {
                Enemy[] allEnemies = GameObject.FindObjectsOfType<Enemy>();
                Debug.Log("there's " + allEnemies.Length + " enemies in the scene");
                List<Enemy> enemies = new List<Enemy>();
                List<FallingPlatform> parents = new List<FallingPlatform>();
                bool shouldChange = false;
                for (int i = 0; i < allEnemies.Length; i++)
                {
                    RaycastHit2D hit = Physics2D.Raycast(allEnemies[i].transform.position + Vector3.down * 1.5f, Vector3.up, 1.5f);
                    Debug.DrawRay(allEnemies[i].transform.position, Vector3.down, Color.magenta, 1.5f);
                    if(hit.collider != null)
                    {
                        if (hit.collider.GetComponent<FallingPlatform>() != null)
                        {
                            Debug.Log(allEnemies[i].name + " detected a platform");
                            enemies.Add(allEnemies[i]);
                            parents.Add(hit.collider.GetComponent<FallingPlatform>());
                            shouldChange = true;
                        }
                    }
                }
                if(!shouldChange)
                {
                    EditorUtility.DisplayDialog("No changes", "No enemy in the scene needs to be reparented!", "Ok");
                    return;
                }
                if (!EditorUtility.DisplayDialog("Possible changes detected.", "One or more enemies need to be reparented. Would you like to proceed?", "Yes", "No"))
                {
                    return;
                }
                PrefabUtility.SaveAsPrefabAsset(manager.gameObject, "Assets/Resources/Levels/" + manager.name);
                /*if (Resources.Load("Levels/" + manager.name))
                {
                    if (EditorUtility.DisplayDialog("Overwriting.", "To complete this action, the prefab will be overwriten. Do you want to continue?", "Yes", "No"))
                    {
                        PrefabUtility.UnpackPrefabInstance(manager.gameObject, PrefabUnpackMode.Completely, InteractionMode.UserAction);
                        AssetDatabase.DeleteAsset("Assets/Resources/Levels/" + manager.name);
                        for (int i = 0; i < enemies.Count; i++)
                        {
                            enemies[i].transform.parent.parent = parents[i].transform;
                        }
                        if(EditorUtility.DisplayDialog("RevertPrefabInstance", "Do you want to continue?", "Yes", "No"))
                        {
                            //PrefabUtility.RevertPrefabInstance(manager.gameObject, InteractionMode.UserAction);
                        }
                        
                    }
                }
                else
                {
                    if(EditorUtility.DisplayDialog("Prefab needed", "To complete this action, a prefab of the level needs to be created. Do you want to continue?", "Yes", "No"))
                    {
                        // same but new prefab
                    }
                }*/
            }
        }
    }
}
