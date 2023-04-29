using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextProResourceListener : MonoBehaviour
{
    public Resource resource;
    

    public bool AddsName = false;
    public bool ShowsMax = false;
    public string formatting = "f0";

    TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
        OnResourceChanged();
        resource.OnValueChanged.AddListener(OnResourceChanged);
    }

    void OnResourceChanged()
    {
        text.text = GetText();
    }

    protected string GetText()
    {
        string val = resource.GetValue().ToString(formatting);

        if (ShowsMax)
        {
            val += "/" + resource.maxValue.ToString(formatting);
        }

        if (AddsName)
        {
            val += " " + resource.m_name;
        }

        return val;
    }
}
