using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem : MonoBehaviour
{
    [SerializeField] private int daño=5;
    [SerializeField] private bool reusable = true;
    [Header("Si no es reusable:")]
    [SerializeField] private int usos = 1;
    private void Update()
    {
        if (!reusable)
        {
            if (usos <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {

            if (collision.GetComponent<Fantasma>() != null)
            {
                collision.GetComponent<Fantasma>().ResivirDaño(daño);
                if (!reusable)
                {
                    usos -= 1;
                }
            }
        }
    }
}
