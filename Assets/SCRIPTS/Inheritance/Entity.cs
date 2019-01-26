using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    public class Entity : MonoBehaviour
    {
        #region MonoBehaviour CallBacks

        private void Start()
        {
            HeritedStart();
        }

        private void Update()
        {
            HeritedUpdate();
        }

        private void OnEnable()
        {
            GameManager.gameState.RegisterEntity(this);
            HeritedOnEnable();
        }

        private void OnDisable()
        {
            GameManager.gameState.UnregisterEntity(this);
            HeritedOnDisable();
        }

        private void FixedUpdate()
        {
            HeritedFixedUpdate();
        }

        #endregion

        #region Entity Callbacks

        public virtual void LevelStart()
        {

        }

        public virtual void ElevatorDing()
        {

        }

        public virtual void TargetPlaced()
        {

        }

        public virtual void CollectibleObtained()
        {

        }

        public virtual void GhostStrikePreparing()
        {

        }

        public virtual void GhostStrikeStart()
        {

        }

        public virtual void GhostStrikeStartDash()
        {

        }

        public virtual void GhostStrikeEndDash()
        {

        }

        public virtual void EnemyHit()
        {

        }

        public virtual void EnemyKilled()
        {

        }

        public virtual void PlayerDetected(Vector3 detectionPosition)
        {

        }

        public virtual void GhostStrikeEnd()
        {

        }

        public virtual void PlayerFinalSalute()
        {

        }

        public virtual void LevelEnd(bool mainObj, bool sideObj, bool dashLimit)
        {

        }

        #endregion

        protected virtual void HeritedStart()
        {

        }

        protected virtual void HeritedUpdate()
        {

        }

        protected virtual void HeritedOnEnable()
        {

        }

        protected virtual void HeritedOnDisable()
        {

        }   
        
        protected virtual void HeritedFixedUpdate()
        {

        }
    }

    #region Interfaces

    public interface IDamageListener
    {
        void DamageSelf();
        void DieInstantly(bool crushed);
        void ShowInteraction();
        void HideInteraction();
    }

    public interface IDetectionListener
    {
        void DetectedPlayer();   
    }

    #endregion
}
