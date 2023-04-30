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

    NavMeshAgent agent;
    Animator animator;
    float chaseDistance;

    bool ready = false;

    bool wandering = true;

    Vector3 startPos;

    Vector3 prevPos;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        chaseDistance = Random.Range(minChaseDistance, maxChaseDistance);
        agent.speed = Random.Range(minSpeed, maxSpeed);
    }
    private void Update()
    {
        if (transform.position.y < -100)
        {
            Destroy(gameObject);
        }

        if (!ready)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("appear"))
                return;

            ready = true;
            agent.enabled = true;
            GetComponent<Collider>().enabled = true;
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
        if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) <= chaseDistance)
        {
            wandering = false;
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
        GetComponent<Rigidbody>().AddForce(new Vector3(0, 2, 0), ForceMode.Impulse);
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Collider>().enabled = false;
    }

    //Override to implement own soft hit behaviour
    override protected void OnHit(PlayerController player, Car truck)
    {

    }
}
