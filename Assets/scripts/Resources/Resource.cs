﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Resource : MonoBehaviour
{
    public string m_name = "";
    public float startValue = 0f;

    public float maxValue = 0f;
    public float minValue = 0f;

    [SerializeField]
    private float currentValue;

    public UnityEvent OnValueChanged;

    void Start()
    {
        Reset();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            IncreaseValue(0.1f);
        }
    }

    public float Reset()
    {
        return SetValue(startValue);
    }

    public float IncreaseValue(float value)
    {
        if (value > 0.0f)
        {
            currentValue += value;
        }
        if (currentValue > maxValue)
        {
            currentValue = maxValue;
        }
        OnValueChanged.Invoke();
        return currentValue;
    }

    public float DecreaseValue(float value)
    {
        if (value > 0.0f)
        {
            currentValue -= value;
        }
        if (currentValue <= minValue)
        {
            currentValue = minValue;
        }
        OnValueChanged.Invoke();
        return currentValue;
    }

    public float SetValue(float value)
    {
        currentValue = value;
        OnValueChanged.Invoke();
        return currentValue;
    }

    public float GetValue()
    {
        return currentValue;
    }
}
