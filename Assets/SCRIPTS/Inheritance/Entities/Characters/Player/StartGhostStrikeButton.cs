using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    public class StartGhostStrikeButton : MonoBehaviour
    {
        [SerializeField] private Player player;

        public void Strike()
        {
            if(player.CanStrike && !player.IsDetected)
            {
                player.StartGhostStrike();
            }
        }
    }
}
