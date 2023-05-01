using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PizzaCard : MonoBehaviour
{
    [SerializeField] private GameObject cardBack;
    [SerializeField] private TMP_Text cardText;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Image faceImage;
    // Start is called before the first frame update
    void Start()
    {
        PizzaController.instance.OnPizzaStarted.AddListener(OnPizzaStart);
        PizzaController.instance.OnPizzaEnded.AddListener(OnPizzaEnd);
    }

    void OnPizzaStart(PizzaRequest request)
    {
        cardText.text = request.GetOrderDetails();
        nameText.text = request.GetCustomerName();
        faceImage.sprite = request.GetFace();
        cardBack.SetActive(true);
    }

    void OnPizzaEnd(PizzaRequest request)
    {
        cardBack.SetActive(false);
    }
}
