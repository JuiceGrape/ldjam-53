using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance
    {
        get;
        private set;
    }

    [SerializeField] public Resource health;
    [SerializeField] public Resource cash;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            throw new System.Exception("Multiple player controller instances");
        }

        instance = this;
    }

    public void TakeDamage(float damage)
    {
        //TODO: Car breaking down when dead
        //TODO: Zombies swarm car when broken down

        health.DecreaseValue(Upgrades.instance.spikes.CalculateValue(damage));
    }
}
