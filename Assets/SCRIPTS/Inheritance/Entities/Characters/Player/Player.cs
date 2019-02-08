using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    public class Player : Character
    {
        #region Serialized Fields

        [Header("Components")]
        [SerializeField] private Rigidbody2D body;
        [SerializeField] private Animator anim;
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private MarkersPool markersPool;
        [SerializeField] private PlayerFXs fx;
        [SerializeField] private BulletTime bulletTime;
        [SerializeField] private CapsuleCollider2D hitboxTrigger;
        [SerializeField] private BoxCollider2D hitboxSolid;
        [SerializeField] private LineRenderer lineDash;
        [SerializeField] private PlayerTouchManager touchManager;
        [SerializeField] private ElevatorPlatform elevator;
        [SerializeField] private Transform elevatorPos;
        [SerializeField] private GameplayUIManager gameplayUIManager;

        [Header("Mark placement")]
        [Space(10)]
        [SerializeField] private float minimumDistanceBetweenMarks = 0.5f;

        [Header("Ghost Strike")]
        [Space(10)]
        [Header("Striking")]
        [SerializeField] private float hitboxDistance = -0.7f;
        [Tooltip("Dash speed.")]
        [SerializeField] private float dashSpeed = 0.1f;

        [Tooltip("Delay between two dashes.")]
        [SerializeField] private float strikeDelay = 0.1f;
        [SerializeField] private float gravityScale = 2f;
        [SerializeField] private AnimationCurve curveDash;
        [SerializeField] private float dashCompletion = 0f;

        private enum MoveSpeedType { ConstantSpeed, ConstantTime }
        [Tooltip("Constant Speed : The player has the same speed, no matter the distance. Constant Time : The time to reach the target is constant, no matter the distance.")]
        [SerializeField] private MoveSpeedType moveSpeedType;

        [Header("Slow-Mo")]
        [Tooltip("Time Scale value when casting Ghost Strike.")]
        [SerializeField] private float ghostTimeScale = 0.2f;
        [Tooltip("How fast will Ghost Strike Slow-Mo reach its targetted timeScale value?")]
        [SerializeField] private float ghostSpeedSlowMo = 0.02f;

        [Header("Initial Jump")]
        [SerializeField] private AnimationCurve initialJumpCurve;
        [SerializeField] private float initialJumpCompletionSpeed = 0.1f;
        [SerializeField] private float initialJumpHeight = 1f;

        #endregion

        #region Hidden Fields

        private List<Vector3> targets = new List<Vector3>();
        private int maxTargetNumbers = 5;
        private List<Marker> markers = new List<Marker>();
        private Marker playerMarker;
        private bool isPreparing = false; //True when the player enters Ghost Strike mode and places his markers. Goes back to false when starting the Strike.
        private bool markerPlaced = false;
        private bool canReceiveInputsAgain = true; //We use the same input for triggering the ghost strike and placing markers. We need a coroutine to avoid placing the 1st marker at the second we start ghost strike.
        private bool isDashing = false;
        private bool isGhostStriking = false;
        private bool isGrounded = false;
        private bool canStrike = false;
        private bool canInitiateStrike = true;
        private bool isInitialJumping = false;
        private bool shouldSalute = false;
        private bool isDetected = false;
        private bool hasRaisedFinger = true;
        private bool hasReleasedFirstFinger = false; // To avoid errors if the user holds his finger too long when initiating ghost strike.
        private bool shouldGetBackToElevator = false;
        private bool lookingForElevator = false;

        private float initialJumpCompletion = 0f;
        private float targetCapsuleYSize = 1.7f;
        private int currentTarget = 0;

        private Marker currentMarker;
        private Vector3 initialJumpStartPosition;
        private Vector3 dashStartPos;
        #endregion

        #region MonoBehaviour Callbacks

        protected override void HeritedStart()// START
        {
            base.HeritedStart();
            body.gravityScale = gravityScale;
            Time.timeScale = 1f;
            lineDash.gameObject.SetActive(false);
            targetCapsuleYSize = hitboxTrigger.size.y;
        }

        protected override void HeritedUpdate()  //UPDATE
        {
            base.HeritedUpdate();
            CheckInputs();
            GhostStrikeManager();
            InitialJumpManager();
            SpriteFlip();
            LineDashManager();
            PlayerMarkerManager();
        }

        protected override void HeritedFixedUpdate()
        {

        }

        #endregion

        #region Entity CallBacks

        public override void LevelStart()
        {
            maxTargetNumbers = GameManager.gameState.Parameters.MaximumDashs;
            
        }

        public override void ElevatorDing()
        {
            sprite.enabled = true;
        }

        #endregion

        #region Inputs

        /// <summary>
        /// Check Inputs, mobile or PC.
        /// </summary>
        private void CheckInputs()
        {
            if (CanPlaceMarker() && !IsDetected) //Once Ghost Strike is On, Tapping again spawns a target on the finger position.
            {
                if (HasTouched() && !gameplayUIManager.CheckInputUi(Input.GetTouch(0).position))
                {
                    PlaceMarker();
                }
                if(HasClicked())
                {
                    PlaceMarker();
                }
            }

            if (CanPlaceMarker() && markerPlaced && !IsDetected) // While the finger is still sliding on the screen, the target follows it.
            {
                if (HasMovedFinger())
                {
                    ReplacingMarker(true);
                }
                else if (Input.GetKey(KeyCode.Mouse0) && !gameplayUIManager.CheckInputUi(Input.mousePosition))
                {
                    ReplacingMarker(false);
                }
            }

            if (CanPlaceMarker() && markerPlaced && !IsDetected && !hasReleasedFirstFinger)
            {
                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    hasReleasedFirstFinger = true;
                }
                else if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    hasReleasedFirstFinger = true;
                }
            }

            if (CanPlaceMarker() && markerPlaced && !IsDetected && hasReleasedFirstFinger && !isInitialJumping) // On finger release, we confirm the target position, adding it to the targets List.
            {
                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    ConfirmingMarkerPlacement(true);
                    //touchManager.ReplaceTarget();
                }
                else if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    ConfirmingMarkerPlacement(false);
                    //touchManager.ReplaceTarget();
                }
            }
        }

        /// <summary>
        /// Called when the player is still replacing his marker.
        /// </summary>
        /// <param name="mobile">If true, we're on mobile build. Else, we're on PC.</param>
        private void ReplacingMarker(bool mobile)
        {
            if(mobile)
            {
                currentMarker.transform.position = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            }

            else
            {
                currentMarker.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            }

            if (hasRaisedFinger)
            {
                markers.Add(currentMarker);
                hasRaisedFinger = false;
            }
            currentMarker.InitiateCast(markers[markers.Count - 2].transform, hitboxTrigger.size.x);
        }

        /// <summary>
        /// Called when releasing mouse/finger to confirm the marker location.
        /// </summary>
        /// <param name="mobile">If true, we're on mobile build. Else, we're on PC.</param>
        private void ConfirmingMarkerPlacement(bool mobile)
        {
            if(mobile)
            {
                AddTarget(Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 10)));
            }

            else
            {
                AddTarget( Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)));
            }
            hasRaisedFinger = true;
            currentMarker.ShouldCast = false;           
        }

        /// <summary>
        /// Returns true on a single touch not aimed at the UI.
        /// </summary>
        /// <returns></returns>
        private bool HasTouched()
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        private bool HasClicked()
        {
            if(Input.GetKeyDown(KeyCode.Mouse0) && !gameplayUIManager.CheckInputUi(Input.mousePosition))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns true if the finger is sliding on screen.
        /// </summary>
        /// <returns></returns>
        private bool HasMovedFinger()
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved && !gameplayUIManager.CheckInputUi(Input.GetTouch(0).position))
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        #endregion

        #region Preparing Strike

        /// <summary>
        /// Called in the Player touch manager.
        /// </summary>
        public void InitiateOrStartStrike()
        {
            if (!isPreparing && !IsDashing && !IsDetected && CanInitiateStrike) //Casting the Ghost Strike ability
            {
                InitiateGhostStrike();
            }
            if (CanStrike && !IsDetected)  //Slash screen to initiate ghost strike!
            {
                StartGhostStrike();
            }
        }

        /// <summary>
        /// Called when the Player touches the screen, preparing his Strike.
        /// </summary>
        private void InitiateGhostStrike()
        {
            canReceiveInputsAgain = false;
            playerMarker = markersPool.TargetToPool();
            playerMarker.gameObject.SetActive(true);
            playerMarker.transform.position = transform.position;
            markers.Add(playerMarker); //Adding the player to the markers list, to trace the line Renderer from the player to the first target too;
            #region TestList
            /*
            for (int i = 0; i < markers.Count; i++)
            {
                if(i == 0)
                {
                    markers[i] = markers[markers.Count - 1];
                }

                else
                {
                    markers[i + 1] = markers[i];
                }
            }
            */
            #endregion
            markers[0] = playerMarker;
            playerMarker.Sprite.enabled = false;
            ActivateLineDash(true);
            //touchManager.ReplaceTarget();
            InitialJump();
            bulletTime.SlowTime(ghostTimeScale, ghostSpeedSlowMo); // Slow Mo   
            isPreparing = true;
            canInitiateStrike = false;
            GameManager.gameState.CallGhostStrikePreparing();
            StartCoroutine(CanReceiveInputDelay());
        }

        private IEnumerator CanReceiveInputDelay()
        {
            yield return new WaitForSecondsRealtime(.5f);
            canReceiveInputsAgain = true;
        }

        private void PlayerMarkerManager()
        {
            if(playerMarker != null)
            {
                playerMarker.transform.position = transform.position;
            }
        }

        /// <summary>
        /// Called when the Player prepares his Strike, making a small jump.
        /// </summary>
        private void InitialJump()
        {
            Anim.SetTrigger("Jump");
            fx.JumpFX();
            isInitialJumping = true;
            initialJumpStartPosition = transform.position;
            GravityToggle(false);
        }

        /// <summary>
        /// Manages the player initial Jump with an animation curve.
        /// </summary>
        private void InitialJumpManager()
        {
            if (isInitialJumping && initialJumpCompletion < 1)
            {
                body.MovePosition(initialJumpStartPosition + new Vector3(0, initialJumpCurve.Evaluate(initialJumpCompletion) * initialJumpHeight, transform.position.z));
                initialJumpCompletion += initialJumpCompletionSpeed * Time.unscaledDeltaTime;
            }

            if (isInitialJumping && initialJumpCompletion >= 1)
            {
                isInitialJumping = false;
                initialJumpCompletion = 0;
                body.MovePosition(initialJumpStartPosition + new Vector3(0, initialJumpCurve.Evaluate(1f) * initialJumpHeight, transform.position.z));
            }
        }

        /// <summary>
        /// Checks if the player meets the condition to spawn a new target marker.
        /// </summary>
        private bool CanPlaceMarker()
        {
            if (isPreparing && targets.Count < maxTargetNumbers && !IsDashing && canReceiveInputsAgain)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        /// <summary>
        /// Pools a Target Sprite on touch position.
        /// </summary>
        private void PlaceMarker()
        {
            markerPlaced = true;
            currentMarker = markersPool.TargetToPool();
            currentMarker.gameObject.SetActive(true);
            currentMarker.GetComponent<Animator>().SetTrigger("In");
            GameManager.gameState.CallTargetPlaced();
        }

        /// <summary>
        /// Adds a target to the list.
        /// </summary>
        private void AddTarget(Vector3 targetPos)
        {
            targets.Add(targetPos);
            if (targets.Count > 1)
            {
                CanStrike = true;
            }
        }

        /// <summary>
        /// Traces a Line Renderer between all the player's targets.
        /// </summary>
        private void LineDashManager()
        {
            lineDash.positionCount = markers.Count;
            for (int i = 0; i < markers.Count; i++)
            {
                lineDash.SetPosition(i, markers[i].transform.position);                
            }
        }

        #endregion

        #region Striking
        /// <summary>
        /// Upon confirmation of marked targets, the player starts his Ghost Strike.
        /// </summary>
        public void StartGhostStrike()
        {
            DeactivateMarkers();
            ActivateLineDash(false);
            Anim.SetTrigger("Dash");
            dashStartPos = transform.position;
            IsGhostStriking = true;
            hitboxSolid.enabled = false;
            CanStrike = false;
            IsDashing = true;
            GravityToggle(false);
            isPreparing = false;
            GameManager.gameState.CallGhostStrikeStart();
            GameManager.gameState.CallGhostStrikeStartDash();
            Time.timeScale = 1f;
        }

        private void DeactivateMarkers()
        {
            StartCoroutine(DeactivatingMarkers());
        }

        private IEnumerator DeactivatingMarkers()
        {
            for (int i = 1; i < markers.Count; i++)
            {
                markers[i].Anim.SetTrigger("Out");
                yield return new WaitForSecondsRealtime(0.02f);
            }
        }

        /// <summary>
        /// Activate or deactivate LineDash.
        /// </summary>
        /// <param name="isOn">If true, LineDash is On. If false, LineDash is off.</param>
        private void ActivateLineDash(bool isOn)
        {
            lineDash.gameObject.SetActive(isOn);
            if (!isOn)
            {
                lineDash.positionCount = 0;
            }
        }

        /// <summary>
        /// Manages player's movement when Ghost Striking.
        /// </summary>
        private void GhostStrikeManager()
        {
            if(isDashing)
            {
                hitboxTrigger.size = new Vector2(hitboxTrigger.size.x, targetCapsuleYSize);
            }else
            {
                hitboxTrigger.size = new Vector2(hitboxTrigger.size.x, 1);
                hitboxTrigger.transform.position = transform.position;
            }

            if (IsDashing && isGhostStriking)
            {

                switch (moveSpeedType)
                {
                    case MoveSpeedType.ConstantSpeed:
                        dashCompletion += ((dashSpeed / (Vector3.Distance(dashStartPos, targets[currentTarget])) * Time.deltaTime));
                        break;

                    case MoveSpeedType.ConstantTime:
                        dashCompletion += (dashSpeed * Time.deltaTime);
                        break;
                }

                //body.MovePosition(Vector3.Lerp(dashStartPos, targets[currentTarget], curveDash.Evaluate(dashCompletion)));

                Vector3 dir = (Vector3.Lerp(dashStartPos, targets[currentTarget], curveDash.Evaluate(dashCompletion)) - transform.position);
                float zRot = Vector3.SignedAngle(transform.up, dir, transform.forward);
                hitboxTrigger.transform.rotation = Quaternion.Euler(0, 0, zRot);
                hitboxTrigger.transform.position = transform.position + dir.normalized * hitboxDistance;
                transform.position = (Vector3.Lerp(dashStartPos, targets[currentTarget], curveDash.Evaluate(dashCompletion)));

                if (dashCompletion >= 1)
                {
                    MarkerReached();
                }
            }
        }

        /// <summary>
        /// Called when Player reaches one of his markers.
        /// </summary>
        private void MarkerReached()
        {
            IsDashing = false;
            transform.position = targets[currentTarget];
            dashStartPos = transform.position;
            currentTarget++;
            dashCompletion = 0f;
            GameManager.gameState.CallGhostStrikeEndDash();

            //SFX - FX marker reached

            if (currentTarget >= targets.Count && !IsDetected)
            {
                EndGhostStrike();
            }

            else
            {
                StartCoroutine(StrikeDelay());
            }
        }

        /// <summary>
        /// Called when ending the GhostStrike Ability
        /// </summary>
        private void EndGhostStrike()
        {
            Time.timeScale = 1f;

            IsGhostStriking = false;
            hitboxSolid.enabled = true;
            targets.Clear();
            IsDashing = false;
            markersPool.DeactivateAll();
            GameManager.gameState.CallGhostStrikeEnd();
            StartCoroutine(ResetGravity());
        }

        /// <summary>
        /// Calling all feedbacks when detected
        /// </summary>
        private void GhostStrikeDetectedFeedbacks()
        {
            GameManager.gameState.CamZoom.TriggerCamZoom(1);
            GameManager.gameState.CamFollow.IsFollowing = true;
            GameManager.gameState.UiManager.Detected();
        }

        /// <summary>
        /// Resetting gravity with a little delay at the end for a better gamefeel.
        /// </summary>
        private IEnumerator ResetGravity()
        {
            yield return new WaitForSeconds(strikeDelay);
            GravityToggle(true);
            lookingForElevator = true;
            hitboxSolid.size *= 0.9f;
            if (isGrounded)
            {
                FinalSalute();
            }

            else
            {
                ShouldSalute = true;
                anim.SetTrigger("Fall");
            }
        }

        /// <summary>
        /// Play final salute and enemies death animations. Called by PlayerCollision.cs, when player lands.
        /// </summary>
        public void FinalSalute()
        {
            hitboxTrigger.enabled = false;
            hitboxSolid.enabled = false;
            GravityToggle(false);
            anim.SetTrigger("SwordIn");
            GameManager.gameState.CallPlayerFinalSalute();
            GameManager.gameState.PlayAnimatorsDeathAnim(elevator.PlayerLandedOn);
        }

        /// <summary>
        /// Delay between each strike.
        /// </summary>
        /// <returns></returns>
        private IEnumerator StrikeDelay()
        {
            yield return new WaitForSeconds(strikeDelay);
            IsDashing = true;
            GameManager.gameState.CallGhostStrikeStartDash();

        }
        #endregion

        /// <summary>
        /// Toggles player affected by gravity or not.
        /// </summary>
        private void GravityToggle(bool isOn)
        {
            if (isOn)
            {
                body.gravityScale = gravityScale;
            }

            else
            {
                body.gravityScale = 0f;
            }
        }

        /// <summary>
        /// Called when player is detected.
        /// </summary>
        public void GhostStrikeDetected()
        {
            IsGhostStriking = false;
            hitboxSolid.enabled = true;
            targets.Clear();
            IsDashing = false;
            markersPool.DeactivateAll();
            GhostStrikeDetectedFeedbacks();
        }

        /// <summary>
        /// Manages Player Sprite flip according to his dash's direction.
        /// </summary>
        private void SpriteFlip()
        {
            if (isDashing && !isDetected)
            {
                if (targets[currentTarget].x < transform.position.x)
                {
                    sprite.flipX = true;
                }

                else
                {
                    sprite.flipX = false;
                }
            }
        }

        /// <summary>
        /// Reloads current level.
        /// </summary>
        private void ReloadScene()
        {
            LevelsManager.ReloadCurrentLevel();
        }

        /// <summary>
        /// Finds the elevator in the scene and reference it. DO NOT USE DURING RUNTIME.
        /// </summary>
        [ContextMenu("Reference Elevator")]
        private void FindElevatorReference()
        {
            elevator = GameObject.Find("Elevator_Platform").GetComponent<ElevatorPlatform>();
        }

        #region Properties

        private bool IsMouseFarEnough // buggy af
        {
            get
            {
                if (markers.Count == 0) return true;

                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos = new Vector3(mousePos.x, mousePos.y, 0);
                return (Vector3.Distance(mousePos, markers[markers.Count - 1].transform.position) > minimumDistanceBetweenMarks);
            }
        }

        private bool IsTouchFarEnough // buggy af
        {
            get
            {
                if (markers.Count == 0) return true;
                Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                touchPos = new Vector3(touchPos.x, touchPos.y, 0);
                return (Vector3.Distance(touchPos, markers[markers.Count - 1].transform.position) > minimumDistanceBetweenMarks);
            }
        }

        public bool IsDashing
        {
            get
            {
                return isDashing;
            }

            set
            {
                isDashing = value;
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

        public bool IsGrounded
        {
            get
            {
                return isGrounded;
            }

            set
            {
                isGrounded = value;
            }
        }

        public bool ShouldSalute
        {
            get
            {
                return shouldSalute;
            }

            set
            {
                shouldSalute = value;
            }
        }

        public bool IsGhostStriking
        {
            get
            {
                return isGhostStriking;
            }

            set
            {
                isGhostStriking = value;
            }
        }

        public Marker GetLastMarker { get => markers[markers.Count - 1]; }

        public bool CanInitiateStrike { get => canInitiateStrike; set => canInitiateStrike = value; }

        public bool IsDetected { get => isDetected; set => isDetected = value; }

        public PlayerFXs Fx { get => fx; set => fx = value; }
        public bool CanStrike { get => canStrike; set => canStrike = value; }
        public bool LookingForElevator { get => lookingForElevator; }

        #endregion
    }
}