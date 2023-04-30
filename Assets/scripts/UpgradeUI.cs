using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private TMP_Text upgradeCost;
    [SerializeField] private TMP_Text upgradeName;
    [SerializeField] private TMP_Text upgradeLevel;
    [SerializeField] private Button upgradeButton;

    [SerializeField] private UpgradeType upgrade;
    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
        upgradeButton.onClick.AddListener(OnUpgradeClick);
        PlayerController.instance.cash.OnValueChanged.AddListener(UpdateUI); //Hacky but works
    }
    void OnUpgradeClick()
    {
        Upgrades.Upgrade(upgrade);
        UpdateUI();
    }

    void UpdateUI()
    {
        Upgrade upr = Upgrades.GetUpgrade(upgrade);

        upgradeName.text = upr.name;
        upgradeLevel.text = upr.currentLevel + "/" + upr.maxLevel;
        setButtonState(upr);
    }

    void setButtonState(Upgrade upr)
    {
        if (upr.currentLevel != upr.maxLevel)
            upgradeCost.SetText(upr.GetCurrentCost().ToString("f2"));
        else
            upgradeCost.SetText("Max level");

        if (upr.CanUpgrade(PlayerController.instance.cash.GetValue()))
        {
            upgradeButton.interactable = true;
        }
        else
        {
            upgradeButton.interactable = false;
        }
    }

    
}
