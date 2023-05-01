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
    public PartOfTown townLocation;
    public string identifier;


    public void Score(Vector3 hitPoint)
    {
        //TODO: Accuracy
        PizzaController.instance.CompleteRequest(1.0f);
    }
}