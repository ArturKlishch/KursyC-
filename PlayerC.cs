using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GameAnalyticsSDK;
using UnityEngine.SceneManagement;

public class PlayerC : MonoBehaviour 
{
        static void Main(string[] args)
        {
            private void SelectTarget()
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    target = hit.transform.gameObject;

                    destination = hit.point;
                    Debug.Log(target.gameObject.tag + " DISTANCE: " + Vector3.Distance(transform.position, target.transform.position));
                }
                if (target != null)
                {
                    if (target.tag == "Enemy")
                    {
                        isApproachingTarget = true;
                        Move(true);
                        target.GetComponent<Interactable>().setAsAttackTarget();

                    }
                    else if (target.tag == "Item" || target.tag == "NPC" || target.tag == "skrzynia")
                    {
                        isApproachingTarget = true;
                        Move(false);
                        target.GetComponent<Interactable>().setAsTarget();
                    }
                }
            }
            private void CheckIfTargetAlive()
            {
                if (target != null && target.GetComponent<EnemyControler>() != null)
                {
                    if (target.GetComponent<EnemyControler>().stats.Health <= 0)
                    {
                        StopAttacking();
                    }
                }
            }
            public void getHit(float damage)
            {
                stats.Health -= Mathf.Clamp((damage - stats.Defense), 0, stats.Health);

                if (stats.Health <= 0)
                    Die();
            }
            private void Die()
            {
                GameAnalytics.NewDesignEvent("PlayerEvent:Death:" + SceneManager.GetActiveScene().name);
                isAlive = false;
                anim.applyRootMotion = true;
                movement.Die();
                anim.SetBool("isAlive", isAlive);
                GameController.instance.ShowDeathScreen();
            }
            private void CheckIfTargetReached()
            {

                if (target.tag == "Enemy")
                {
                    if (Vector3.Distance(target.transform.position, transform.position) < 5.0f)
                    {

                        isApproachingTarget = false;
                        StopMoving();
                        target.GetComponent<Interactable>().setAsAttackTarget();
                        Attack();
                    }
                }
                else if (target.tag == "Item" || target.tag == "NPC" || target.tag == "skrzynia")
                {
                    if (Vector3.Distance(target.transform.position, transform.position) < 5.0f)
                    {
                        isApproachingTarget = false;
                        StopMoving();
                        Interact();
                    }
                }

            }
            private void StopMoving()
            {
                movement.StopMoving();
            }
            private void Move(bool isEnemy)
            {
                anim.SetBool("isAttacking", false);
                if (!isEnemy)
                {
                    isAttacking = false;
                    movement.MoveToPoint(destination);
                }
                else
                {
                    movement.MoveToPoint(target.transform.position);
                }
            }
            private void Interact()
            {
                target.GetComponent<Interactable>().Interact();
                target.GetComponent<Interactable>().deselectTarget();
            }
            public bool AddExperience(int xp)
            {
                return stats.AddExperience(xp);
            }
            private void Attack()
            {
                isAttacking = true;
                anim.speed = stats.AttackSpeed;
                anim.SetBool("isMoving", false);
                anim.SetBool("isAttacking", true);
            }
            private void StopAttacking()
            {
                isAttacking = false;
                anim.SetBool("isMoving", false);
                anim.SetBool("isAttacking", false);
                anim.speed = 1.0f;
            }
            private void Hit()
            {
                if (target != null)
                {
                    if (target.GetComponent<EnemyControler>() != null)
                    {
                        target.GetComponent<EnemyControler>().getHit(stats.Attack);
                        if (!target.GetComponent<EnemyControler>().isAlive)
                            StopAttacking();
                    }
                }

                anim.speed = stats.AttackSpeed;

            }
        }
    }
    }
}
