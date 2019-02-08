using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Pagann.OniHunter
{
    public class UiInputBlocker : Entity
    {
        /// <summary>
        /// Returns true if the touch/click/... at position pos is on top of a UI element.
        /// </summary>
        public static bool IsInputUI(Vector3 pos, GraphicRaycaster[] raycasters, EventSystem eventSystem)
        {
            PointerEventData pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = pos;
            List<RaycastResult> results = new List<RaycastResult>();
            for (int i = 0; i < raycasters.Length; i++)
            {
                raycasters[i].Raycast(pointerEventData, results);
            }
            for (int i = 0; i < results.Count; i++)
            {
                Debug.Log(results[i].gameObject.name + " was hit by input");
            }
            if (results.Count > 0) return true;
            else return false;
        }
    }
}
