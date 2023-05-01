using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text cash;
    [SerializeField] private TMP_Text kills;
    [SerializeField] private TMP_Text crashes;
    [SerializeField] private TMP_Text repairs;
    [SerializeField] private TMP_Text driven;

    // Start is called before the first frame update
    void Start()
    {
        GameStats stats = GameStats.instance;

        cash.text = stats.cashEarned.ToString("f2");
        kills.text = stats.zombiesKilled.ToString();
        crashes.text = stats.timesCrashed.ToString();
        repairs.text = stats.repairAmount.ToString();
        driven.text = stats.distanceDriven.ToString("f2"); ;
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        Car.Broken = false;
        ZombieController.LiveZombies = 0;
        SceneManager.LoadScene(0);
    }
}
