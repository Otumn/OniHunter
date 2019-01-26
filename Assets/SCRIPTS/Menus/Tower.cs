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

        [SerializeField] private MainMenuManager mMM;
        [SerializeField] private SpriteRenderer visual;
        [SerializeField] private Transform buttonsParent;
        [SerializeField] private List<LevelButton> levelButtons = new List<LevelButton>();
        [SerializeField] private Transform[] buttonPos;
        [SerializeField] private Sprite[] blackNumbers;
        [SerializeField] private Sprite[] whiteNumbers;
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
            description.SetActive(true);
        }

        public void CloseDescription()
        {
            description.SetActive(false);
        }

        public void LoadLevel()
        {
            mMM.LoadGameScene();
        }

        [ContextMenu("Find main menu manager")]
        public void FindMMM()
        {
            mMM = GameObject.Find("MainMenuManager").GetComponent<MainMenuManager>();
        }

        #endregion

        #region Properties

        public SpriteRenderer Visual { get => visual; set => visual = value;}
        public List<LevelButton> LevelButtons { get => levelButtons; }
        public Transform[] ButtonPos { get => buttonPos; }
        public Transform ButtonsParent { get => buttonsParent; set => buttonsParent = value; }
        public Sprite[] WhiteNumbers { get => whiteNumbers; }
        public Sprite[] BlackNumbers { get => blackNumbers; }

        #endregion
    }
}
