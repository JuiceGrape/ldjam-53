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

    override protected void OnDeath(PlayerController player, Car truck, Collision collision)
    {
        if (hurtFX != null && Upgrades.instance.spikes.currentLevel > 0)
        {
            hurtFX.SetActive(true);
        }

        GameStats.RegisterKill();
        GetComponent<AudioSource>()?.Play();
        agent.enabled = false;
        GetComponent<Collider>().isTrigger = true;
        dead = true;
        ZombieController.LiveZombies--;

        Vector3 direction = transform.position - truck.transform.position;
        StartCoroutine(BlowAway(-direction.normalized, truck.CurrentSpeed.magnitude / 20));
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

            agent?.SetDestination(RandomNavSphere(transform.position, range, -1));
            while(agent?.pathStatus != NavMeshPathStatus.PathComplete && agent.remainingDistance >= 0.5f)
            {
                if (agent?.pathStatus == NavMeshPathStatus.PathInvalid)
                {
                    Debug.LogWarning("Zombie found invalid path");
                    break;
                }
                yield return null;
            }
        }
        
    }

    IEnumerator BlowAway(Vector3 direction, float flySpeed)
    {
        Vector3 startPoint = transform.position;
        Vector3 endPoint = transform.position - (direction * 20 * flySpeed) - (transform.up * 2.0f);
        Vector3 arcPoint = startPoint + (endPoint - startPoint) / 2 + Vector3.up * 5.0f;


        float count = 0.0f;
        while(count < 1.0f)
        {
            count += 1.0f * Time.deltaTime;

            Vector3 m1 = Vector3.Lerp(startPoint, arcPoint, count);
            Vector3 m2 = Vector3.Lerp(arcPoint, endPoint, count);
            transform.position = Vector3.Lerp(m1, m2, count);
            yield return new WaitForEndOfFrame();
        }

        while(true)
        {
            transform.position = transform.position + (Vector3.down * 5.0f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
