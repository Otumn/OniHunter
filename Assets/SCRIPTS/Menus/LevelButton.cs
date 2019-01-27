using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pagann.OniHunter
{
    public class LevelButton : MenuTouchable
    {
        #region Private Variables

        [SerializeField] private Tower menuTower;
        [SerializeField] private string levelToLoad;
        [SerializeField] private WinConditions.SideObjective gameMode;
        [SerializeField] private Text levelNumber;

        #endregion

        #region Public Methods

        public void PopUpLevelDescription()
        {
            menuTower.PopDescription();
            SelectedLevelHolder.selectedLevel = levelToLoad;
        }

        #endregion

        #region Properties

        public string LevelToLoad
        {
            get
            {
                return levelToLoad;
            }
            set
            {
                levelToLoad = value;
            }
        }

        public WinConditions.SideObjective GameMode { get => gameMode; }
        public Tower MenuTower { get => menuTower; set => menuTower = value; }
        public Text LevelNumber { get => levelNumber; }

        #endregion
    }
}
