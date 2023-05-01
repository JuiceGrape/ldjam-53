using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    [SerializeField] private float spawnTimer = 5.0f;
    [SerializeField] private float timePassed = 0.0f;

    [SerializeField] private float zombieRushTimer = 60.0f;
    [SerializeField] private float zombieRushDuration = 10.0f;
    [SerializeField] private float zombieRushSpawnTimer = 0.5f;
    [SerializeField] private float rushTimePassed = 0.0f;
    [SerializeField] private bool rushActive = false;

    [SerializeField] private ZombieSpawner[] spawners;
    [SerializeField] private int maxZombies = 1000;



    public static int LiveZombies = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (spawners.Length == 0)
        {
            spawners = FindObjectsOfType<ZombieSpawner>();
        }
        
    }

    public void SetSpawnTimer(float timer)
    {
        spawnTimer = timer;
    }

    // Update is called once per frame
    void Update()
    {
        if (LiveZombies >= maxZombies)
            return;

        timePassed += Time.deltaTime;
        rushTimePassed += Time.deltaTime;

        

        if (rushActive)
        {
            if (timePassed >= zombieRushSpawnTimer)
            {
                timePassed -= zombieRushSpawnTimer;
                spawners[Random.Range(0, spawners.Length)].Spawn();
            }

            if (rushTimePassed >= zombieRushDuration)
            {
                rushTimePassed = 0.0f;
                rushActive = false;
            }
        }
        else
        {
            if (timePassed >= spawnTimer)
            {
                timePassed -= spawnTimer;
                spawners[Random.Range(0, spawners.Length)].Spawn();
            }

            if (rushTimePassed >= zombieRushTimer)
            {
                rushTimePassed = 0.0f;
                rushActive = true;
            }
        }
    }
}
