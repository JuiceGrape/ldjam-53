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

    [SerializeField] private ParticleSystem damageEffect;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            throw new System.Exception("Multiple player controller instances");
        }

        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            cash.IncreaseValue(5.0f);
            health.DecreaseValue(5.0f);
        }
    }

    public void TakeDamage(float damage, bool zombieDamage)
    {
        //TODO: Car breaking down when dead
        //TODO: Zombies swarm car when broken down

        float calculatedDamage = damage;
        if (zombieDamage)
            calculatedDamage = Upgrades.instance.spikes.CalculateValue(calculatedDamage);

        if (calculatedDamage > 0.01f)
        {
            damageEffect.Play();
        }
        health.DecreaseValue(calculatedDamage);
    }
}
