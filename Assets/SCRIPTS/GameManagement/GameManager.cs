using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    public class GameManager : MonoBehaviour
    {
        #region Private Variables

        [SerializeField] private WinConditions winConditions;
        [SerializeField] private SpriteRenderer background;
        [SerializeField] private bool testing = false;
        [SerializeField] private LevelParameters levelParameters;
        [SerializeField] private CamZoom camZoom;
        [SerializeField] private UIManager ui;
        [SerializeField] private ObjectFollow camFollow;
        [SerializeField] private BlinkManager blinkManager;
        [SerializeField] private AnimationCurve elevatorCurve;
        [SerializeField] [HideInInspector] private TowerParameters towerParameters;
        [SerializeField] [HideInInspector] private int indexInTower;
        private int[] test;
        private bool shouldProgress = false;
        private float elevatorProgression = 0f;
        private bool destroyWhenFinished = false;
        private bool entering = true;

        #endregion

        #region Public Variables

         /// <summary>
         /// The actual state of the game. 
         /// </summary>
        public static GameState gameState = new GameState();

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            if(testing)
            {
                FindReferences();
                GameManager.gameState.CallLevelStart();
            }

            Application.targetFrameRate = 60;
        }

        private void Update()
        {
            if(shouldProgress)
            {
                elevatorProgression += Time.deltaTime;
                if(entering)
                    transform.position = new Vector3(0, elevatorCurve.Evaluate(elevatorProgression), 0);
                else
                    transform.position = new Vector3(0, elevatorCurve.Evaluate(elevatorProgression) - elevatorCurve.keys[0].value, 0);
                if (elevatorProgression >= 1)
                {
                    elevatorProgression = 0;
                    shouldProgress = false;
                    if (destroyWhenFinished)
                    {
                        Destroy(this.gameObject);
                    }
                    else
                        GameManager.gameState.CallLevelStart();
                }
            }
        }

        #endregion

        #region Public Methods

        public void StartProgression(bool goingIn, bool destroy)
        {
            shouldProgress = true;
            destroyWhenFinished = destroy;
            entering = goingIn;
        }

        /// <summary>
        /// Set useful references from the level Manager, called when instantiating a new level into the scene.
        /// </summary>
        /// <param name="zoom"></param>
        /// <param name="follow"></param>
        /// <param name="ui"></param>
        public void SetReferences(CamZoom zoom, ObjectFollow follow, UIManager uiMng)
        {
            camZoom = zoom;
            camFollow = follow;
            ui = uiMng;
            gameState.Initialize(levelParameters, camZoom, this, camFollow, ui, blinkManager);                       
        }

        /// <summary>
        /// Use only with context menu, contains GameObject.find().
        /// </summary>
        [ContextMenu("Find references")]
        private void FindReferences()
        {
            LevelsManager lvlManager = GameObject.Find("LevelsManager").GetComponent<LevelsManager>();
            lvlManager.LinkGameManager(this);
        }

        #endregion

        #region Accessors

        public bool ReferencesState
        {
            get
            {
                if (camFollow != null && camFollow != null && ui != null)
                    return true;
                else
                    return false;
            }
        }

        public int IndexInTower
        {
            get
            {
                return indexInTower;
            }

            set
            {
                indexInTower = value;
            }
        }

        /// <summary>
        /// Returns informations about the tower this level belongs to.
        /// </summary>
        public TowerParameters TowerParameters
        {
            get
            {
                return towerParameters;
            }

            set
            {
                towerParameters = value;
            }
        }

        public SpriteRenderer Background { get => background; }
        public WinConditions WinConditions { get => winConditions; }

        #endregion
    }
}

