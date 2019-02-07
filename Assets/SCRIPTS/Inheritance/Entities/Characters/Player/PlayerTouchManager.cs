using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    /// <summary>
    /// This class manage the placement of the collider responsible for the initiation and the start of the strike.
    /// </summary>
    public class PlayerTouchManager : Entity
    {
        #region Serialized fields

        [SerializeField] private Player player;
        [SerializeField] private LayerMask mask;
        [SerializeField] private GameObject target;
        [SerializeField] private CircleCollider2D touchCollider;
        [SerializeField] private float targetRadius = 0.5f; // 0 because WIP with the new UI strike buttons

        #endregion

        #region Hidden fields

        private RaycastHit2D hitOut;
        

        #endregion

        #region Monobehaviour Callbacks

        protected override void HeritedStart()
        {
            base.HeritedStart();
        }

        private void Update()
        {
            if (Input.touchCount > 0 && Input.GetTouch(Input.touchCount - 1).phase == TouchPhase.Began)
            {
                Ray lastTouchRay = Camera.main.ScreenPointToRay(Input.GetTouch(Input.touchCount - 1).position);
                hitOut = Physics2D.Raycast(lastTouchRay.origin, lastTouchRay.direction, 15, mask);
            }
            if(Input.GetMouseButtonDown(0))
            {
                Ray lastTouchRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                hitOut = Physics2D.Raycast(lastTouchRay.origin, lastTouchRay.direction, 15, mask);
            }
            if(hitOut.collider != null)
            {
                player.InitiateOrStartStrike();
                hitOut = new RaycastHit2D();
            }
        }

        #endregion

        public override void ElevatorDing()
        {
            target.SetActive(true);
        }

        public override void GhostStrikePreparing()
        {
            touchCollider.radius = targetRadius; // line from the time when the strike had to be launch by touching the last target we placed
        }

        #region Public Methods

        public void ReplaceTarget()
        {
            target.transform.position = player.GetLastMarker.transform.position;
        }

        #endregion

        #region Properties

        public static bool GetOnlyMouseButton(int index)
        {
            return (Input.GetMouseButton(index) && Input.touchCount == 0);
        }

        public static bool GetOnlyMouseButtonDown(int index)
        {
            return (Input.GetMouseButtonDown(index) && Input.touchCount == 0);
        }

        public static bool GetOnlyMouseButtonUp(int index)
        {
            return (Input.GetMouseButtonUp(index) && Input.touchCount == 0);
        }

        #endregion
    }
}
