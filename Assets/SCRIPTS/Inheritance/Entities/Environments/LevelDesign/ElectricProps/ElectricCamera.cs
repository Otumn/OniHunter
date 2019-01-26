using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    public class ElectricCamera : ElectricProp, IDetectionListener, IDamageListener
    {
        #region Private Variables

        [Header ("Camera settings")]
        [SerializeField] private GameObject lineOfSight;
        [SerializeField] private Collider2D lineOfSightColl;
        [SerializeField] private Collider2D camCollider;
        [SerializeField] private GameObject bgCam;
        [SerializeField] private GameObject eyeCam;
        [SerializeField] private GameObject preview;
        [SerializeField] private GameObject pivot;
        [SerializeField] private Animator anim;
        [SerializeField] private Animator losAnim;
        [SerializeField] private CameraFX fx;
        [SerializeField] private bool isRotating = false;
        [SerializeField] private bool pingPong = false;
        [SerializeField] private bool favoriseAntiClockwise = true;
        [SerializeField] [Range(0, 0.999999f)]private float startStateProgression = 0;
        [SerializeField] private float timeBeforeRotating = 1f;
        [SerializeField] private float rotationTime = 0.2f;
        [SerializeField] private int strikesBeforeRotating = 2;
        [SerializeField] [Range(0, 3)] private int[] stateRoutine;
        

        private Vector3 targetRotation;

        private bool rotateInRealTime = false;
        private bool pingponged = false;

        private float timeSpentWaiting = 0f;
        private float currentVel = 0f;

        private int currentState = 0;
        private int accountedStrikes = 0;

        private Vector3 p_targetRotation;
        private int p_state = 0;
        private float p_refVel;
        private bool p_pingponged = false;
        private bool p_favoriseAntiClockwise = true;


        #endregion

        #region MonoBehaviours Callbacks

        protected override void HeritedStart()
        {
            base.HeritedStart();
            preview.SetActive(false);
            targetRotation = new Vector3(0, 0, 90f * stateRoutine[0]);
            timeSpentWaiting = timeBeforeRotating * startStateProgression;
            lineOfSight.SetActive(true);
            if (isOn)
            {
                Activate();
            }
            else
            {
                Deactivate();
            }
        }

        protected override void HeritedUpdate()
        {
            base.HeritedUpdate();
            RotationManager();
            PreviewRotationManager();
        }

        #endregion

        #region Entity CallBacks

        public override void GhostStrikeStart()
        {
            rotateInRealTime = false;
            preview.SetActive(false);
        }

        public override void GhostStrikePreparing()
        {
            preview.SetActive(true);
            p_state = currentState;
            p_favoriseAntiClockwise = favoriseAntiClockwise;
            p_pingponged = pingponged;
            p_targetRotation = targetRotation;
            p_refVel = currentVel;
        }

        public override void TargetPlaced()
        {
            RotatePreview();
        }

        public override void GhostStrikeEndDash()
        {
            accountedStrikes++;
            if(accountedStrikes == strikesBeforeRotating)
            {
                Rotate();
                accountedStrikes = 0;
            }
        }

        public override void PlayerFinalSalute()
        {
            rotateInRealTime = true;
            accountedStrikes = 0;
        }

        public void DetectedPlayer()
        {
            Debug.Log("Player Detected by camera!");
            //Camera animation            
        }

        #endregion

        #region Private methods

        private void Rotate()
        {
            int lastState = stateRoutine[currentState];
            if(pingPong)
            {
                if(currentState >= stateRoutine.Length - 1)
                {
                    pingponged = true;
                    favoriseAntiClockwise = !favoriseAntiClockwise;
                }
                if(currentState <= 0)
                {
                    pingponged = false;
                    favoriseAntiClockwise = !favoriseAntiClockwise;
                }

                if(pingponged)
                {
                    currentState--;
                }
                else
                {
                    currentState++;
                }
            }
            else
            {
                if (currentState >= stateRoutine.Length - 1)
                {
                    currentState = 0;
                }
                else
                {
                    currentState++;
                }
            }

            if(Mathf.Abs(stateRoutine[currentState] - lastState) > 1)
            {
                if(favoriseAntiClockwise)
                {
                    pivot.transform.eulerAngles += new Vector3(0, 0, 5);
                }
                else
                {
                    pivot.transform.eulerAngles += new Vector3(0, 0, -5);
                }
            }

            RotatePreview();
            targetRotation = new Vector3(0, 0, 90f * stateRoutine[currentState]);
            //anim
        }

        private void RotatePreview()
        {
            if (preview.activeSelf)
            {
                int p_lastState = stateRoutine[p_state];
                if (pingPong)
                {
                    if (p_state >= stateRoutine.Length - 1)
                    {
                        p_pingponged = true;
                        p_favoriseAntiClockwise = !p_favoriseAntiClockwise;
                    }
                    if (p_state <= 0)
                    {
                        p_pingponged = false;
                        p_favoriseAntiClockwise = !p_favoriseAntiClockwise;
                    }

                    if (p_pingponged)
                    {
                        p_state--;
                    }
                    else
                    {
                        p_state++;
                    }
                }
                else
                {
                    if(p_state >= stateRoutine.Length - 1)
                    {
                        p_state = 0;
                    }
                    else
                    {
                        p_state++;
                    }
                }

                if (Mathf.Abs(stateRoutine[p_state] - p_lastState) > 1)
                {
                    if (p_favoriseAntiClockwise)
                    {
                        preview.transform.eulerAngles += new Vector3(0, 0, 5);
                    }
                    else
                    {
                        preview.transform.eulerAngles += new Vector3(0, 0, -5);
                    }
                }

                p_targetRotation = new Vector3(0, 0, 90f * stateRoutine[p_state]);
            }
        }

        private void RotationManager()
        {
            if(rotateInRealTime)
            {
                timeSpentWaiting += Time.deltaTime;
                if(timeSpentWaiting > timeBeforeRotating)
                {
                    timeSpentWaiting = 0;
                    Rotate();
                }
            }
            pivot.transform.eulerAngles = new Vector3(0, 0, Mathf.SmoothDampAngle(pivot.transform.eulerAngles.z, targetRotation.z, ref currentVel, rotationTime));
        }

        private void PreviewRotationManager()
        {
            if(preview.activeSelf)
            {
                preview.transform.eulerAngles = new Vector3(0, 0, Mathf.SmoothDampAngle(preview.transform.eulerAngles.z, p_targetRotation.z, ref p_refVel, rotationTime));
            }
        }

        #endregion

        #region Public Methods

        public override void Activate()
        {
            base.Activate();
            if(isRotating)
            {
                rotateInRealTime = true;
            }
            losAnim.SetTrigger("In");
            anim.SetBool("isOn", true);
            lineOfSightColl.enabled = true;
            // activated anim
        }

        public override void Deactivate()
        {
            base.Deactivate();
            rotateInRealTime = false;
            losAnim.SetTrigger("Out");
            anim.SetBool("isOn", false);
            lineOfSightColl.enabled = false;
            //deactivated anim
        }

        #endregion

        #region IDamageListener

        public void DamageSelf()
        {

        }

        public void DieInstantly(bool crushed)
        {
            eyeCam.SetActive(false);
            bgCam.SetActive(false);
            lineOfSightColl.enabled = false;
            losAnim.SetTrigger("Out");
            fx.PlayExplosionFX();
            StartCoroutine(DeactivateAll());
            camCollider.enabled = false;
        }
        
        private IEnumerator DeactivateAll()
        {
            yield return new WaitForSeconds(1.5f);
            gameObject.SetActive(false);
        }

        public void ShowInteraction()
        {
           
        }

        public void HideInteraction()
        {
            
        }

        #endregion

        public int StartState
        {
            get
            {
                return stateRoutine[0];
            }
        }

        public GameObject Pivot
        {
            get
            {
                return pivot;
            }
        }

        public GameObject LineOfSight { get => lineOfSight; }
        public Animator Anim { get => anim; }
        public Collider2D LineOfSightColl { get => lineOfSightColl;  }
    }
}
