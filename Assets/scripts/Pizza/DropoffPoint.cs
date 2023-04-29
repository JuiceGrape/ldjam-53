using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DropoffPoint : MonoBehaviour
{
    new private Collider collider;

    private void Start()
    {
        collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (PizzaController.instance.HasActiveRequest())
        {
            PizzaController.instance.CompleteRequest(1.0f);
        }
    }
}