using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    public class DashTrailsManager : MonoBehaviour
    {
        [SerializeField] private int currentlyEquippedTrail = 0;
        [Space(10)]
        [SerializeField] private DashTrailFX[] trails;

        private void Update()
        {
            UpdateTrailEquipped();
        }
        
        private void UpdateTrailEquipped()
        {
            for (int i = 0; i < trails.Length; i++)
            {
                if (i == currentlyEquippedTrail)
                {
                    trails[i].gameObject.SetActive(true);
                }

                else
                {
                    trails[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
