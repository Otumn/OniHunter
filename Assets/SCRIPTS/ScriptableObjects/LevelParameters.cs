using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    [CreateAssetMenu(fileName = "LevelParameters", menuName = "OniHunter/Level Parameters")]
    public class LevelParameters : ScriptableObject
    {
        [SerializeField] private int maximumDashs = 5;
        [SerializeField] private int dashObjective = 3;

        /// <summary>
        /// Number of dashes the player can do in this level.
        /// </summary>
        public int MaximumDashs
        {
            get
            {
                return maximumDashs;
            }
        }

        /// <summary>
        /// The maximum number of dashs the level has to be completed to earn a reward.
        /// </summary>
        public int DashObjective { get => dashObjective; }
    }
}
