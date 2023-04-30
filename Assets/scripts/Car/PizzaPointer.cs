using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaPointer : MonoBehaviour
{
    public Transform pickupLocation;
    void Update()
    {
        if (PizzaController.instance.HasActiveRequest())
        {
            transform.LookAt(PizzaController.instance.GetActiveRequest().cachedTarget.transform);
        }
        else
        {
            transform.LookAt(pickupLocation);
        }
    }
}
