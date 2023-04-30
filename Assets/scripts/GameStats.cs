using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats : MonoBehaviour
{
    public static GameStats instance
    {
        get;
        private set;
    }

    public float cashEarned
    {
        get;
        private set;
    }

    public int zombiesKilled
    {
        get;
        private set;
    }

    public int timesCrashed
    {
        get;
        private set;
    }

    public float distanceDriven
    {
        get;
        private set;
    }
    public int repairAmount
    {
        get;
        private set;
    }

    private void Start()
    {
        if (instance != null)
        {
            throw new System.Exception("more than 1 gamestats");
        }
        instance = this;
        cashEarned = 0;
        distanceDriven = 0;
        zombiesKilled = 0;
        repairAmount = 0;
        timesCrashed = 0;
    }

    public static void RegisterCash(float cash)
    {
        instance.cashEarned += cash;
    }

    public static void RegisterDistance(float distance)
    {
        instance.distanceDriven += distance;
    }

    public static void RegisterKill()
    {
        instance.zombiesKilled++;
    }

    public static void RegisterRepair()
    {
        instance.repairAmount++;
    }

    public static void RegisterCrash()
    {
        instance.timesCrashed++;
    }
}
