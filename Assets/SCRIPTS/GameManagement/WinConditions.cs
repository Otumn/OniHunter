using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    public class WinConditions : Entity
    {
        public enum SideObjective { KillAll, Assassinate, BeginWith, EndWith}

        #region Serialized field

        [SerializeField] private SideObjective objective;
        [SerializeField] private Enemy relatedTarget;

        #endregion

        #region Hidden fields

        [SerializeField] private List<Enemy> allEnemies = new List<Enemy>();
        private List<Enemy> killedEnemies = new List<Enemy>();

        #endregion

        #region Public Methods

        public bool ObjectiveChecked()
        {
            if (relatedTarget == null && objective != SideObjective.KillAll)
            {
                Debug.Log("No main target referenced. Win condition false by default");
                return false;
            }

            if (objective == SideObjective.Assassinate)
            {
                Debug.Log("Assassinate objective tested");
                if (killedEnemies.Count == 1)
                {
                    if (killedEnemies[0] == relatedTarget) return true;
                    else return false;
                }
                else return false;
            }
            else if(objective == SideObjective.KillAll)
            {
                Debug.Log("KillAll objective tested");
                for (int i = 0; i < allEnemies.Count; i++)
                {
                    if (allEnemies[i].IsDead) continue;
                    else return false;
                }
                return true;
            }
            else if(objective == SideObjective.BeginWith)
            {
                Debug.Log("BeginWith objective tested");
                for (int i = 0; i < allEnemies.Count; i++)
                {
                    if (allEnemies[i].IsDead) continue;
                    else return false;
                }
                if (killedEnemies.Count > 0)
                {
                    if (killedEnemies[0] == relatedTarget) return true;
                    else return false;
                }
            }
            else if(objective == SideObjective.EndWith)
            {
                Debug.Log("EndWith objective tested");
                for (int i = 0; i < allEnemies.Count; i++)
                {
                    if (allEnemies[i].IsDead) continue;
                    else return false;
                }
                if (killedEnemies.Count > 0)
                {
                    if (killedEnemies[killedEnemies.Count - 1] == relatedTarget) return true;
                    else return false;
                }
            }
            return false;
        }

        public void RegisterKilledEnemy(Enemy killedEnemy)
        {
            killedEnemies.Add(killedEnemy);
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
