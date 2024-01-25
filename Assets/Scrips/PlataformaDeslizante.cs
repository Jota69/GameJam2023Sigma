using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlataformaDeslizante : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private bool iniciaActivado = false;
    [SerializeField] private int idPlataforma;
    [SerializeField] private bool idModificable = false;
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
        if (idResiver == idPlataforma)
        {
            if (animator.GetBool("deslizar") == true)
            {
                animator.SetBool("deslizar", false);
                if (idModificable)
                {
                    idPlataforma++;
                }
            }
            else
            {
                animator.SetBool("deslizar", true);
                if (idModificable)
                {
                    idPlataforma++;
                }
            }
        }
        
    }

    private void OnEnable()
    {
        Eventos.eve.moverPlataforma.AddListener(ActivarPlataforma);
    }
    private void OnDisable()
    {
        Eventos.eve.moverPlataforma.RemoveListener(ActivarPlataforma);
    }
}
