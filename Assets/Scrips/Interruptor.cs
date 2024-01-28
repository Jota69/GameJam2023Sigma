using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interruptor : MonoBehaviour
{

    [SerializeField] private int id;
    [SerializeField] private bool presionMantenida;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")|| collision.CompareTag("ObInteract"))
        {
            Eventos.eve.moverPlataforma?.Invoke(id);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.CompareTag("Player")|| collision.CompareTag("ObInteract")) &&presionMantenida)
        {
            Eventos.eve.moverPlataforma?.Invoke(id);
        }
    }

}
