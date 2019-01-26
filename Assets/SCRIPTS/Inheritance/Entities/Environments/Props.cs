using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Props : Environment
    {
        #region Private Variables

        [Header("Components")]
        [SerializeField] private Rigidbody2D body;
        [Header("Props Parameters")]
        [SerializeField] private bool shouldCollideWithPlayer = false;
        [Tooltip("Will the prop be movable by exterior forces at start?")]
        [SerializeField] private bool staticOnStart = false;
        [Tooltip("Velocity at which this prop will damage damaging listening entities upon collision")]
        [SerializeField] private float lethalVelocity = 1f;

        [SerializeField] private float maxGravityForce = 7f;
        [SerializeField] private AnimationCurve gravityCurve;

        private float usedGravityForce;
        private float incrementer = 0f;

        private bool movableNextDash = false;
        private bool shouldFall = false;

        #endregion

        #region Monobehaviour Callbacks

        protected override void HeritedStart()
        {
            body.isKinematic = true;
            body.gravityScale = 0;
        }

        protected override void HeritedUpdate()
        {
            base.HeritedUpdate();
            if (shouldFall)
            {
                body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            }
            else
            {
                body.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
            }
            GravityCurveEvaluation();
        }

        private void FixedUpdate()
        {
            Fall();
        }

        protected override void HeritedOnEnable()
        {
            GameManager.gameState.RegisterProp(this);
        }

        protected override void HeritedOnDisable()
        {
            GameManager.gameState.RegisterProp(this);
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (coll.GetComponentInParent<IDamageListener>() != null && coll.GetComponentInParent<Entity>() != null && this.shouldFall)
            {
                Debug.Log(gameObject.name + " killed " + coll.gameObject.name);
                coll.GetComponentInParent<IDamageListener>().DieInstantly(true);
            }

            if(!shouldCollideWithPlayer)
            {
                if (coll.GetComponentInParent<IDamageListener>() != null && coll.GetComponentInParent<Entity>() != null && this.shouldFall)
                {
                    Debug.Log(gameObject.name + " killed " + coll.gameObject.name);
                    coll.GetComponentInParent<IDamageListener>().DieInstantly(true);
                }

                if (coll.gameObject.tag == "Platform")
                {
                    body.isKinematic = true;
                    shouldFall = false;
                    GameManager.gameState.CheckForAllLethalProps();
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D coll)
        {
            if(shouldCollideWithPlayer)
            {
                if (coll.collider.GetComponentInParent<IDamageListener>() != null && coll.collider.GetComponentInParent<Entity>() != null && this.shouldFall)
                {
                    Debug.Log(gameObject.name + " killed " + coll.gameObject.name);
                    coll.collider.GetComponentInParent<IDamageListener>().DieInstantly(true);
                }

                if (coll.gameObject.tag == "Platform")
                {
                    body.isKinematic = true;
                    shouldFall = false;
                    GameManager.gameState.CheckForAllLethalProps();
                }
            }
        }

        #endregion

        #region Private Methods

        private void Fall()
        {
            if (shouldFall)
            {
                body.MovePosition(transform.position + (Vector3.down * usedGravityForce * Time.deltaTime));
            }
        }

        private void GravityCurveEvaluation()
        {
            if(shouldFall)
            {
                usedGravityForce = gravityCurve.Evaluate(incrementer) * maxGravityForce;
                incrementer += Time.deltaTime;
                if (incrementer > 1f)
                {
                    incrementer = 1f;
                }
            }
        }

        private void MovableNextDash()
        {
            body.isKinematic = false;
            shouldFall = true;
        }

        #endregion

        #region Public Methods

        [ContextMenu("Manually set movable")]
        public void SetMovable()
        {
            MovableNextDash();
        }

        #endregion

        #region Accessors

        public bool ShouldFall
        {
            get
            {
                return shouldFall;
            }
        }

        public Rigidbody2D Body
        {
            get
            {
                return body;
            }
        }

        #endregion
    }
}
