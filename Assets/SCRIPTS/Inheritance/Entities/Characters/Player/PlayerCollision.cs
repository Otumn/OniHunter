using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagann.OniHunter
{
    public class PlayerCollision : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Player player;
        [SerializeField] private FreezeFrame freezeFrame;
        
        private void OnTriggerEnter2D(Collider2D coll)
        {            
            if (coll.gameObject.GetComponent<LineOfSight>() != null && !player.IsDetected) // DETECTED BY ENEMY
            {
                Detected(coll.GetComponent<LineOfSight>());
            }

            if (coll.GetComponentInParent<IDamageListener>() != null && !player.IsDetected && player.IsDashing) // DAMAGING SOMETHING
            {
                //Debug.Log("Collided with damage listening object");
                coll.GetComponentInParent<IDamageListener>().DamageSelf();
                player.Anim.SetTrigger("Attack");
                freezeFrame.TriggerFreezeFrame();

                if (coll.GetComponentInParent<ElectricButton>() == null)
                {
                    player.Fx.Slash(); //Slash FX
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D coll)
        {
            if (coll.gameObject.tag == "Platform" && player.LookingForElevator)
            {
                if (coll.gameObject.GetComponent<ElevatorPlatform>() != null && coll.gameObject.GetComponent<ElevatorPlatform>().IsWaitingForPlayer)
                {
                    coll.gameObject.GetComponent<ElevatorPlatform>().PlayerLandedOn = true;
                }
                SetGrounded(true);
                // player.ShouldGetBackToElevator = true;
                // copy the rest inside player.cs 
                if (player.ShouldSalute) // Final Salute on Landing
                {
                    player.FinalSalute();
                    player.CanInitiateStrike = false;
                    player.ShouldSalute = false;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Platform")
            {
                SetGrounded(false);
            }
        }        

        private void DamageEnemy(Enemy enemy)
        {
            enemy.HealthPoints -= 1;
            GameManager.gameState.CallEnemyHit();
            GameManager.gameState.CamZoom.TriggerCamZoom(0);// Feedbacks Killing Enemy
            //FREEZEFRAME
        }

        private void Detected(LineOfSight lineOfSight)
        {
            lineOfSight.GetComponentInParent<IDetectionListener>().DetectedPlayer();
            player.IsDetected = true;
            player.GhostStrikeDetected();
            player.transform.position = lineOfSight.PlayerPosOnDetection.position;
            GameManager.gameState.CallPlayerDetected(player.transform.position);
            GameManager.gameState.CallPlayerFinalSalute();
            Debug.Log("Player Detected");
        }

        private void SetGrounded(bool isGrounded)
        {
            player.IsGrounded = isGrounded;
        }
    }
}
