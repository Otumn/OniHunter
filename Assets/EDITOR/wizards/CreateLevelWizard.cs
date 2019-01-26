using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Pagann.OniHunter
{
    public class CreateLevelWizard : ScriptableWizard
    {
        //[SerializeField] private string levelName = "Level";
        [SerializeField] private int world = 1;
        [SerializeField] private int tower = 1;
        [SerializeField] private int level = 1;
        //[SerializeField] private bool DestroyOtherLevelInScene = false;

        [MenuItem("OniHunter/Create or load a level")]
        private static void CreateLevel()
        {
            ScriptableWizard.DisplayWizard<CreateLevelWizard>("Create or load level", "Create/Load");
        }

        private void OnWizardCreate()
        {
            /*if(DestroyOtherLevelInScene)
            {
                GameObject[] managers = Resources.FindObjectsOfTypeAll(typeof(GameManager)) as GameObject[];
                if (managers.Length > 0)
                {
                    for (int i = 0; i < managers.Length; i++)
                    {
                        DestroyImmediate(managers[i].gameObject);
                    }
                }
            }*/
            string levelName = "W" + world.ToString() + "_T" + tower.ToString() + "_L" + level.ToString();
            GameObject lvl;
            if(Resources.Load("Levels/"+levelName) != null)
            {
                lvl = PrefabUtility.InstantiatePrefab(Resources.Load("Levels/" + levelName)) as GameObject;
                lvl.name = levelName;
            }
            else
            {
                lvl = PrefabUtility.InstantiatePrefab(Resources.Load("Levels/LevelBase")) as GameObject;
                lvl.name = levelName;
            }
            lvl.transform.position = Vector3.zero;
        }
    }
}
