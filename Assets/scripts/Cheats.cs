using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Cheats : MonoBehaviour
{
    public TMP_InputField cheatInputBox;

    private Coroutine flashing;
    private void Start()
    {
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            cheatInputBox.gameObject.SetActive(!cheatInputBox.gameObject.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.Return) && cheatInputBox.gameObject.activeSelf)
        {
            EnterCheat(cheatInputBox.text);
        }
    }

    private void EnterCheat(string cheat)
    {
        if (flashing != null)
        {
            return;
        }

        switch(cheat)
        {
            case "HardMode":
                FindObjectOfType<PizzaPointer>().gameObject.SetActive(false);
                break;
            case "Massacre":
                Upgrades.instance.spikes.UpgradeOnce();
                Upgrades.instance.spikes.UpgradeOnce();
                break;
            case "Motherload":
                PlayerController.instance.cash.IncreaseValue(100.0f);
                break;
            case "Rush Hour":
                FindObjectOfType<ZombieController>().SetSpawnTimer(1f);
                break;
            default:
                flashing = StartCoroutine(FlashBox(Color.red));
                return;
        }
        flashing = StartCoroutine(FlashBox(Color.green));
    }

    IEnumerator FlashBox(Color color)
    {
        Color original = cheatInputBox.GetComponent<Image>().color;

        cheatInputBox.GetComponent<Image>().color = color;
        yield return new WaitForSecondsRealtime(1.0f);
        cheatInputBox.GetComponent<Image>().color = original;
        flashing = null;
    }
}
