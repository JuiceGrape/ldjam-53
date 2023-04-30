using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : Desctructable
{
    [SerializeField] float minChaseDistance = 5.0f;
    [SerializeField] float maxChaseDistance = 20.0f;

    [SerializeField] float minSpeed = 1.5f;
    [SerializeField] float maxSpeed = 3.0f;

    [SerializeField] float minWaitTime = 5.0f;
    [SerializeField] float maxWaitTime = 10.0f;

    [SerializeField] private GameObject hurtFX;

    NavMeshAgent agent;
    Animator animator;
    float chaseDistance;

    bool ready = false;
    bool wandering = true;
    bool dead = false;

    Vector3 startPos;
    Vector3 prevPos;

    Coroutine wanderRoutine;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        chaseDistance = Random.Range(minChaseDistance, maxChaseDistance);
        agent.speed = Random.Range(minSpeed, maxSpeed);

        //GetComponent<Rigidbody>().AddForce(new Vector3(0, 6f, 0), ForceMode.Impulse);
    }
    private void Update()
    {
        
        if (transform.position.y < -100)
        {
            Destroy(gameObject);
        }

        if (dead)
            return;

        if (!ready)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("appear"))
                return;

            ready = true;
            agent.enabled = true;
            GetComponent<Collider>().enabled = true;
            GetComponent<Rigidbody>().useGravity = false;
            startPos = transform.position;
            prevPos = transform.position;
        }

        if (Vector3.Distance(transform.position, prevPos) >= 1.0f * Time.deltaTime)
        {
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
        prevPos = transform.position;

        if (wandering)
        {
            HandleWander();
        }
        else
        {
            HandleChasing();
        }

        
    }

    private void HandleWander()
    {
        if (wanderRoutine == null)
        {
            wanderRoutine = StartCoroutine(Wander(50.0f, minWaitTime, maxWaitTime));
        }
        
        if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) <= chaseDistance)
        {
            wandering = false;
            StopCoroutine(wanderRoutine);
            wanderRoutine = null;
        }
    }

    private void HandleChasing()
    {
        if (agent.isActiveAndEnabled)
            agent.SetDestination(PlayerController.instance.transform.position);

        if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) >= chaseDistance)
        {
            wandering = true;
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }

    override protected void OnDeath(PlayerController player, Car truck)
    {
        if (hurtFX != null)
        {
            hurtFX.SetActive(true);
        }
        GetComponent<Rigidbody>().AddForce(new Vector3(0, 100, 0), ForceMode.Impulse);
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Collider>().enabled = false;
        dead = true;
    }

    //Override to implement own soft hit behaviour
    override protected void OnHit(PlayerController player, Car truck)
    {

    }

    IEnumerator Wander(float range, float minWaitTime, float maxWaitTime)
    {
        while (wandering && !dead)
        {
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);

            agent.SetDestination(RandomNavSphere(transform.position, range, -1));
            while(agent.pathStatus != NavMeshPathStatus.PathComplete && agent.remainingDistance >= 0.5f)
            {
                if (agent.pathStatus == NavMeshPathStatus.PathInvalid)
                {
                    Debug.LogWarning("Zombie found invalid path");
                    break;
                }
                yield return null;
            }
        }
        
    }
}
