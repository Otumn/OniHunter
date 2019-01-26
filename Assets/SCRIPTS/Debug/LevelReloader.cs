using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    public class LevelReloader : MonoBehaviour
    {
        #region Private Variables
        [Header("DO NOT USE IN BUILD")]
        [SerializeField] private GameManager testedPrefab;

        #endregion

        #region Monobehaviour callbacks

        private void Start()
        {
            LevelsManager.state.CurrentLoadedLevel = testedPrefab;
        }

        private void Update()
        {

        }

        #endregion

        #region Private Methods



        #endregion

        #region Public Methods
        #endregion
    }
}