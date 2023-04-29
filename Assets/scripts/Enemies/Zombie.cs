using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : Desctructable
{
    private void Update()
    {
        if (transform.position.y < -100)
        {
            Destroy(gameObject);
        }
    }
    override protected void OnDeath(PlayerController player, Car truck)
    {
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Collider>().enabled = false;
    }

    //Override to implement own soft hit behaviour
    override protected void OnHit(PlayerController player, Car truck)
    {

    }
}
