using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaDeslizante : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameObject plataformaAmover;
    [SerializeField] private bool iniciaActivado;
    [SerializeField] private int idPlataforma;

    private bool activado;

    private void Start()
    {
        
    }

    private void ActivarPlataforma(int idResiver)
    {
        if (idResiver==idPlataforma && !activado)
        if (plataformaAmover.GetComponent<Animator>().GetBool("deslizar") == true) 
        {
            plataformaAmover.GetComponent<Animator>().SetBool("deslizar", false);
            activado = true;

        }
        else 
        {
            plataformaAmover.GetComponent<Animator>().SetBool("deslizar", true);
            activado = true;
        }
    }

    private void OnEnable()
    {
        Eventos.eve.moverPlataforma.AddListener(ActivarPlataforma);
    }
}
