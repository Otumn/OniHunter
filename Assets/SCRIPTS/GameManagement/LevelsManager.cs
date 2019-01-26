using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pagann.OniHunter
{
    public class LevelsManager : MonoBehaviour
    {
        #region Private Variables

        [SerializeField] private bool shouldLoadLevels = true;
        [SerializeField] private bool useMainMenu = false;
        [Space(10)]
        [Header("First level to instantiate")]
        [SerializeField] private int world = 1;
        [SerializeField] private int tower = 1;
        [SerializeField] private int level = 1;
        [Header("Management references")]
        [SerializeField] private CamZoom camZoom;
        [SerializeField] private UIManager ui;
        [SerializeField] private ObjectFollow camFollow;
        private string levelName;

        #endregion

        #region Public Variables

        public static LevelState state = new LevelState();

        #endregion

        #region Monobehaviour Callbacks

        private void Start()
        {
            LevelsManager.state.SaveUtility(camZoom, ui, camFollow);
            if(useMainMenu)
            {
                if(SelectedLevelHolder.selectedLevel == "")
                {
                    levelName = "W_" + world.ToString() + "T_" + tower.ToString() + "L_" + level.ToString();
                }
                else
                {
                    levelName = SelectedLevelHolder.selectedLevel;
                }
            }
            else
            {
                levelName = "W_" + world.ToString() + "T_" + tower.ToString() + "L_" + level.ToString();
            }

            if (shouldLoadLevels)
            {               
                LevelsManager.LoadNewLevel(levelName);
            }
        }

        #endregion

        public void LinkGameManager(GameManager manager)
        {
            manager.SetReferences(camZoom, camFollow, ui);
        }

        public static void LoadNewLevel(string levelName)
        {
            if (LevelsManager.state.CurrentLoadedLevel != null) // check if we are loading a level from scratch, or if we are loading the next level.
            {
                LevelsManager.state.PreviousLoadedLevel = LevelsManager.state.CurrentLoadedLevel;
                LevelsManager.state.PreviousLoadedLevel.StartProgression(false, true);
            }
            GameObject newlevel = Instantiate(Resources.Load("Levels/" + levelName)) as GameObject;
            newlevel.transform.position = new Vector3(0, 13.07f, 0);
            newlevel.name = LevelsManager.GetRealPrefabName(newlevel);
            LevelsManager.state.CurrentLoadedLevel = newlevel.GetComponent<GameManager>();
            newlevel.GetComponent<GameManager>().SetReferences(LevelsManager.state.Zoom, LevelsManager.state.CamFollow, LevelsManager.state.UImanager);
            GameManager.gameState.UiManager.Hide();
            LevelsManager.state.CurrentLoadedLevel.StartProgression(true, false);
        }

        public static void ReloadCurrentLevel()
        {
            string name = LevelsManager.state.CurrentLoadedLevel.gameObject.name;
            Destroy(LevelsManager.state.CurrentLoadedLevel.gameObject);
            GameObject newlevel = Instantiate(Resources.Load("Levels/" + name)) as GameObject;
            newlevel.transform.position = new Vector3(0, 13.07f, 0);
            newlevel.name = LevelsManager.GetRealPrefabName(newlevel);
            LevelsManager.state.CurrentLoadedLevel = newlevel.GetComponent<GameManager>();
            newlevel.GetComponent<GameManager>().SetReferences(LevelsManager.state.Zoom, LevelsManager.state.CamFollow, LevelsManager.state.UImanager);
            LevelsManager.state.CurrentLoadedLevel.StartProgression(true, false);
            GameManager.gameState.UiManager.Hide();
            
        }

        #region GameManager Version of the loading methods
        /*
        public static GameManager LoadNewLevel(string levelName)
        {
            if(LevelsManager.state.CurrentLoadedLevel != null) // check if we are loading a level from scratch, or if we are loading the next level.
            {
                LevelsManager.state.PreviousLoadedLevel = LevelsManager.state.CurrentLoadedLevel;
                LevelsManager.state.PreviousLoadedLevel.StartProgression(false, true);
            }
            GameObject newlevel = Instantiate(Resources.Load("Levels/" + levelName)) as GameObject;
            newlevel.name = LevelsManager.GetRealPrefabName(newlevel);
            return newlevel.GetComponent<GameManager>();
        }

        public static GameManager ReloadCurrentLevel()
        {
            string name = LevelsManager.state.CurrentLoadedLevel.gameObject.name;
            Destroy(LevelsManager.state.CurrentLoadedLevel.gameObject);
            GameObject newlevel = Instantiate(Resources.Load("Levels/" + name)) as GameObject;
            newlevel.name = LevelsManager.GetRealPrefabName(newlevel);
            return newlevel.GetComponent<GameManager>();
        }
        */
        #endregion

        public static string GetRealPrefabName(GameObject GO)
        {
            string newName = "";
            for (int i = 0; i < GO.name.Length - 7; i++)
            {
                newName += GO.name[i];
            }
            return newName;
        }

        public void NextLevel()
        {
            //Debug.Log(LevelsManager.state.CurrentLoadedLevel.TowerParameters.Levels[LevelsManager.state.CurrentLoadedLevel.IndexInTower + 1].gameObject.name);
            if(LevelsManager.state.CurrentLoadedLevel.IndexInTower + 1 == LevelsManager.state.CurrentLoadedLevel.TowerParameters.Levels.Length)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                LevelsManager.LoadNewLevel(LevelsManager.state.CurrentLoadedLevel.TowerParameters.Levels[LevelsManager.state.CurrentLoadedLevel.IndexInTower + 1].gameObject.name);
            }
        }

        public void Reload()
        {
            LevelsManager.ReloadCurrentLevel();
        }

        public void GoToMainMenu()
        {
            SceneManager.LoadScene(0);
        }

    }



    public class LevelState
    {
        #region Private Variables

        private GameManager currentLoadedLevel;
        private GameManager previousLoadedLevel;
        private CamZoom camZoom;
        private UIManager ui;
        private ObjectFollow camFollow;

        #endregion

        #region Public Methods

        public void SaveUtility(CamZoom zoom, UIManager uiMng, ObjectFollow flw)
        {
            camZoom = zoom;
            ui = uiMng;
            camFollow = flw;
        }

        #endregion

        #region Accessors

        public CamZoom Zoom
        {
            get
            {
                return camZoom;
            }
        }

        public UIManager UImanager
        {
            get
            {
                return ui;
            }
        }

        public ObjectFollow CamFollow
        {
            get
            {
                return camFollow;
            }
        }

        /// <summary>
        /// Returns the GameManager that is currently loaded.
        /// </summary>
        public GameManager CurrentLoadedLevel
        {
            get
            {
                return currentLoadedLevel;
            }

            set
            {
                currentLoadedLevel = value;
            }
        }

        /// <summary>
        /// Returns the GameManager that was previously loaded.
        /// </summary>
        public GameManager PreviousLoadedLevel
        {
            get
            {
                return previousLoadedLevel;
            }

            set
            {
                previousLoadedLevel = value;
            }
        }
        #endregion
    }
}
