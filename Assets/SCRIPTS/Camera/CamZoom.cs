using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    [System.Serializable]
    public struct Zooms
    {
        #region Zoom Class Fields
        [SerializeField] private string name;
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private float speed;
        #endregion

        #region Zoom Class Properties
        public AnimationCurve Curve
        {
            get
            {
                return curve;
            }

            set
            {
                curve = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public float Speed
        {
            get
            {
                return speed;
            }

            set
            {
                speed = value;
            }
        }
        #endregion
    }

    public class CamZoom : MonoBehaviour
    {
        #region Private Variables
        [Header("Camera")]
        [SerializeField] private Camera cam;

        [Space(10)]
        [SerializeField] private Zooms[] zooms;
        [Space(10)]
        [Header("Debug")]
        [SerializeField] private KeyCode debugBind = KeyCode.G;
        [SerializeField] private int debugIndex = 0;

        private int currentZoomIndex = 0;
        private float camZoomMultiplier = 1f;
        private float camZoomCompletion = 0f;
        private bool isZooming = false;
        #endregion

        #region Private Methods
        private void Start()
        {
            camZoomMultiplier = cam.orthographicSize;
        }

        private void Update()
        {
            ZoomManager();
            CheatCode();
        }

        private void ZoomManager()
        {
            if (isZooming)
            {
                cam.orthographicSize = zooms[currentZoomIndex].Curve.Evaluate(camZoomCompletion) * camZoomMultiplier;
                camZoomCompletion += zooms[currentZoomIndex].Speed * Time.unscaledDeltaTime;
                if (camZoomCompletion >= 1)
                {
                    isZooming = false;
                    cam.orthographicSize = camZoomMultiplier;
                }
            }
        }

        private void CheatCode()
        {
            if (Input.GetKeyDown(debugBind))
            {
                TriggerCamZoom(debugIndex);
            }
        }
        #endregion

        #region Public Methods
        public virtual void TriggerCamZoom(int index)
        {
            currentZoomIndex = index;
            isZooming = true;
            camZoomCompletion = 0f;
        }

        public virtual void TriggerCamZoom(string name)
        {
            for (int i = 0; i < zooms.Length; i++)
            {
                if(zooms[i].Name == name)
                {
                    currentZoomIndex = i;
                }
            }

            isZooming = true;
            camZoomCompletion = 0f;
        }
        #endregion
    }
}
