using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controladorVida : MonoBehaviour
{
    [SerializeField] private int vida;

    private void Update()
    {
        if (vida == 0 && vida <= 0) 
        {
            ///////// (cambio escena) //////
        }
    }

    private void OnEnable()
    {
        Eventos.eve.perderVida.AddListener(quitarVida);
    }
    private void quitarVida() 
    {
        vida--;
    }
}
