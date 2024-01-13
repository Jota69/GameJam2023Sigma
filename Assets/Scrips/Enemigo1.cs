using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.InputSystem.Controls.AxisControl;

public class Enemigo : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator animator;
    private Collider2D col;
    private bool muerto;
    private bool isGrounded;
    private bool atacando;
    private bool golpeEjecutado = false;

    private Vector3 vectorPosicionRaycast;
    [SerializeField] private int vida;

    [Header("Configuraciones de movimiento:")]
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private float distanciaPared;
    [SerializeField] private float raycastDistancia2;
    [SerializeField] private Transform inicioRaycastGround;
    [SerializeField] private LayerMask queEsSuelo;

    [Header("Configuraciones de detección al jugador:")]
    [SerializeField] private float detectionRadius;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private AnimationClip AnimacionAtaque;

    [Header("configuraciones de enemigos cuerpo a cuerpo:")]
    [SerializeField] private Transform controladorGolpe;
    [SerializeField] private int dañoGolpe;
    [SerializeField] private float radioGolpe;

    [Header("Tipo de enemigo:")]
    [SerializeField] private bool isMobile;
    [SerializeField] private bool isCac;//cuerpo a cuerpo


    void Start()
    {
        atacando = false;
        muerto = false;
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }


    private void Update()
    {
        DebugRaycast();

        if (isMobile)
        {
            isGrounded = CheckGrounded();
            if (!atacando)
            {
                vectorPosicionRaycast = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
                rb.velocity = new Vector2(velocidadMovimiento * transform.right.x, rb.velocity.y);
            }
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius, playerMask);
            RaycastHit2D informacionSuelo = Physics2D.Raycast(vectorPosicionRaycast, transform.right, distanciaPared, queEsSuelo);
            RaycastHit2D informacionPlayerCac = Physics2D.Raycast(vectorPosicionRaycast, transform.right, distanciaPared, playerMask);

            foreach (var hit in hits)
            {
                Vector3 directionToPlayer = (hit.transform.position - transform.position).normalized;
                if (!Physics2D.Raycast(transform.position, directionToPlayer, detectionRadius, queEsSuelo))
                {
                    atacar();
                    Debug.Log("El jugador ha sido detectado");
                }

                
            }
            if (informacionPlayerCac && isCac)

            {
                atacar();
            }

            if (informacionSuelo || !isGrounded)
            {
                Girar();
            }

        }
    }

    private bool CheckGrounded()
    {
        Vector2 raycastOrigin = inicioRaycastGround.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, raycastDistancia2, queEsSuelo);
        return (hit.collider != null);
    }

    private void atacar()
    {
        
        atacando = true;

        StartCoroutine(ataque());
        
    }

    private IEnumerator ataque()
    {
        //animator.SetBool("ataque",true);
        yield return new WaitForSeconds(3f /*AnimacionAtaque.length*/);

        if (!golpeEjecutado) // Verificar nuevamente antes de ejecutar el golpe
        {
            golpeEjecutado = true;
            Golpe();
            ; // Marcar el golpe como ejecutado
        }
       
        atacando = false;
        yield return new WaitForSeconds(1f);
        golpeEjecutado = false;
    }

    private void Golpe()
    {
        Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorGolpe.position, radioGolpe);
        foreach (Collider2D colicionador in objetos)
        {
            if (colicionador.CompareTag("Player"))
            {
                Eventos.eve.perderVida.Invoke();
            }
        }
    }
    

    private void Girar()
    {
        transform.eulerAngles = new Vector3(0,transform.eulerAngles.y+180,0);
    }

    public void ResivirDaño(int daño)
    {
        

        if (vida <= 0)
        {
            Destroy(gameObject);
            
        }
        vida -= daño;

    }



    void DebugRaycast()
    {
        Vector2 raycastOrigin = inicioRaycastGround.transform.position;
        Debug.DrawRay(raycastOrigin, Vector2.down * raycastDistancia2, Color.red);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    //private void OnDrawGizmos()
    //{

    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(vectorPosicionRaycast, vectorPosicionRaycast + transform.right * distanciaPared);
    //}

}
