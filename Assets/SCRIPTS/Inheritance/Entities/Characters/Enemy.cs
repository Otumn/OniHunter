    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    public class Enemy : Character, IDamageListener, IDetectionListener
    {
        [Header("Components")]
        [SerializeField] private Rigidbody2D body;
        [SerializeField] private Collider2D hitbox;
        [SerializeField] private Animator anim;
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private GameObject collisionFeedback;
        [SerializeField] private EnemyFX fx;
        [SerializeField] private Blink blink;
        [Space(10)]
        [SerializeField] private List<Transform> targets = new List<Transform>();
        [SerializeField] private GameObject targetsParent;
        [SerializeField] private Transform targetsReference;

        [Header("Health")]
        [SerializeField] private float healthPoints = 1f;

        [Header("Movement")]
        [Tooltip("Will enemy move or not?")]
        [SerializeField] private bool isMobile = true;
        [Tooltip("Does the enemy needs to be on a platform to move?")]
        [SerializeField] private bool isWalker = true;

        private enum MoveSpeedType { ConstantSpeed, ConstantTime}
        [Tooltip("Constant Speed : The enemy has the same speed, no matter the distance. Constant Time : The time to reach the target is constant, no matter the distance.")]
        [SerializeField] private MoveSpeedType moveSpeedType;

        [Tooltip("Does the enemy only move horizontally?")]
        [SerializeField] private bool isGrounded = true;

        [Tooltip("Will enemy spawn on his first Target?")]
        [SerializeField] private bool startOnFirstTarget = false;

        [Tooltip("Which target of the list will the enemy reach first? (0-based!)")]        
        [SerializeField] private int firstTargetIndex = 0;

        [Tooltip("How long will enemy wait when reaching a target before moving again?")]
        [SerializeField] private float waitTime = 1f;

        [Tooltip("Speed of the enemy based on his current position between his starting point and his destination.")]
        [SerializeField] private AnimationCurve progressCurve;

        [Tooltip("Speed multiplier")]
        [SerializeField] private float moveSpeed = 1f;

        private bool hasDetectedPlayer = false;
        private bool canMove = true;
        private bool isDead = false;
        private int currentTargetIndex = 0;
        private float moveCompletion = 0f;
        private Vector3 startingPoint;
        private Vector3 currentTarget = new Vector3(5,8,9);
        private Vector3 currentVel;
        private Coroutine waitRoutine;
        private Vector3 dirBetweenTargetAndRef;

        protected override void HeritedStart()
        {
            base.HeritedStart();
            if(targetsReference != null)
            {
                dirBetweenTargetAndRef = (targetsParent.transform.position - targetsReference.position);
            }
        }

        protected override void HeritedUpdate()
        {
            base.HeritedUpdate();
            HealthManager();
            if (isMobile)
            {
                Movement();
            }
            if (targetsReference != null)
            {
                targetsParent.transform.position = targetsReference.position + dirBetweenTargetAndRef;
            }
            if (isGrounded) transform.position = new Vector3(transform.position.x, currentTarget.y, 0);
            if (targets.Count > 0) currentTarget = targets[currentTargetIndex].position;
            SpriteFlip();
            Animations();
        }

        protected override void HeritedOnEnable()
        {
            
        }

        public override void LevelStart()
        {
            GameManager.gameState.Manager.WinConditions.RegisterEnemy(this);
            currentTargetIndex = firstTargetIndex;

            startingPoint = isGrounded ?
                    new Vector3(targets[1-currentTargetIndex].position.x, transform.position.y, transform.position.z) :
                    targets[1-currentTargetIndex].position;

            currentTarget = targets[currentTargetIndex].position;

            if (startOnFirstTarget)
            {                    
                body.MovePosition (startingPoint);
                moveCompletion = 1f;
            }

            else
            {
                moveCompletion = Mathf.InverseLerp(startingPoint.x, currentTarget.x, transform.position.x);
            }                        
        }

        #region Movement
        /// <summary>
        /// Manages Enemy movement. Make them translate from their current target to the next one at a speed defined by the Progress Animation Curve.
        /// </summary>
        private void Movement()
        {
            if (CanMove && !isDead && !HasDetectedPlayer)
            {
                body.MovePosition((isGrounded ?
                    new Vector3(Mathf.Lerp(startingPoint.x, currentTarget.x, progressCurve.Evaluate(moveCompletion)), transform.position.y, transform.position.z) :
                    Vector3.Lerp(startingPoint, currentTarget, progressCurve.Evaluate(moveCompletion))));

                switch (moveSpeedType)
                {
                    case MoveSpeedType.ConstantSpeed:
                        moveCompletion += ((moveSpeed / (Vector3.Distance(startingPoint, currentTarget)) * Time.deltaTime));
                        break;

                    case MoveSpeedType.ConstantTime:
                        moveCompletion += (moveSpeed * Time.deltaTime);
                        break;
                }

                if (moveCompletion >= 1)
                {
                    if (!HasDetectedPlayer)
                    {
                        ReachTarget();
                    }
                }
            }
        }
                                                 
        /// <summary>
        /// Called when reaching a target.
        /// </summary>                                    
        private void ReachTarget()
        {
            moveCompletion = 0f;                                                                                    
            Anim.SetTrigger("Idle");
            body.MovePosition(isGrounded ?
                new Vector3(currentTarget.x, transform.position.y, transform.position.z) :
                currentTarget);
            startingPoint = transform.position;
            CanMove = false;
            WaitRoutine = StartCoroutine(WaitTime());                                                                           
        }

        /// <summary>
        /// Delay before moving again.
        /// </summary>
        private IEnumerator WaitTime()
        {
            yield return new WaitForSeconds(waitTime);
            if (currentTargetIndex == targets.Count - 1)
            {
                currentTargetIndex = 0;
            }

            else
            {
                currentTargetIndex++;
            }
            yield return new WaitForSeconds(0.02f);
            CanMove = true;
        }
        #endregion

        #region Detection
        public void DetectedPlayer()
        {
            CanMove = false;
            HasDetectedPlayer = true;
            Anim.SetTrigger("Detect");
            if(waitRoutine != null)
                StopCoroutine(WaitRoutine);
        }
        #endregion

        #region Health

        public virtual void DieInstantly(bool crushed)
        {
            Death();
            if(crushed)
            {
                Anim.SetBool("isCrushed", true);
                fx.PlayCrushedFX();
            }

            else
            {
                Anim.SetBool("isDead", isDead);
            }
        }

        public void Death()
        {
            isDead = true;
            canMove = false;
            HideInteraction();
            hitbox.enabled = false;
            GameManager.gameState.CallEnemyKilled();
            GameManager.gameState.CamZoom.TriggerCamZoom(0);
            Debug.Log(gameObject.name + " died instantly");
        }

        public void DamageSelf()
        {
            if(healthPoints > 0)
            {
                //Debug.Log(gameObject.name + " was damaged");
                blink.Blinks(GameManager.gameState.BlinkManager.BlinkNumber, GameManager.gameState.BlinkManager.BlinkDelay, GameManager.gameState.BlinkManager.BlinkTime);
                hitbox.enabled = false; //Avoiding collision feedbacks on dead enemies.
                healthPoints -= 1;
                canMove = false;
                hitbox.enabled = false;
                GameManager.gameState.KilledAnimators.Add(anim);
                GameManager.gameState.CallEnemyHit();
                GameManager.gameState.CamZoom.TriggerCamZoom(0);
            }
        }

        public void ShowInteraction()
        {
            collisionFeedback.SetActive(true);
        }

        public void HideInteraction()
        {
            collisionFeedback.SetActive(false);
        }

        private void HealthManager()
        {
            if (healthPoints <= 0  && !isDead)
            {
                Kill();
            }
        }

        private void Kill()
        {
            //Feedbacks Kill 
            isDead = true;
            Anim.SetTrigger("Freeze");
            HideInteraction();
            GameManager.gameState.CallEnemyKilled();
        }

        #endregion

        #region Sprite

        private void SpriteFlip()
        {
            if (currentTarget.x > transform.position.x)
            {
                Sprite.flipX = true;
            }

            if (currentTarget.x < transform.position.x)
            {
                Sprite.flipX = false;
            }
        }

        private void Animations()
        {
            if(isMobile)
            {
                Anim.SetBool("isWalking", CanMove);
            }
        }

        #endregion

        #region Properties

        public float HealthPoints
        {
            get
            {
                return healthPoints;
            }

            set
            {
                healthPoints = value;
            }
        }

        public SpriteRenderer Sprite
        {
            get
            {
                return sprite;
            }

            set
            {
                sprite = value;
            }
        }

        public bool CanMove
        {
            get
            {
                return canMove;
            }

            set
            {
                canMove = value;
            }
        }

        public Animator Anim
        {
            get
            {
                return anim;
            }

            set
            {
                anim = value;
            }
        }

        public bool HasDetectedPlayer { get => hasDetectedPlayer; set => hasDetectedPlayer = value; }

        public Coroutine WaitRoutine { get => waitRoutine; set => waitRoutine = value; }

        public bool IsDead { get => isDead; }

        #endregion
    }
}