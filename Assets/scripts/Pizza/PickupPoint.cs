using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class PickupPoint : MonoBehaviour
{
    public UnityEvent<PizzaRequest> OnPizzaRequestObtained;
    new private Collider collider;
    [SerializeField] private PizzaUI uiController;

    private void Start()
    {
        collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInChildren<PlayerController>() != null)
        {
            uiController.Open();
        }
    }
}
