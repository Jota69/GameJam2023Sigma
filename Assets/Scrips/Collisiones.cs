using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisiones : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) 
        {
            if (collision.GetComponent<Fantasma>())
            {
                Eventos.eve.perderVida.Invoke();
            }
            if (collision.GetComponent<Cangrejo>())
            {
                Eventos.eve.perderVida.Invoke();
            }
        }
        if (collision.CompareTag("Bala"))
        {
            if (collision.GetComponent<Proyectil>())
            {
                Eventos.eve.perderVida.Invoke();
            }
        }
    }


}
