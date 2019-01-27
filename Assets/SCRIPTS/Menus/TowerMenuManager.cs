using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pagann.OniHunter
{
    public class TowerMenuManager : MonoBehaviour
    {
        #region Private Variables

        [SerializeField] private int numberOfTower = 0;
        private AsyncOperation asyncLoad;

        #endregion

        #region Monobehaviour callbacks

        private void Start()
        {
            StartCoroutine(LoadingAsync());
        }

        #endregion

        #region Public Methods

        public void AddATower()
        {
            numberOfTower++;
        }

        public void LoadGameScene()
        {
            SelectedLevelHolder.lastMainMenuPos = Camera.main.transform.position;
            asyncLoad.allowSceneActivation = true;
        }

        IEnumerator LoadingAsync()
        {
            asyncLoad = SceneManager.LoadSceneAsync(2);
            asyncLoad.allowSceneActivation = false;
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }

        #endregion

        #region Properties

        public int NumberOfTower
        {
            get
            {
                return numberOfTower;
            }
        }

        #endregion
    }
}


