using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaRequest {

    public DropoffPoint cachedTarget;
    public bool isActive;

    public PizzaRequest(DropoffPoint target)
    {
        cachedTarget = target;
        isActive = false;
    }

    public void Activate()
    {
        cachedTarget.gameObject.SetActive(true);
        isActive = true;
    }

    public void Deactivate()
    {
        cachedTarget.gameObject.SetActive(false);
        isActive = false;
    }

    public string GetOrderDetails()
    {
        return "TEST";
    }
    

}
