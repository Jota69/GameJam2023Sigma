using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cangrejo : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private Vector2 vectorPosicionRaycast;
    private bool atacando;
    private bool detected;

    [SerializeField] private int vida;
    [SerializeField] private float stunTime;

    [Header("Configuraciones de detección al jugador:")]
    [SerializeField] private float detectionRadiusFirst;
    [SerializeField] private float detectionRadiusSecond;
    [SerializeField] private float timeToDetect;
    [SerializeField] private LayerMask playerMask;

    [Header("Configuraciones de movimiento:")]
    
    [SerializeField] private float movementVelocity;
    [SerializeField] private float distanciaPared;
    [SerializeField] private Transform inicioRaycastGround;
    [SerializeField] private float raycastDistancia2;
    [SerializeField] private LayerMask queEsSuelo;

    [Header("Configuraciones de ataque:")]
    [SerializeField] private AnimationClip detectClip;
    [SerializeField] private float atackVelocity;
    [SerializeField] private float atackTime;

    IEnumerator timer;
    private bool alerted = false;
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //DebugRaycast();
        isGrounded = CheckGrounded();
        RaycastHit2D informacionSuelo = Physics2D.Raycast(vectorPosicionRaycast, transform.right, distanciaPared, queEsSuelo);
        vectorPosicionRaycast = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        RaycastHit2D[] raycastHits = Physics2D.RaycastAll(vectorPosicionRaycast, transform.right, detectionRadiusFirst);
        foreach (var hit in raycastHits)
        {
            if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Player1") || hit.collider.gameObject.layer == LayerMask.NameToLayer("Player2"))
            {     
                if (!alerted)
                {
                    //Animator.setBool("Alerta",true);
                    StartCoroutine(Detect());
                }
                if (detected&&!atacando)
                {
                    atacando = true;
                    alerted = true;
                    detectionRadiusFirst = detectionRadiusSecond;
                }
                break;
            }
            else if( hit.collider.gameObject.layer == LayerMask.NameToLayer("Terreno") )
            {
                break;
            }
        }
        if (atacando)
        {
            rb.AddForce(transform.right * atackVelocity, ForceMode2D.Force);
            timer = AddForceDuring();
            StartCoroutine(timer);
        }
        if (!atacando&&alerted)
        {
            rb.velocity = new Vector2(movementVelocity * transform.right.x, rb.velocity.y);
            animator.SetBool("caminando", true);
            
        }
        if (informacionSuelo || !isGrounded)
        {
            Girar();
        }
        if (vida <= 0)
        {
            Destroy(gameObject);
        }
    }
    IEnumerator AddForceDuring()
    {
        yield return new WaitForSeconds(atackTime);
        atacando = false;
    }

    IEnumerator Detect()
    {
        yield return new WaitForSeconds(timeToDetect /*detectClip.length+1f*/);
        detected= true;
    }


    private void Girar()
    {
        if (transform.eulerAngles.y == 0)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

    }

    public void ResivirDaño(int daño)
    {
        vida -= daño;
        StopAllCoroutines();
        detected = false;
        alerted = false;
        StartCoroutine(WaitDamage());
    }
    IEnumerator WaitDamage()
    {
        yield return new WaitForSeconds(stunTime);
        detected = true;
        alerted= true;
    }

    private bool CheckGrounded()
    {
        Vector2 raycastOrigin = inicioRaycastGround.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, raycastDistancia2, queEsSuelo);
        return (hit.collider != null);
    }

    //void DebugRaycast()
    //{
    //    Vector2 raycastOrigin = inicioRaycastGround.transform.position;
    //    Debug.DrawRay(raycastOrigin, Vector2.down* raycastDistancia2, Color.blue);
    //}
}
