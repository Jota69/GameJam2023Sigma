using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cuerda : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float tiempoEspera=1;
    [SerializeField] private bool aparecer;
    [SerializeField] private int id;
    [SerializeField] private GameObject child;
    private void Start()
    {
    }
    public void Desactivar()
    {
        StartCoroutine(Espera());
    }
    public void Activar()
    {
        
        StartCoroutine(EsperaAct());
    }
    IEnumerator Espera()
    {
        yield return new WaitForSeconds(tiempoEspera);
        this.gameObject.SetActive(false);
    }
    IEnumerator EsperaAct()
    {
        yield return new WaitForSeconds(tiempoEspera);
        child.SetActive(true);
    }

    private void ActivarPlataforma(int idResiver)
    {
        if (idResiver == id)
        {
            Activar();
        }

    }

    private void OnEnable()
    {
        Eventos.eve.activarCuerda.AddListener(ActivarPlataforma);
    }
    private void OnDisable()
    {
        Eventos.eve.activarCuerda.RemoveListener(ActivarPlataforma);
    }
}
