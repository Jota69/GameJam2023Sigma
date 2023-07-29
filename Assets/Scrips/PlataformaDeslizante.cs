using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlataformaDeslizante : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private bool iniciaActivado = false;
    [SerializeField] private int idPlataforma;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (iniciaActivado)
        {
            animator.SetBool("deslizar", true);
        }
    }

    private void ActivarPlataforma(int idResiver)
    {
        if (idResiver==idPlataforma)
        if (animator.GetBool("deslizar") == true) 
        {
            animator.SetBool("deslizar", false);
            idPlataforma++;

        }
        else 
        {
            animator.SetBool("deslizar", true);
            idPlataforma++;
            }
    }

    private void OnEnable()
    {
        Eventos.eve.moverPlataforma.AddListener(ActivarPlataforma);
    }
}
