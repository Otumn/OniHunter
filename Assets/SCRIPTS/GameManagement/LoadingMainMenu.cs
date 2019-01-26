using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pagann.OniHunter
{
    public class LoadingMainMenu : MonoBehaviour
    {

        private void Start()
        {
            StartCoroutine(LoadingAsync());
        }

        IEnumerator LoadingAsync()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);
            while(!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }
}
