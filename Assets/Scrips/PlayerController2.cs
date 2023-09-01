using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController2 : MonoBehaviour
{
    private Mapa _myInput;
    public Vector2 moveVector = Vector2.zero;
    private Rigidbody2D rb;
    public Animator animator;
    public bool isGrounded;
    private float TiempoEntreAtaque;
    private float TiempoSiguienteAtaque;
    public bool isActive;
    private bool pausePlayer;

    [SerializeField] private AnimationClip atrackClip;
    [SerializeField] private Transform controladorGolpe;
    [SerializeField] private float radioGolpe;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float raycastDistance = 1f;
    [SerializeField] private float raycastDistance2 = 1f;
    [SerializeField] private float jumpForce;
    [SerializeField] private float speed;
    [SerializeField] private int dañoGolpe;
    private bool isIdle = true;
    public bool IsIdle
    {
        get { return isIdle; }
        set { isIdle = value; }
    }


    private void Awake()
    {
        isActive = false;
        controladorGolpe = transform.GetChild(0).GetComponent<Transform>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        TiempoEntreAtaque = atrackClip.length;

        _myInput = new Mapa();
        _myInput.Enable();


    }

    private void OnEnable()
    {
        //Movimiento
        _myInput.Player.Movimiento.performed += OnMovementPerformed;
        _myInput.Player.Movimiento.canceled += OnMovementCancelled;

        //Otras acciones
        _myInput.Player.Atacar.performed += OnAtackPerformed;
        _myInput.Player.Jump.performed += OnJumpPerformed;

        //Eventos
        Eventos.eve.PausarPlayer2.AddListener(PausarPlayer);
        Eventos.eve.DespausarPlayer2.AddListener(DesausarPlayer);
    }
    private void OnDisable()
    {
        _myInput.Player.Movimiento.performed -= OnMovementPerformed;
        _myInput.Player.Movimiento.canceled -= OnMovementCancelled;
        _myInput.Player.Jump.performed -= OnJumpPerformed;
        _myInput.Player.Atacar.performed -= OnAtackPerformed;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2 (moveVector.x*speed,rb.velocity.y);
        
    }

    private void Update()
    {
        //DebugRaycast();
        isGrounded = CheckGrounded();

        if (pausePlayer)
        {
            moveVector = Vector2.zero;
        }

        bool isAlmostIdle = isGrounded && rb.velocity.magnitude < 0.1f;

        // Actualizar isIdle a true si el personaje está en reposo, de lo contrario, actualizar a false
        isIdle = isAlmostIdle;


        bool isJumping = !isGrounded && rb.velocity.y > 0;
        bool isFalling = !isGrounded && rb.velocity.y < 0;

        if (isFalling || isJumping) animator.ResetTrigger("Atacar");
        
        if (TiempoSiguienteAtaque > 0) { 
            TiempoSiguienteAtaque -= Time.deltaTime;
        }

        animator.SetBool("Callendo", isFalling);
        animator.SetBool("Saltando", isJumping);
    
    }
    private void OnJumpPerformed(InputAction.CallbackContext value) {


        if (!pausePlayer)
        {
            if (isGrounded)
            {
                rb.AddForce(Vector2.up * jumpForce);
            }

            isIdle = false;
        }





    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {


        if (!pausePlayer)
        {
            moveVector = value.ReadValue<Vector2>();
            animator.SetBool("Corriendo", true);
            if ((moveVector.x < 0 && transform.rotation.y >= 0) || (moveVector.x > 0 && transform.rotation.y < 0))
            {
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
            }
            isIdle = false;
        }
        else
        {
            // Si el player está en pausa (modo ocio), no está realizando ninguna acción activa.
            // Entonces, actualiza isIdle a true.
            isIdle = true;
        }



    }

    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        moveVector = Vector2.zero;
        animator.SetBool("Corriendo", false);
  
    }

    private void OnAtackPerformed(InputAction.CallbackContext value)
    {
        if (value.performed && TiempoSiguienteAtaque<=0 && isActive)
        {
            animator.SetTrigger("Atacar");
            Golpe();
            TiempoSiguienteAtaque = TiempoEntreAtaque;
        } 
        
    }



    private void Golpe() 
    {
        Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorGolpe.position, radioGolpe);
        foreach (Collider2D colicionador in objetos) {
            if (colicionador.CompareTag("Enemy"))
            {
                colicionador.transform.GetComponent<Enemigo>().ResivirDaño(dañoGolpe);
            }
        }
    }


    private bool CheckGrounded() {
        Vector2 raycastOrigin = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, raycastDistance, groundLayer);
        return (hit.collider != null);
    }

    private void PausarPlayer()
    {
        pausePlayer = true;
    }
    private void DesausarPlayer()
    {
        pausePlayer = false;
    }


    

    //private IEnumerator EsperarAnimacionMuerte()
    //{
    //    AnimationClip animacionMuerteClip = animator.GetCurrentAnimatorClipInfo(0)[0].clip;
    //    float duracionAnimacion = animacionMuerteClip.length;

    //    yield return new WaitForSeconds(duracionAnimacion);

    //    Destroy(gameObject);
    //}

    //private void OnDrawGizmos()
    //{
    //   Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(controladorGolpe.position, radioGolpe);
    //}


    //void DebugRaycast()
    //{
    //    Vector2 raycastOrigin = transform.position;
    //    Debug.DrawRay(raycastOrigin, Vector2.down * raycastDistance2, Color.red);
    //}


}
