using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Pagann.OniHunter
{
    public class Tower : MonoBehaviour
    {
        #region Private Variables

        [SerializeField] private TowerMenuManager tMM;
        [SerializeField] private RectTransform buttonsParent;
        [SerializeField] private List<LevelButton> levelButtons = new List<LevelButton>();
        [Header("Level Descrition")]
        [SerializeField] private GameObject description;



        #endregion

        #region Monobehaviour callbacks

        private void Start()
        {
            FindMMM();
        }

        private void Update()
        {

        }

        #endregion

        #region Private Methods



        #endregion

        #region Public Methods

        public void PopDescription()
        {
            //add lines depending on the game mode
            description.SetActive(true);
        }

        public void CloseDescription()
        {
            description.SetActive(false);
        }

        public void LoadLevel()
        {
            tMM.LoadGameScene();
        }

        [ContextMenu("Find main menu manager")]
        public void FindMMM()
        {
            tMM = GameObject.Find("TowerMenuManager").GetComponent<TowerMenuManager>();
        }

        #endregion

        #region Properties

        public List<LevelButton> LevelButtons { get => levelButtons; }
        public RectTransform ButtonsParent { get => buttonsParent; set => buttonsParent = value; }

        #endregion
    }
}
