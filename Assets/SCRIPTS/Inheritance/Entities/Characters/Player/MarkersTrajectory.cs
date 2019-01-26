using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    public class MarkersTrajectory : MonoBehaviour
    {
        #region Monobehaviour callbacks

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponentInParent<IDamageListener>() != null)
            {
                //collision.GetComponentInParent<IDamageListener>().ShowInteraction();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.GetComponentInParent<IDamageListener>() != null)
            {
                //collision.GetComponentInParent<IDamageListener>().HideInteraction();
            }
        }

        #endregion
    }
}
