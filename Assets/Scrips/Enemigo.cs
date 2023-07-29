using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemigo : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator animator;
    private Collider2D col;
    private bool muerto;

    private Vector3 vectorPosicionRaycast;
    [SerializeField] private int vida;
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private float distancia;
    [SerializeField] private LayerMask plataforma;

    void Start()
    {
        
        muerto = false;
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }


    private void Update()
    {
        vectorPosicionRaycast = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        rb.velocity = new Vector2(velocidadMovimiento * transform.right.x, rb.velocity.y);

        RaycastHit2D informacionSuelo = Physics2D.Raycast(vectorPosicionRaycast,transform.right, distancia, plataforma);

        if (informacionSuelo)
        {
            Girar();
        }

    }


    private void Girar()
    {
        transform.eulerAngles = new Vector3(0,transform.eulerAngles.y+180,0);
    }

    public void ResivirDaño(int daño)
    {
        

        if (vida <= 0 && !muerto)
        {
            muerto=true;
            Eventos.eve.enemigoMuertoCount.Invoke();
            animator.SetTrigger("Dead");
            StartCoroutine(EsperarAnimacionMuerte());
        }
        vida -= daño;

    }

    private IEnumerator EsperarAnimacionMuerte()
    {
        AnimationClip animacionMuerteClip = animator.GetCurrentAnimatorClipInfo(0)[0].clip;
        float duracionAnimacion = animacionMuerteClip.length;

        yield return new WaitForSeconds(duracionAnimacion);

        Destroy(gameObject);
    }



    //private void OnDrawGizmos()
    //{
        
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(vectorPosicionRaycast, vectorPosicionRaycast + transform.right * distancia);
    //}

}
