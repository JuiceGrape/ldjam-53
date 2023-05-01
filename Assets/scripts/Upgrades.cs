using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UpgradeType
{
    Speed,
    Acell,
    Brake,
    Turning,
    Spikes,
    Tips,
    Wages
}

public class Upgrades : MonoBehaviour
{
    public static Upgrades instance
    {
        get;
        private set;
    }

    public Upgrade speed = new Upgrade("Max Speed", 10, 25f, 1.0f, 0.2f);
    public Upgrade accell = new Upgrade("Acelleration", 10, 25f, 1.0f, 0.5f);
    public Upgrade brake = new Upgrade("Braking Force", 10, 25f, 1.0f, 1.0f);
    public Upgrade turning = new Upgrade("TurningSpeed", 10, 25f, 1.0f, 0.1f);
    //public Upgrade plating = new Upgrade("Armour plating", 10, 10.0f, 2.0f);

    public Upgrade spikes = new Upgrade("Bumper Spikes", 2, 100.0f, 4.0f, -0.5f);

    public Upgrade tips = new Upgrade("Tip amount", 10, 10.0f, 1.0f, 0.5f);
    public Upgrade wages = new Upgrade("Get a raise", 10, 10.0f, 1.0f, 0.25f);
    //public Upgrade patience = new Upgrade("Customer patience", 10, 10.0f, 2.0f);

    private Dictionary<UpgradeType, Upgrade> upgradeDict;


    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            throw new System.Exception("Multiple upgrades instances");
        }

        instance = this;

        upgradeDict = new Dictionary<UpgradeType, Upgrade>()
        {
            { UpgradeType.Speed, speed },
            { UpgradeType.Acell, accell },
            { UpgradeType.Brake, brake },
            { UpgradeType.Turning, turning },
            { UpgradeType.Spikes, spikes },
            { UpgradeType.Tips, tips },
            { UpgradeType.Wages, wages },
        };
    }

    public static Upgrade GetUpgrade(UpgradeType type)
    {
        return instance.upgradeDict[type];
    }

    public static void Upgrade(UpgradeType upgradeType)
    {
        Upgrade upgrade = GetUpgrade(upgradeType);
        PlayerController.instance.cash.DecreaseValue(upgrade.GetCurrentCost());
        upgrade.UpgradeOnce();
    }
}
