using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance
    {
        get;
        private set;
    }

    [SerializeField] public Resource health;
    [SerializeField] public Resource cash;

    [SerializeField] private ParticleSystem damageEffect;
    [SerializeField] private GameOverScreen gameoverScreen;

    [SerializeField] private GameObject pauseMenu;

    public bool gameStarted = false;

    private Vector3 startPos;
    private Quaternion startRotation;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            throw new System.Exception("Multiple player controller instances");
        }

        instance = this;
        Time.timeScale = 0.0f;
        startPos = transform.parent.position;
        startRotation = transform.parent.rotation;
    }

    private void Update()
    {
        if (gameStarted && Input.GetButtonDown("Pause"))
        {
            TogglePause();
        }
    }

    
    public void TogglePause()
    {
        if (Car.Broken)
            return;

        gameStarted = true;

        if (pauseMenu.activeSelf)
        {
            Time.timeScale = 1.0f;
            pauseMenu.SetActive(false);
        }
        else
        {
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
        }
            
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResetPosition()
    {
        transform.parent.position = startPos;
        transform.parent.rotation = startRotation;
        transform.parent.GetComponent<Rigidbody>().velocity = Vector3.zero;
        health.SetValue(10.0f);
        TogglePause();
    }

    public void TakeDamage(float damage, bool zombieDamage)
    {
        //TODO: Car breaking down when dead
        //TODO: Zombies swarm car when broken down

        float calculatedDamage = damage;
        if (zombieDamage)
            calculatedDamage = Upgrades.instance.spikes.CalculateValue(calculatedDamage);
        else
            GameStats.RegisterCrash();

        if (calculatedDamage > 0.01f)
        {
            damageEffect.Play();
            damageEffect.GetComponent<AudioSource>().Play();
        }
        health.DecreaseValue(calculatedDamage);

        if (health.GetValue() <= health.minValue)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        Car.Broken = true;
        for(float scale = 1.0f; scale >= 0.01f; scale-=0.01f)
        {
            Time.timeScale = scale;
            yield return new WaitForSecondsRealtime(0.05f);
        }
        gameoverScreen.gameObject.SetActive(true);
    }
}
