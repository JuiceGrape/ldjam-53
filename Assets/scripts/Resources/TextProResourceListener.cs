using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextProResourceListener : TextResourceListener
{


     TMP_Text text;

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
}
