using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    [CreateAssetMenu(fileName = "New Tower Parameters", menuName = "OniHunter/Tower Parameters")]
    public class TowerParameters : ScriptableObject
    {
        #region Private Variables

        [SerializeField] private string towerName;
        [SerializeField] private Sprite visual;
        [SerializeField] private Sprite lastFloorBackground;
        [SerializeField] private Sprite[] floorsBackgrounds;
        [SerializeField] private Sprite firstFloorBackground;
        [SerializeField] private GameManager[] levels;
        //[SerializeField] private GameObject linkedPrefab;

        #endregion

        #region Properties

        /// <summary>
        /// The name of the tower.
        /// </summary>
        public string TowerName
        {
            get
            {
                return towerName;
            }
            set
            {
                towerName = value;
            }
        }

        /// <summary>
        /// The visual of the tower inside the main menu.
        /// </summary>
        public Sprite Visual
        {
            get
            {
                return visual;
            }
            set
            {
                visual = value;
            }
        }

        /// <summary>
        /// The background of the last floor of the tower.
        /// </summary>
        public Sprite LastFloorBackground
        {
            get
            {
                return lastFloorBackground;
            }

            set
            {
                lastFloorBackground = value;
            }
        }

        /// <summary>
        /// The array of possible floor backgrounds of the tower.
        /// </summary>
        public Sprite[] FloorsBackgrounds
        {
            get
            {
                return floorsBackgrounds;
            }

            set
            {
                floorsBackgrounds = value;
            }
        }

        /// <summary>
        /// The background of the first floor of the tower.
        /// </summary>
        public Sprite FirstFloorBackground
        {
            get
            {
                return firstFloorBackground;
            }

            set
            {
                firstFloorBackground = value;
            }
        }

        /// <summary>
        /// Returns all the levels that this TowerParameters contains.
        /// </summary>
        public GameManager[] Levels
        {
            get
            {
                return levels;
            }

            set
            {
                levels = value;
            }
        }

        //public GameObject LinkedPrefab { get => linkedPrefab; set => linkedPrefab = value; }

        #endregion
    }
}
