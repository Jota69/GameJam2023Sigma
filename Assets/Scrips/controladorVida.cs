using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class controladorVida : MonoBehaviour
{
    [SerializeField] private int vida;
    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = vida;
        slider.value = vida;
    }
    private void Update()
    {
        if (vida <= 0) 
        {
            ///////// (cambio escena) //////
        }
    }

    private void OnEnable()
    {
        Eventos.eve.perderVida.AddListener(quitarVida);
    }
    private void OnDisable()
    {
        Eventos.eve.perderVida.RemoveListener(quitarVida);
    }
    private void quitarVida() 
    {
        slider.value--;
    }
}
