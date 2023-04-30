using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    [SerializeField] private float spawnTimer = 5.0f;
    [SerializeField] private float timePassed = 0.0f;

    [SerializeField] private ZombieSpawner[] spawners;
    // Start is called before the first frame update
    void Start()
    {
        if (spawners.Length == 0)
        {
            spawners = FindObjectsOfType<ZombieSpawner>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed >= spawnTimer)
        {
            timePassed -= spawnTimer;
            spawners[Random.Range(0, spawners.Length)].Spawn();
        }
    }
}
