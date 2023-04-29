using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class PickupPoint : MonoBehaviour
{
    public UnityEvent<PizzaRequest> OnPizzaRequestObtained;
    new private Collider collider;

    private void Start()
    {
        collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!PizzaController.instance.HasActiveRequest())
        {
            var request = PizzaController.instance.generateRequest();
            PizzaController.instance.StartPizzaQuest(request);
            OnPizzaRequestObtained.Invoke(request);
        }
    }
}
