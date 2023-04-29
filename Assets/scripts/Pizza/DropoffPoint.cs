using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PartOfTown
{
    Suburbs,
    Hillside,
    Downtown,
    Riverside
}

[RequireComponent(typeof(Collider))]
public class DropoffPoint : MonoBehaviour
{
    new private Collider collider;

    public PartOfTown townLocation;
    public string identifier;

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