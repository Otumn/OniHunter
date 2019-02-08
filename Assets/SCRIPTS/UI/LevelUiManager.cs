using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Pagann.OniHunter
{
    /// <summary>
    /// Handles the ui related with level/scene management features
    /// </summary>
    public class LevelUiManager : Entity
    {
        [Header("Gameplay Input Checking")]
        [SerializeField] private EventSystem eventSystem;
        [SerializeField] private GraphicRaycaster[] raycasters;
        private PointerEventData pointerEventData;
        [Header("UI Animations")]
        [SerializeField] private Animator detectAnim;
        [SerializeField] private Animator winAnim;
        [SerializeField] private Animator elevatorAnim;
        [SerializeField] private Animator incompleteAnim;

        public bool CheckUiInput(Vector3 pos, GraphicRaycaster[] addedRaycasters)
        {
            List<GraphicRaycaster> fullRaycasters = new List<GraphicRaycaster>();
            for (int i = 0; i < raycasters.Length; i++)
            {
                fullRaycasters.Add(raycasters[i]);
            }
            for (int i = 0; i < addedRaycasters.Length; i++)
            {
                fullRaycasters.Add(raycasters[i]);
            }
            return UiInputBlocker.IsInputUI(pos, fullRaycasters.ToArray(), eventSystem);
        }

        #region Entity Callbacks

        public override void LevelEnd(bool mainObj, bool sideObj, bool dashLimit)
        {
            Debug.Log("Ended. Main : " + mainObj + ", Side : " + sideObj + ", Dashs : " + dashLimit);
            if (sideObj)
            {
                winAnim.SetBool("Win", true);
            }
            else
            {
                incompleteAnim.SetBool("NeedElevator", true);
            }
        }

        public override void LevelStart()
        {
            base.LevelStart();
        }

        #endregion

        #region UI Animations

        public void Hide()
        {
            detectAnim.SetBool("Detected", false);
            winAnim.SetBool("Win", false);
            elevatorAnim.SetBool("NeedElevator", false);
            incompleteAnim.SetBool("NeedElevator", false);
        }

        public void Detected()
        {
            detectAnim.SetBool("Detected", true);
        }

        public void NeedElevator()
        {
            elevatorAnim.SetBool("NeedElevator", true);
        }

        public void Incomplete()
        {
            //currently using the same animator than elevatorAnim. This is a placeHolder.
            incompleteAnim.SetBool("NeedElevator", true);
        }

        #endregion
    }
}
