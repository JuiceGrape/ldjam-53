using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaRequest {

    private string[] PizzaTypes = new string[]
    {
        "Pizza Margharita",
        "Pizza Funghi",
        "Pizza Salame",
        "Pizza Chunky funky munky",
        "Raw Pizza",
        "Tripledecker Funtime"
    };

    public DropoffPoint cachedTarget;
    public bool isActive;

    private string details = "";

    public PizzaRequest(DropoffPoint target)
    {
        cachedTarget = target;
        isActive = false;

        details = GenerateDetails();
    }

    public void Activate()
    {
        isActive = true;
    }

    public void Deactivate()
    {
        isActive = false;
    }

    private string GenerateDetails()
    {
        string retval = "";

        retval += "1 " + PizzaTypes[Random.Range(0, PizzaTypes.Length)] + "\n";
        retval += "Location: " + cachedTarget.townLocation.ToString() + "\n";
        retval += "Identifier: " + cachedTarget.identifier + "\n";
        retval += "Delivery instructions: Throw at front door";
        //retval += "Delivery instructions: Throw pizza at front door";

        return retval;
    }

    public string GetOrderDetails()
    {
        return details;
    }
    

}
