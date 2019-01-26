using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    public class ObjectShaker : MonoBehaviour
    {
        #region Enums

        private enum Space
        {
            Local,
            World
        }
        
        private enum Dimension
        {
            ThreeDimensional,
            TwoDimensional
        }

        #endregion

        #region Fields

        [SerializeField] private Space space;
        [SerializeField] private Dimension dimension;
        [SerializeField] private GameObject objectToShake;
        [SerializeField] private float intensity = 5f;
        [SerializeField] private float frequency = 0f;
        [SerializeField] private bool scaledDeltaTime = true;

        private Vector3 initialPos;
        private bool isThreeD = true;

        public float Intensity { get => intensity; set => intensity = value; }

        #endregion

        #region MonoBehaviour Callbacks

        private void OnEnable()
        {
            Initialize();
            ContinuousShake();
        }
        
        #endregion

        #region Context Menus

        [ContextMenu("Assign Fields")]
        private void AssignFields()
        {
            AssignObject();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes position
        /// </summary>
        private void Initialize()
        {
            AssignObject();

            switch (space)
            {
                case Space.Local:
                    initialPos = objectToShake.transform.localPosition;
                    break;
                case Space.World:
                    initialPos = objectToShake.transform.position;
                    break;
            }

            switch (dimension)
            {
                case Dimension.ThreeDimensional:
                    isThreeD = true;
                    break;
                case Dimension.TwoDimensional:
                    isThreeD = false;
                    break;
            }
        }

        /// <summary>
        /// Continuously shakes object based on Amount parameter.
        /// </summary>
        private void ContinuousShake()
        {
            switch (space)
            {
                case Space.Local:
                    objectToShake.transform.localPosition = initialPos + RandomInsideUnit(isThreeD) * Intensity * Time.deltaTime;
                    break;
                case Space.World:
                    objectToShake.transform.position = initialPos + RandomInsideUnit(isThreeD) * Intensity * Time.deltaTime;
                    break;
            }
            if(scaledDeltaTime)
            {
                StartCoroutine(Frequency(false));
            }

            else
            {
                StartCoroutine(Frequency(true));
            }
        }

        private IEnumerator Frequency(bool realtime)
        {
            if(realtime)
            {
                yield return new WaitForSecondsRealtime(frequency);
            }

            else
            {
                yield return new WaitForSeconds(frequency);
            }

            ContinuousShake();
        }

        /// <summary>
        /// Assigns which object to use automatically as this.gameObject
        /// </summary>
        private void AssignObject()
        {
            if (objectToShake == null)
            {
                objectToShake = this.gameObject;
            }
        }

        /// <summary>
        /// Returns either a Random.insideUnitCircle or Random.insideUnitSphere based on a dimension parameter.
        /// </summary>
        /// <param name="isThreeDimensional"></param>
        /// <returns></returns>
        private Vector3 RandomInsideUnit(bool isThreeDimensional)
        {
            if(isThreeDimensional)
            {
                return Random.insideUnitSphere;
            }

            else
            {
                return Random.insideUnitCircle;
            }
        }

        #endregion
    }
}
