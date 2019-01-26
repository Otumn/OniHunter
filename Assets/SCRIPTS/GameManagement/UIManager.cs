using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Pagann.OniHunter
{
    public class UIManager : Entity
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

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                Debug.Log(IsInputUI(Input.mousePosition));
            }
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

        #region Gameplay input checking

        /// <summary>
        /// Returns true if the touch/click/... at position pos is on top of a UI element.
        /// </summary>
        /// <param name="pos">Position of the tested input in pixel coordinate.</param>
        /// <returns></returns>
        public bool IsInputUI(Vector3 pos)
        {
            pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = pos;
            List<RaycastResult> results = new List<RaycastResult>();
            for (int i = 0; i < raycasters.Length; i++)
            {
                raycasters[i].Raycast(pointerEventData, results);
            }
            for (int i = 0; i < results.Count; i++)
            {
                Debug.Log(results[i].gameObject.name + " was hit");
            }
            if (results.Count > 0) return true;
            else return false;
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
