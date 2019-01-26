using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    public class LineOfSight : Entity, IDamageListener
    {
        [SerializeField] private Transform playerPosOnDetection;
        [SerializeField] private GameObject interactionFeedBack;

        public Transform PlayerPosOnDetection { get => playerPosOnDetection; set => playerPosOnDetection = value; }
        
        public void DamageSelf()
        {

        }

        public void DieInstantly(bool crushed)
        {

        }

        public void HideInteraction()
        {
            interactionFeedBack.SetActive(false);
        }

        public void ShowInteraction()
        {
            interactionFeedBack.SetActive(true);
        }
    }
}
