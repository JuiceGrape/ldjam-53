using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillResourceListener : MonoBehaviour
{
    public Resource resource;
    public float minFill = 0.0f;
    public float maxFill = 1.0f;
    public float creepSpeed = 1.0f;

    private Image image;
    private float targetFill;
    
    void Start()
    {
        resource.OnValueChanged.AddListener(OnValueChanged);
        image = GetComponent<Image>();
        OnValueChanged();
    }

    private void Update()
    {
        if (image.fillAmount != targetFill)
        {
            if (Time.timeScale <= 0.5f)
            {
                image.fillAmount = targetFill;
            }
            else
            {
                float fillAddition = targetFill - image.fillAmount;
                fillAddition = Mathf.Clamp(fillAddition, -creepSpeed * Time.deltaTime, creepSpeed * Time.deltaTime);
                image.fillAmount += fillAddition;
            }    
            
        }
    }

    public void OnValueChanged()
    {
        targetFill = UnitIntervalRange(resource.minValue, resource.maxValue, minFill, maxFill, resource.GetValue());
    }

    float UnitIntervalRange(float stageStartRange, float stageFinishRange, float newStartRange, float newFinishRange, float floatingValue)
    {
        float outRange = Mathf.Abs(newFinishRange - newStartRange);
        float inRange = Mathf.Abs(stageFinishRange - stageStartRange);
        float range = (outRange / inRange);
        return (newStartRange + (range * (floatingValue - stageStartRange)));
    }
}

