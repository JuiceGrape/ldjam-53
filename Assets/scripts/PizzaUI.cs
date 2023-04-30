using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PizzaUI : MonoBehaviour
{
    public GameObject UIPanel;

    public Button TruckRepairButton;
    public Button PizzaTimeButton;

    public float repairCost = 10.0f;

    bool isOpen = false;

    private void Start()
    {
        PizzaTimeButton.onClick.AddListener(PizzaTime);
        TruckRepairButton.onClick.AddListener(RepairCar);

        PlayerController.instance.cash.OnValueChanged.AddListener(UpdateUI);

        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen && Input.GetButtonDown("Cancel"))
        {
            Close();
        }
    }

    public void UpdateUI()
    {
        TruckRepairButton.GetComponentInChildren<TMP_Text>().text = repairCost.ToString("f2");
        if (PlayerController.instance.cash.GetValue() >= repairCost)
        {
            TruckRepairButton.interactable = true;
        }
        else
        {
            TruckRepairButton.interactable = false;
        }

        if (PizzaController.instance.HasActiveRequest())
        {
            PizzaTimeButton.interactable = false;
        }
        else
        {
            PizzaTimeButton.interactable = true;
        }
    }

    public void RepairCar()
    {
        GameStats.RegisterRepair();
        PlayerController.instance.cash.DecreaseValue(10.0f);
        PlayerController.instance.health.Reset();
    }

    public void PizzaTime()
    {
        if (!PizzaController.instance.HasActiveRequest())
        {
            var pizzaRequest = PizzaController.instance.generateRequest();
            PizzaController.instance.StartPizzaQuest(pizzaRequest);
            Close();
        }
    }

    public void Open()
    {
        UpdateUI();
        UIPanel.SetActive(true);
        isOpen = true;
        Time.timeScale = 0;
    }

    public void Close()
    {
        UIPanel.SetActive(false);
        isOpen = false;
        Time.timeScale = 1;
    }
}
