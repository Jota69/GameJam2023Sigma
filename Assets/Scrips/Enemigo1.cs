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
    private bool muerto;
    private bool isGrounded;
    private bool atacando;
    private bool detectandoPlayer;
    public bool playerDetectado;
    private bool modoAlerta;
    bool parado;
    private bool golpeEjecutado = false;
    public Collider2D[] hits;
    private bool modoAtaque;

    private Vector3 vectorPosicionRaycast;
    [SerializeField] private int vida;

    [Header("Configuraciones de movimiento:")]
    [SerializeField] private LayerMask otrosEnemigos;
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private float distanciaPared;
    [SerializeField] private float raycastDistancia2;
    [SerializeField] private Transform inicioRaycastGround;
    [SerializeField] private LayerMask queEsSuelo;

    [Header("Configuraciones de detección al jugador:")]
    [SerializeField] private float detectionRadius;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private AnimationClip AnimacionAtaque;
    [SerializeField] private float tEsperaAtaque;

    [Header("configuraciones de enemigos cuerpo a cuerpo:")]
    [SerializeField] private Transform controladorGolpe;
    [SerializeField] private float radioGolpe;

    [Header("Tipo de enemigo:")]
    [SerializeField] private bool isMobile;
    [SerializeField] private bool isCac;//cuerpo a cuerpo
    [SerializeField] private GameObject arma;

    [Header("Configuraciones de ataque:")]
    [SerializeField] private int dañoGolpe;
    [SerializeField] private float tEntreAtaques;
    [SerializeField] private float prueba;



    void Start()
    {
        atacando = false;
        muerto = false;
        parado = false;
        detectandoPlayer = false;
        modoAlerta = false;
        playerDetectado = false;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        modoAtaque = false;
    }


    private void Update()
    {
        vectorPosicionRaycast = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        isGrounded = CheckGrounded();
        hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius, playerMask);
        RaycastHit2D informacionSuelo = Physics2D.Raycast(vectorPosicionRaycast, transform.right, distanciaPared, queEsSuelo);
        RaycastHit2D informacionPlayerCac = Physics2D.Raycast(vectorPosicionRaycast, transform.right, distanciaPared, playerMask);
        //ENEMIGOS PATRULLEROS A DISTANCIA
        if (!isCac) {
            arma.SetActive(true);
            foreach (var hit in hits)
            {
                Vector3 directionToPlayer = (hit.transform.position - transform.position).normalized;
                if (!Physics2D.Raycast(transform.position, directionToPlayer, detectionRadius, queEsSuelo)) //No hay nada entre el jugador y el enemigo
                {
                    //Girar hacia el player
                    float angulo=0;
                    if (directionToPlayer.x > 0)
                    {
                        angulo = 0; // Rota hacia la derecha
                    }
                    else if (directionToPlayer.x < 0)
                    {
                        angulo = 180; // Rota hacia la izquierda
                    }
                    transform.rotation = Quaternion.AngleAxis(angulo, Vector3.up);
                    ////////////////////////////////////////////////////////////////

                    if (!detectandoPlayer && modoAtaque) { modoAtaque = false; }
                    parado = true;

                    if (!playerDetectado&&!detectandoPlayer&&!modoAtaque)
                    {
                        detectar();
                    }
                    if (playerDetectado)
                    {
                        if (hit.CompareTag("Player") && !atacando)
                        {
                            atacar();
                        }
                    }
                }
                else if((!parado&&!detectandoPlayer&&modoAlerta||playerDetectado) && isMobile)
                {
                    rb.velocity = new Vector2(velocidadMovimiento*0.2f * transform.right.x, rb.velocity.y);
                }
            }
            if (hits.Length == 0) 
            { 
                parado = false;
                modoAlerta = false;
                modoAtaque = false;
                playerDetectado = false;
            }
        }
        else
        {
            arma.SetActive(false);
        }
            /////////////////////////////////////////
            
            //ENEMIGOS PATRULLEROS CUERPO A CUERPO
        if (informacionPlayerCac && isCac && !atacando)
        {
            parado = true;
            atacar();
        }
        if (!parado&&isMobile&&!detectandoPlayer&&!modoAlerta)
        {
            rb.velocity = new Vector2(velocidadMovimiento * transform.right.x, rb.velocity.y);
        }
        if (informacionSuelo || !isGrounded)
        {
            Girar();
        }
    }

    private bool CheckGrounded()
    {
        Vector2 raycastOrigin = inicioRaycastGround.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, raycastDistancia2, queEsSuelo);
        return (hit.collider != null);
    }

    private bool detectar()
    {
        detectandoPlayer = true;
        StartCoroutine(detectado());
        return playerDetectado;
    }
    private IEnumerator detectado()
    {
        yield return new WaitForSeconds(tEsperaAtaque);
        foreach (var hit in hits)
        {
            Vector3 directionToPlayer = (hit.transform.position - transform.position).normalized;

            if (!Physics2D.Raycast(transform.position, directionToPlayer, detectionRadius, queEsSuelo))
            {
                if (hit.CompareTag("Player") && !atacando)
                {
                    playerDetectado = true;
                    modoAtaque = true;
                }
                //activa el modo ataque tras detectar al jugador (no saldrá del modo ataque hasta que el player salga de su rango de alcance o se esconda)
            }
            else 
            {
                parado = false;
                modoAlerta = true;
                playerDetectado = false;
            }
        }
        detectandoPlayer = false;

    }


    private void atacar()
    {
        atacando = true;
        StartCoroutine(ataque());
    }

    private IEnumerator ataque()
    {
        //animator.SetBool("ataque",true);
        yield return new WaitForSeconds(0.5f /*AnimacionAtaque.length*/);

        if (!golpeEjecutado) // Verificar nuevamente antes de ejecutar el golpe
        {
            golpeEjecutado = true;
            Golpe();
            Debug.Log("Atacó");
            // Marcar el golpe como ejecutado
        }
        yield return new WaitForSeconds(tEntreAtaques);
        if (isCac)
        {
            parado = false;
        }
        golpeEjecutado = false;
        atacando = false;
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



    //void DebugRaycast()
    //{
    //    Vector2 raycastOrigin = inicioRaycastGround.transform.position;
    //    Debug.DrawRay(raycastOrigin, Vector2.down * raycastDistancia2, Color.red);
    //}
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, detectionRadius);
    //}

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawLine(vectorPosicionRaycast, vectorPosicionRaycast + transform.right * distanciaPared);
    }

}
