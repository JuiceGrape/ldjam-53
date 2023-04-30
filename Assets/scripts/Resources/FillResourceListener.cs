using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillResourceListener : MonoBehaviour
{
    public Resource resource;
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
            float fillAddition = targetFill - image.fillAmount;
            fillAddition = Mathf.Clamp(fillAddition, -creepSpeed * Time.deltaTime, creepSpeed * Time.deltaTime);
            image.fillAmount += fillAddition;
        }
    }

    public void OnValueChanged()
    {
        targetFill = resource.GetValue() / (resource.maxValue - resource.minValue) * maxFill;
    }
}

