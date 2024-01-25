using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caja : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private int dañoCaida=10;
    [SerializeField] private float speedToDoDamage=7;
    float cont;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (rb.velocityY <= 0)
        {
            cont = rb.velocityY;
        }
        else
        {
            StartCoroutine(reiniciarCont());
        }
    }
    IEnumerator reiniciarCont()
    {
        yield return new WaitForSeconds(0.3f);
        cont = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.GetComponent<Enemigo>())
            {
                if (cont<=-speedToDoDamage)
                {
                    collision.gameObject.GetComponent<Enemigo>().ResivirDaño(dañoCaida);
                }
            }
        }
    }
}
