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

    //Called whenever the pizza timer runs out.
    public UnityEvent<PizzaRequest> OnPizzaFailed;

    //Called whenever the pizza request is no longer valid (completed / failed)
    public UnityEvent<PizzaRequest> OnPizzaEnded;

    //Called whenever a new pizza request begins
    public UnityEvent<PizzaRequest> OnPizzaStarted;

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
                //OnPizzaFailed.Invoke(activeRequest);
                //OnPizzaEnded.Invoke(activeRequest);
                //activeRequest.Deactivate();
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
        float payoutWithMods = Upgrades.instance.wages.CalculateValue(BasePayout);
        float tipWithMods = Upgrades.instance.tips.CalculateValue(BaseTip);

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
        activeRequest.Activate();
        OnPizzaStarted.Invoke(activeRequest);
    }

    //Accuracy is a value from 0.0f to 1.0f based on how close to the front door the pizza landed
    public void CompleteRequest(float accuracy)
    {
        OnPizzaEnded.Invoke(activeRequest);
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
