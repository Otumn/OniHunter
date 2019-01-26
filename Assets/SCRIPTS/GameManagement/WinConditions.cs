using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    public class WinConditions : Entity
    {
        public enum SideObjective { Kill, Assassinate, Loot}

        #region Serialized field

        [SerializeField] private Enemy mainTarget;
        [SerializeField] private SideObjective sideObjective;

        #endregion

        #region Hidden fields

        [SerializeField] [HideInInspector] private List<Enemy> allEnemies = new List<Enemy>();
        private int countedDashs = 0;

        #endregion

        #region Entity callbacks

        public override void TargetPlaced()
        {
            countedDashs++;
        }

        #endregion

        #region Public Methods

        public bool MainObjectiveChecked()
        {
            if(mainTarget == null)
            {
                Debug.Log("No main target referenced. Main win condition false by default");
                return false;
            }

            if(mainTarget.IsDead)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SideObjectiveChecked()
        {
            if(sideObjective == SideObjective.Kill)
            {
                bool enemyAlive = false;
                for (int i = 0; i < AllEnemies.Count; i++)
                {
                    if(!AllEnemies[i].IsDead)
                    {
                        enemyAlive = true;
                        break;
                    }
                }
                return !enemyAlive;
            }
            else if(sideObjective == SideObjective.Assassinate)
            {
                return false;
            }
            else if(sideObjective == SideObjective.Loot)
            {
                return false;
            }
            return false;
        }

        public bool DashLimitChecked()
        {
            if(countedDashs > GameManager.gameState.Parameters.DashObjective)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void RegisterEnemy(Enemy enemy)
        {
            allEnemies.Add(enemy);
        }

        public void UnregisterEnemy(Enemy enemy)
        {
            allEnemies.Remove(enemy);
        }

        #endregion

        #region Properties

        public List<Enemy> AllEnemies { get => allEnemies; }

        #endregion

    }
}
