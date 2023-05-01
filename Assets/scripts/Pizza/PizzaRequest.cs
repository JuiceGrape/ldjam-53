using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaRequest {

    private string[] PizzaTypes = new string[]
    {
        "Pizza Margherita",
        "Pizza Fungi",
        "Pizza Salame",
        "Chunky Funky Munky",
        "Raw Pizza",
        "Tripledecker Funtime",
        "Pizza Hawaii",
        "Pizza Berlusconi",
        "Pizza Doner",
        "Pizza Spinaci"
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

    public Sprite GetFace()
    {
        return cachedTarget.face;
    }

    public string GetCustomerName()
    {
        return cachedTarget.personName;
    }

    private string GenerateDetails()
    {
        string retval = "";

        retval += PizzaTypes[Random.Range(0, PizzaTypes.Length)] + "\n";
        retval += "Location: " + cachedTarget.townLocation.ToString() + "\n";
        retval += "Identifier: " + cachedTarget.identifier;

        return retval;
    }

    public string GetOrderDetails()
    {
        return details;
    }

    public float GetOrderMultiplier()
    {
        switch(cachedTarget.townLocation)
        {
            case PartOfTown.Downtown:
                return 1f;
            case PartOfTown.Hillside:
                return 2.5f;
            case PartOfTown.Riverside:
                return 1.25f;
            case PartOfTown.Suburbs:
                return 1.5f;
            default:
                return 1f;
        }
    }
    

}
