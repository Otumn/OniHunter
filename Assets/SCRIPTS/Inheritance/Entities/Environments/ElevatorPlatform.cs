using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    public class ElevatorPlatform : Entity
    {
        private bool isWaitingForPlayer = false;
        private bool playerLandedOn = false;

        public override void GhostStrikePreparing()
        {
            isWaitingForPlayer = true;
        }

        public bool PlayerLandedOn { get => playerLandedOn; set => playerLandedOn = value; }
        public bool IsWaitingForPlayer { get => isWaitingForPlayer; }
    }
}
