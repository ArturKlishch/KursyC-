using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyC : Interactable
{
    public ItemPickUp itemDrop;
    private PlayerController target;
    private EnemyMovement movement;
    public bool isAlive = true;
    public bool isAttacking = false;
    private Animator anim;
    private CapsuleCollider collider;
    public Stats stats;
    public List<Item> DropList;
    public int Experience = 0;
    public bool isACoward = false;
    private bool isRunningAway = false;

    override public void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>(); ;
        anim = GetComponent<Animator>();
        stats = GetComponent<Stats>();
        movement = GetComponent<EnemyMovement>();
        collider = GetComponent<CapsuleCollider>();
        base.Start();
    }
    public void getHit(float damage)
    {
        stats.Health -= (damage - stats.Defense);
        movement.GetHit();
        if (stats.Health <= 0)
            Die();
        if(isACoward && stats.Health<= 0.3f*(stats.MaxHealth))
        { 
            RunAway();
        }
    }
    public void RunAway()
    {
        StopAttacking();
        isRunningAway = true;
        movement.RunAway();

    }
    private void FaceTarget()
    {
        Vector3 lookPos = target.transform.position - transform.position;

        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
    }
    private IEnumerator Decompose()
    {
        collider.enabled = false;
        yield return new WaitForSeconds(4.5f);
        float timer = 4.5f;
        while(timer>=0)
        {
            timer -= Time.deltaTime;
            transform.position += new Vector3(0, -Time.deltaTime, 0);
            yield return null;
        }
        Destroy(gameObject);
    }
    private void Die()
    {
        if (isAlive)
        {
            StartCoroutine(Decompose());
            movement.Die();
            var drop = Instantiate(itemDrop, transform.position + Vector3.up, Quaternion.Euler(-90, 0, 0));
            if (DropList.Count > 0)
                drop.item = DropList[Random.Range(0, DropList.Count)];
            GameController.instance.AddExperience(Experience, stats.Name);

            isAlive = false;
            isAttacking = false;
            isInteractable = false;
            gameObject.layer = 2;
            anim.applyRootMotion = true;
            deselectTarget();
            anim.SetBool("isAlieve", isAlive);
        }
    }
    private void Update()
    {
        if (isAlive && movement.isTargetInReach && !isRunningAway)
        {
            if (!isAttacking && Vector3.Distance(transform.position, target.transform.position) <= 4.5)
            {
                Attack();
            }
            else if (Vector3.Distance(transform.position, target.transform.position) > 4.8f)
            {
                StopAttacking();
            }


            if (isAttacking)
            {
                FaceTarget();

                if (!target.isAlive)
                {
                    StopAttacking();
                   
                }
            }
        }
        
    }
    private void Attack()
    {

        isAttacking = true;
        anim.speed = stats.AttackSpeed;
        anim.SetBool("isAttacked", true);
    }
    private void StopAttacking()
    {
        isAttacking = false;

        anim.SetBool("isAttacked", false);
    }
    private void Hit()
    {
        if (target != null && isAlive)
        {
            if (target.GetComponent<PlayerController>() != null)
                target.GetComponent<PlayerController>().getHit(stats.Attack);
        }

    }
}
