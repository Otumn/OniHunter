using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pagann.OniHunter
{
    /// <summary>
    /// Handles the ui related with gameplay features.
    /// </summary>
    public class GameplayUIManager : Entity
    {
        [SerializeField] private Player player;
        [Header("UI")]
        [SerializeField] private GraphicRaycaster[] raycasters;
        [Header("Animations")]
        [SerializeField] private Animator strikingAnim;

        public bool CheckInputUi(Vector3 pos)
        {
            return LevelsManager.state.UImanager.CheckUiInput(pos, raycasters);
        }

        public override void GhostStrikePreparing()
        {
            base.GhostStrikePreparing();
            strikingAnim.SetBool("Prepare", true);
        }

        public override void GhostStrikeStart()
        {
            base.GhostStrikeStart();
            strikingAnim.SetBool("Prepare", false);
        }

        public void SendInitiateOrStartStrike()
        {
            player.InitiateOrStartStrike();
        }
    }
}
