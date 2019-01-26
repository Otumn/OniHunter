using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pagann.OniHunter
{
    public class TrailerSwitcher : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                SceneManager.LoadScene(3);
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
