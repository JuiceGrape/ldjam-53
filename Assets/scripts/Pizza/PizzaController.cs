using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PizzaController : MonoBehaviour
{
    public static PizzaController instance
    {
        get;
        private set;
    }

    public UnityEvent OnPizzaFailed;

    [SerializeField] private DropoffPoint[] dropoffPoints;
    [SerializeField] private float BasePayout = 5.0f;
    [SerializeField] private float BaseTip = 5.0f;
    [SerializeField] private Resource PizzaTimer;

    private PizzaRequest activeRequest;

    void Start()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple pizza controllers in scene");
            throw new System.Exception("Multiple pizza controllers in scene");
        }
        instance = this;

        if (dropoffPoints.Length == 0)
        {
            dropoffPoints = FindObjectsOfType<DropoffPoint>();
        }
        foreach(var point in dropoffPoints)
        {
            point.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (HasActiveRequest())
        {
            if (PizzaTimer.GetValue() == PizzaTimer.minValue)
            {
                OnPizzaFailed.Invoke();
                activeRequest.Deactivate();
            }
            else
            {
                PizzaTimer.DecreaseValue(Time.deltaTime);
            }
        }
    }

    private float CalculatePayout(float accuracy)
    {
        //TODO: Mods
        float payoutWithMods = BasePayout;
        float tipWithMods = BaseTip;

        payoutWithMods *= accuracy;
        tipWithMods *= PizzaTimer.GetValue() / PizzaTimer.maxValue;

        return payoutWithMods + tipWithMods;
    }

    public PizzaRequest generateRequest()
    {
        return new PizzaRequest(dropoffPoints[Random.Range(0, dropoffPoints.Length)]);
    }

    public void StartPizzaQuest(PizzaRequest pizzaRequest)
    {
        activeRequest = pizzaRequest;
        pizzaRequest.Activate();
    }

    //Accuracy is a value from 0.0f to 1.0f based on how close to the front door the pizza landed
    public void CompleteRequest(float accuracy)
    {
        activeRequest.Deactivate();
        PlayerController.instance.cash.IncreaseValue(CalculatePayout(accuracy));
        PizzaTimer.Reset();
    }

    public bool HasActiveRequest()
    {
        return activeRequest != null && activeRequest.isActive;
    }

    //Max time, Passed time
    public Resource GetPizzaTimer()
    {
        return PizzaTimer;
    }

    public PizzaRequest GetActiveRequest()
    {
        return activeRequest;
    }
}
