using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Desctructable : MonoBehaviour
{
    [SerializeField] private float damageToVehicle = 1.0f;
    [SerializeField] private float minimumSpeed = 8.0f;

    //Override to implement own death behaviour
    virtual protected void OnDeath(PlayerController player, Car truck, Collision collision)
    {
        GetComponent<Collider>().enabled = false;
    }

    //Override to implement own soft hit behaviour
    virtual protected void OnHit(PlayerController player, Car truck)
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerController player = collision.collider.GetComponent<Collider>().GetComponentInChildren<PlayerController>();
        Car truck = collision.collider.GetComponent<Collider>().GetComponent<Car>();
        
        if (player != null && truck != null)
        {
            if (Upgrades.instance.spikes.currentLevel != Upgrades.instance.spikes.maxLevel)
                player.TakeDamage(damageToVehicle);

            OnDeath(player, truck, collision);

            //TODO: Polish: Figure this the fuck out lmao
            //if (collision.relativeVelocity.magnitude >= minimumSpeed)
            //{
            //    player.TakeDamage(damageToVehicle);
            //    OnDeath(player, truck);
            //}
            //else if (collision.relativeVelocity.magnitude >= 1.0f)
            //{
            //    player.TakeDamage(damageToVehicle / 4);
            //    OnHit(player, truck);
            //}
        }
    }
}
