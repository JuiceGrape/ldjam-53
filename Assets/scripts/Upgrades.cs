using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrades : MonoBehaviour
{
    public static Upgrades instance
    {
        get;
        private set;
    }

    public Upgrade speed = new Upgrade("Max Speed", 10, 10.0f, 2.0f, 1.0f);
    public Upgrade accell = new Upgrade("Acelleration", 10, 10.0f, 2.0f, 1.0f);
    public Upgrade brake = new Upgrade("Braking Force", 10, 10.0f, 2.0f, 1.0f);
    public Upgrade turning = new Upgrade("TurningSpeed", 10, 10.0f, 2.0f, 0.25f);
    //public Upgrade plating = new Upgrade("Armour plating", 10, 10.0f, 2.0f);

    public Upgrade spikes = new Upgrade("Bumper Spikes", 1, 100.0f, 1.0f, -0.5f);

    public Upgrade tips = new Upgrade("Tip amount", 10, 10.0f, 2.0f, 1.0f);
    public Upgrade wages = new Upgrade("Get a raise", 10, 10.0f, 2.0f, 0.5f);
    //public Upgrade patience = new Upgrade("Customer patience", 10, 10.0f, 2.0f);


    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            throw new System.Exception("Multiple upgrades instances");
        }

        instance = this;
    }

    void Upgrade(Upgrade upgrade)
    {
        PlayerController.instance.cash.DecreaseValue(upgrade.GetCurrentCost());
        upgrade.UpgradeOnce();
    }
}
