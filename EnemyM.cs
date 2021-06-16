using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyM : MonoBehaviour
{
    Transform target;
    NavMeshAgent agent;
    Animator anim;
    public Animator exclamation;
    private float distanceFromPlayer;
    public SkinnedMeshRenderer renderer;
    public SphereCollider collider;
    private Stats stats;
    bool isAlive = true;
    public bool isTargetInReach = false;
    public bool isApproachingTarget = false;
    public float ReactionDistance = 17.5f;
    public float RoamingDistanceX = 8.0f;
    public float RoamingDistanceZ = 2.5f;
    public float RoamingDelay = 8.0f;

    private float RunningAwayTimer=1.0f;
    private float RoamingTimer;
    private Vector3 startingPosition;
    private bool isRunningAway = false;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        stats = GetComponent<Stats>();
        agent.speed =  stats.WalkingSpeed;
        startingPosition = transform.position;
        (collider as SphereCollider).radius = ReactionDistance;
        RoamingTimer = Random.Range(0,RoamingDelay);
    }
    public void RunAway()
    {
        isRunningAway = true;
    }
    private void FollowPlayer()
    {
        distanceFromPlayer = Vector3.Distance(target.position, transform.position);
        
            if (!isApproachingTarget)
            {
                exclamation.SetTrigger("Show");
                isApproachingTarget = true;
            }
            if (target != null && distanceFromPlayer > 3.9f)
            {
                agent.SetDestination(target.position);
                agent.isStopped = false;
                anim.speed = stats.WalkingSpeed / 7.5f;
            }
            else
            {
                agent.isStopped = true;
                agent.ResetPath();

            }
        
    }
    public void GoBackToStartingPosition()
    {
        if (isAlive)
        {
            isApproachingTarget = false;
            agent.SetDestination(startingPosition);
        }
    }
    void Roam()
    {
        if (isAlive && !isApproachingTarget && !isRunningAway)
        {
            RoamingTimer -= Time.deltaTime;
            if (RoamingTimer <= 0.1)
            {
                agent.SetDestination(startingPosition + new Vector3(Random.Range(-RoamingDistanceX, RoamingDistanceX), 0, Random.Range(-RoamingDistanceZ, RoamingDistanceZ)));
                RoamingTimer = RoamingDelay;
            }
        }
    }
    public void Run()
    {
        if (isAlive)
        {
            RunningAwayTimer -= Time.deltaTime;
            if (RunningAwayTimer <= 0.1)
            {
                agent.SetDestination(9.2f*(startingPosition - target.position));
                RunningAwayTimer = 1.3f;
            }
        }
    }
    void Update()
    {
        if (isTargetInReach && isAlive && !isRunningAway)
        {
            FollowPlayer();        
        }
        if(isAlive)
        {
            anim.SetBool("isMove", agent.hasPath);
        }
        Roam();
        if (isRunningAway)
            Run();
    }
    public void Die()
    {
        isAlive = false;
        agent.enabled = false;
    }
    public void GetHit()
    {
        StartCoroutine(discolour());
    }
    private IEnumerator discolour()
    {
        Color original = renderer.material.color;
        renderer.material.color = Color.red;
        float delay = 0.3f;
        while(delay>=0)
        {
            delay -= Time.fixedDeltaTime;
            renderer.material.color = Color.Lerp(original, Color.red, delay);
            yield return null;
        }
       
      
    }
}
