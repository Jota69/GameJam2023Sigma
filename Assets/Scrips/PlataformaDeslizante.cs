using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaDeslizante : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameObject plataformaAmover;
    [SerializeField] private bool activado;
    [SerializeField] private int idPlataforma;

    private void ActivarPlataforma(int idResiver)
    {
        if (idResiver==idPlataforma)
        if (plataformaAmover.GetComponent<Animator>().GetBool("deslizar") == true) 
        {
            plataformaAmover.GetComponent<Animator>().SetBool("deslizar", false);
        }
        else 
        {
            plataformaAmover.GetComponent<Animator>().SetBool("deslizar", true);
        }
    }

    private void OnEnable()
    {
        Eventos.eve.moverPlataforma.AddListener(ActivarPlataforma);
    }
}
