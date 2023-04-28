using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextResourceListener : MonoBehaviour
{
    public Resource resource;

    public bool AddsName = false;
    public bool ShowsMax = false;

    Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        OnResourceChanged();
        resource.OnValueChanged.AddListener(OnResourceChanged);
    }
    
    void OnResourceChanged()
    {
        text.text = GetText();
    }

    protected string GetText()
    {
        string val = resource.GetValue().ToString();

        if (ShowsMax)
        {
            val += "/" + resource.maxValue.ToString();
        }

        if (AddsName)
        {
            val += " " + resource.m_name;
        }

        return val;
    }
}
