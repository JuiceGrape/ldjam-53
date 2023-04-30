using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade { 

    public string name
    {
        get;
        private set;
    }

    public int maxLevel
    {
        get;
        private set;
    }

    public int currentLevel
    {
        get;
        private set;
    }

    float costMod;
    float baseCost;
    float valueModifier;

    public Upgrade(string name, int maxLevel, float baseCost, float costPerLevelModifier, float valueModifier)
    {
        this.name = name;
        this.maxLevel = maxLevel;
        this.baseCost = baseCost;
        this.valueModifier = valueModifier;

        currentLevel = 0;
        costMod = costPerLevelModifier;
    }

    public bool CanUpgrade(float currentCash)
    {
        if (currentLevel >= maxLevel)
            return false;

        return GetCurrentCost() <= currentCash; 
    }

    public float GetCurrentCost()
    {
        return baseCost + (baseCost * costMod * currentLevel);
    }

    public void UpgradeOnce()
    {
        currentLevel++;
    }

    public float CalculateValue(float input)
    {
        return input + (input * currentLevel * valueModifier);
    }

}
