using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Pagann.OniHunter
{
    public class CreateTowerWizard : ScriptableWizard
    {
        [SerializeField] private string towerName;
        [SerializeField] private Sprite towerVisual;
        [SerializeField] private Sprite lastFloorBackground;
        [SerializeField] private Sprite[] possibleFloorBackground;
        [SerializeField] private Sprite firstFloorBackground;
        [SerializeField] private GameManager[] levels;
        [SerializeField] private bool useBlackNumbers;

        [MenuItem("OniHunter/Create new tower")]
        private static void CreateTower()
        {
            ScriptableWizard.DisplayWizard<CreateTowerWizard>("Create a new Tower", "Create");
        }

        private void OnWizardCreate()
        {
            TowerParameters createdParameters = ScriptableObject.CreateInstance<TowerParameters>();
            createdParameters.TowerName = towerName;
            createdParameters.Visual = towerVisual;
            createdParameters.LastFloorBackground = lastFloorBackground;
            createdParameters.FloorsBackgrounds = possibleFloorBackground;
            createdParameters.FirstFloorBackground = firstFloorBackground;
            createdParameters.Levels = levels;

            GameObject createdTowerGameObject = createdTowerGameObject = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/PREFABS/Main menu/Towers/TowerBase.prefab")) as GameObject;
            createdTowerGameObject.name = towerName;
            createdTowerGameObject.transform.position = new Vector3(GameObject.FindObjectOfType<MainMenuManager>().NumberOfTower * 7.5f, -6.5f, 0);
            for (int i = 0; i < levels.Length; i++)
            {
                GameObject newLevelButton = null;
                if (i == 9)
                {
                    newLevelButton = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/PREFABS/Main menu/FinalLevelButton.prefab")) as GameObject;
                }
                else
                {
                    newLevelButton = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/PREFABS/Main menu/LevelButton.prefab")) as GameObject;
                }
                newLevelButton.transform.position = createdTowerGameObject.GetComponent<Tower>().ButtonPos[i].position;
                newLevelButton.transform.parent = createdTowerGameObject.GetComponent<Tower>().ButtonsParent;
                newLevelButton.name = levels[i].name + "_Button";
                LevelButton actualLevelButton = newLevelButton.GetComponent<LevelButton>();
                actualLevelButton.LevelToLoad = levels[i].name;
                actualLevelButton.MenuTower = createdTowerGameObject.GetComponent<Tower>();
                if (useBlackNumbers)
                    actualLevelButton.NumberImage.sprite = createdTowerGameObject.GetComponent<Tower>().BlackNumbers[i];
                else
                    actualLevelButton.NumberImage.sprite = createdTowerGameObject.GetComponent<Tower>().WhiteNumbers[i];
            }
            PrefabUtility.SaveAsPrefabAsset(createdTowerGameObject, "Assets/PREFABS/Main menu/Towers/"+towerName+".prefab");
            createdParameters.LinkedPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/PREFABS/Main menu/Towers/" + towerName + ".prefab") as GameObject;
            AssetDatabase.CreateAsset(createdParameters, "Assets/SCRIPTABLEOBJECTS/Tower parameters/" + towerName + "Parameters.asset");
            DestroyImmediate(createdTowerGameObject);
            GameObject finaltower = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/PREFABS/Main menu/Towers/"+towerName+".prefab")) as GameObject;
            GameObject.FindObjectOfType<MainMenuManager>().AddATower();
            CreateTowerWizard.UpdateTowerParameters(AssetDatabase.LoadAssetAtPath<TowerParameters>("Assets/SCRIPTABLEOBJECTS/Tower parameters/" + towerName + "Parameters.asset") as TowerParameters);
        }

        public static void UpdateTowerParameters(TowerParameters param)
        {
            if (!EditorUtility.DisplayDialog("Warning", "Make sure to save the level(s) you are currently working on as prefab variant. Any overrides from the prefab will be erased.", "Continue", "Cancel")) return;
            for (int i = 0; i < param.Levels.Length; i++)
            {
                param.Levels[i].TowerParameters = param;
                param.Levels[i].IndexInTower = i;
                param.Levels[i].Background.sprite = param.FloorsBackgrounds[Random.Range(0, param.FloorsBackgrounds.Length)];
                PrefabUtility.SavePrefabAsset(param.Levels[i].gameObject);
            }
            GameManager[] managers = GameObject.FindObjectsOfType<GameManager>();
            if (managers.Length > 0)
            {
                for (int i = 0; i < managers.Length; i++)
                {
                    PrefabUtility.RevertObjectOverride(managers[i], InteractionMode.AutomatedAction);
                }
            }
        }
    }
}
