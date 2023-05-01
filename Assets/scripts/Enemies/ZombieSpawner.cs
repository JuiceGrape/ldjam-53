using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private GameObject SpawnerVisual;
    [SerializeField] private Vector3 SpawnOffset = new Vector3(0, -0.85f, 0);
    [SerializeField] private float SpawnRange = 20.0f;
    [SerializeField] private int StartSpawnCount = 5;
    [SerializeField] private Zombie[] spawnableZombies;
    
    
    // Start is called before the first frame update
    void Start()
    {
        SpawnerVisual.SetActive(false);
        for (int i = 0; i < StartSpawnCount; i++)
        {
            Spawn();
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

    public void Spawn()
    {
        ZombieController.LiveZombies++;
        Zombie zombie = Instantiate(spawnableZombies[Random.Range(0, spawnableZombies.Length)]);
        zombie.transform.position = RandomNavSphere(transform.position, SpawnRange, -1) + SpawnOffset;
    }

}
