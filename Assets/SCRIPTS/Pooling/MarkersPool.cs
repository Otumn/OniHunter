using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    public class MarkersPool : MonoBehaviour
    {
        [SerializeField] private List<Marker> markersObject = new List<Marker>();

        private void Start()
        {
            for (int i = 0; i < markersObject.Count; i++)
            {
                markersObject[i].gameObject.SetActive(false);
            }
        }

        public Marker TargetToPool()
        {
            for (int i = 0; i < markersObject.Count; i++)
            {
                if (!markersObject[i].gameObject.activeInHierarchy)
                {
                    return markersObject[i];
                }
            }

            return null;
        }

        public void DeactivateAll()
        {
            for (int i = 0; i < markersObject.Count; i++)
            {
                if (markersObject[i].gameObject.activeInHierarchy)
                {
                    markersObject[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
