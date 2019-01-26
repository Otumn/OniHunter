using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Pagann.OniHunter
{
    /// <summary>
    /// Contains several informations about the overrall state of the level.
    /// </summary>

    public class GameState
    {
        #region Private Variables    
        
        private CamZoom camZoom;
        private UIManager uiManager;
        private ObjectFollow camFollow;
        private LevelParameters parameters;
        private GameManager gameManager;
        private BlinkManager blinkManager;
        private bool hasKilledMainTarget = false;
        private bool isMarking = false;
        private List<Entity> levelEntities = new List<Entity>();
        private List<Props> registeredProps = new List<Props>();
        private List<Animator> killedAnimators = new List<Animator>();
        private bool finalSaluteDone = false;
        private bool propsAllHarmless = false;

        #endregion

        #region Private Methods

        #endregion

        #region Public Methods

        /// <summary>
        /// Initialize severals attributes without creating a new instance of GameState.
        /// </summary>
        /// <param name="param">The level parameters, they won't change during play time.</param>
        /// <param name="cam">Allow several camera effects.</param>
        /// <param name="manager">Reference to the GameManager.</param>
        /// <param name="input">Input Type. 0 if Mouse, 1 if touch. </param>
        public void Initialize(LevelParameters param, CamZoom cam, GameManager manager, ObjectFollow camFoll, UIManager ui, BlinkManager blinkMng)
        {
            CamFollow = camFoll;
            parameters = param;
            camZoom = cam;
            gameManager = manager;
            UiManager = ui;
            BlinkManager = blinkMng;
            Debug.Log("Game state successfully referenced level " + manager.name + " has manager ");
        }

        public void RegisterProp(Props prop)
        {
            registeredProps.Add(prop);
        }

        public void UnregisterProp(Props prop)
        {
            registeredProps.Remove(prop);
        }

        /// <summary>
        /// Register an entity into the list of entities that listen to the Enity class callbacks.
        /// </summary>
        /// <param name="entity">The enity to register.</param>
        public void RegisterEntity(Entity entity)
        {
            levelEntities.Add(entity);
        }

        /// <summary>
        /// Unregister an entity to the list of entities that listen to the Entity class callbacks
        /// </summary>
        /// <param name="entity">The entity to unregister.</param>
        public void UnregisterEntity(Entity entity)
        {
            levelEntities.Remove(entity);
        }

        public void CheckForEnd()
        {
            if (!propsAllHarmless)
                CheckForAllLethalProps();
            
            bool mainObj = Manager.WinConditions.MainObjectiveChecked();
            bool sideObj = Manager.WinConditions.SideObjectiveChecked();
            bool dashLimit = Manager.WinConditions.DashLimitChecked();

            if (finalSaluteDone && propsAllHarmless)
            {
                CallLevelEnd(mainObj, sideObj, dashLimit);
            }
        }

        public void CheckForAllLethalProps()
        {
            for (int i = 0; i < registeredProps.Count; i++)
            {
                if(registeredProps[i].ShouldFall)
                {
                    Debug.Log(registeredProps[i].name + " is lethal");
                    return;
                }
            }
            propsAllHarmless = true;

        }

        /// <summary>
        /// Play the death/destruction animation of all the object sliced during ghost strike
        /// </summary>
        public void PlayAnimatorsDeathAnim(bool gotToElevator)
        {
            //gameManager.EnemyDeathAnims(killedAnimators);
            for (int i = 0; i < killedAnimators.Count; i++)
            {
                killedAnimators[i].SetBool("isDead", true);
                killedAnimators[i].speed = Random.Range(0.75f, 1.25f);
            }
            killedAnimators.Clear();
            
            if(gotToElevator)
            {
                Debug.Log("Player got back to the elevator");
                GameManager.gameState.FinalSaluteDone = true;
                GameManager.gameState.CheckForEnd();
            }
            else
            {
                Debug.Log("Player didn't finished on the elevator");
                uiManager.NeedElevator();
            }
        }

        /// <summary>
        /// Call the LevelStart method on all registered entities.
        /// </summary>
        public void CallLevelStart()
        {

            for (int i = 0; i < levelEntities.Count; i++)
            {
                levelEntities[i].LevelStart();
            }
        }

        public void CallElevatorDing()
        {
            for (int i = 0; i < levelEntities.Count; i++)
            {
                levelEntities[i].ElevatorDing();
            }
        }

        /// <summary>
        /// Call the LevelEnd method on all registered entities.
        /// </summary>
        public void CallLevelEnd(bool mainObj, bool sideObj, bool dashLimit)
        {
            for (int i = 0; i < levelEntities.Count; i++)
            {
                levelEntities[i].LevelEnd(mainObj, sideObj, dashLimit);
            }
            Debug.Log("Level " + gameManager.gameObject.name + " ended");
        }

        /// <summary>
        /// Call the TargetPlaced method on all registered entities.
        /// </summary>
        public void CallTargetPlaced()
        {
            for (int i = 0; i < levelEntities.Count; i++)
            {
                levelEntities[i].TargetPlaced();
            }
        }

        /// <summary>
        /// Call the EnemyHit method on all registered entities.
        /// </summary>
        public void CallEnemyHit()
        {
            for (int i = 0; i < levelEntities.Count; i++)
            {
                levelEntities[i].EnemyHit();
            }
        }

        /// <summary>
        /// Call the EnemyKilled method on all registered entities.
        /// </summary>
        public void CallEnemyKilled()
        {
            for (int i = 0; i < levelEntities.Count; i++)
            {
                levelEntities[i].EnemyKilled();
            }
        }

        /// <summary>
        /// Call the CollectibleObtained method on all registered entities.
        /// </summary>
        public void CallCollectibleObtained()
        {
            for (int i = 0; i < levelEntities.Count; i++)
            {
                levelEntities[i].CollectibleObtained();
            }
        }

        public void CallGhostStrikePreparing()
        {
            for (int i = 0; i < levelEntities.Count; i++)
            {
                levelEntities[i].GhostStrikePreparing();
            }
        }

        public void CallGhostStrikeStart()
        {
            killedAnimators.Clear();
            for (int i = 0; i < levelEntities.Count; i++)
            {
                levelEntities[i].GhostStrikeStart();
            }
        }

        public void CallGhostStrikeEndDash()
        {
            for (int i = 0; i < levelEntities.Count; i++)
            {
                levelEntities[i].GhostStrikeEndDash();
            }
        }

        public void CallGhostStrikeStartDash()
        {
            for (int i = 0; i < levelEntities.Count; i++)
            {
                levelEntities[i].GhostStrikeStartDash();
            }
        }

        public void CallGhostStrikeEnd()
        {
            for (int i = 0; i < levelEntities.Count; i++)
            {
                levelEntities[i].GhostStrikeEnd();
            }
        }

        public void CallPlayerFinalSalute()
        {
            for (int i = 0; i < levelEntities.Count; i++)
            {
                levelEntities[i].PlayerFinalSalute();
            }
        }

        public void CallPlayerDetected(Vector3 detectionPosition)
        {
            for (int i = 0; i < levelEntities.Count; i++)
            {
                levelEntities[i].PlayerDetected(detectionPosition);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Has the player finished his ghoststrike?
        /// </summary>
        public bool FinalSaluteDone
        {
            get
            {
                return finalSaluteDone;
            }

            set
            {
                finalSaluteDone = value;
            }
        }

        /// <summary>
        /// Contains the list of animator which's gameObject have been touched by the player during ghost strike
        /// </summary>
        public List<Animator> KilledAnimators
        {
            get
            {
                return killedAnimators;
            }
        }

        /// <summary>
        /// Contains zoom effects methods for Main Camera.
        /// </summary>
        public CamZoom CamZoom
        {
            get
            {
                return camZoom;
            }
        }

        /// <summary>
        /// Access several information about the parameter of the current level.
        /// </summary>
        public LevelParameters Parameters
        {
            get
            {
                return parameters;
            }
        }

        /// <summary>
        /// Has the collectible of the level been acquired by the player?
        /// </summary>
        public bool HasKilledMainTarget
        {
            get
            {
                return hasKilledMainTarget;
            }

            set
            {
                hasKilledMainTarget = value;
            }
        }

        /// <summary>
        /// Is the player currently placing his marks?
        /// </summary>
        public bool IsMarking
        {
            get
            {
                return isMarking;
            }

            set
            {
                isMarking = value;
            }
        }

        /// <summary>
        /// Camera Following player for detection effect.
        /// </summary>
        public ObjectFollow CamFollow
        {
            get
            {
                return camFollow;
            }

            set
            {
                camFollow = value;
            }
        }

        public UIManager UiManager { get => uiManager; set => uiManager = value; }

        public GameManager Manager { get => gameManager;  }
        public BlinkManager BlinkManager { get => blinkManager; set => blinkManager = value; }

        #endregion
    }
}
