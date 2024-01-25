using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    private Mapa _myInput;
    public Vector2 moveVector = Vector2.zero;
    private Rigidbody2D rb;
    public Animator animator;
    //private CapsuleCollider2D collider;
    private CapsuleCollider2D[] colliders; 
    public bool isGrounded;
    //private float TiempoEntreAtaque;
    private float TiempoSiguienteAtaque;
    public bool isActive;
    public bool pausePlayer;
    [SerializeField] private int vida=10;
    [SerializeField] private AnimationClip atrackClip;
    //[SerializeField] private Transform controladorGolpe;
    [SerializeField] private float radioGolpe;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float raycastDistance = 1f;
    [SerializeField] private float jumpForce;
    [SerializeField] private float speed;
    //[SerializeField] private int dañoGolpe;
    //[SerializeField] private PlayerInput;
    private bool isIdle = true;
    public bool IsIdle
    {
        get { return isIdle; }
        set { isIdle = value; }
    }



    private void Awake()
    {
        colliders = GetComponents<CapsuleCollider2D>();
        //colliders[0].sharedMaterial.friction = 1;
        //colliders[1].sharedMaterial.friction = 0;
        isActive = true;
        //controladorGolpe = transform.GetChild(0).GetComponent<Transform>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        //collider =GetComponent<CapsuleCollider2D>();


        //TiempoEntreAtaque = atrackClip.length;

        _myInput = new Mapa();
        _myInput.Enable();
    }

    private void OnEnable()
    {
        //Movimiento
        _myInput.Player.Movimiento.performed += OnMovementPerformed;
        _myInput.Player.Movimiento.canceled += OnMovementCancelled;

        //Otras acciones
        //_myInput.Player.Atacar.performed += OnAtackPerformed;
        _myInput.Player.Jump.performed += OnJumpPerformed;

        //Eventos
        Eventos.eve.DespausarPlayer.AddListener(DesausarPlayer);
        Eventos.eve.PausarPlayer.AddListener(PausarPlayer);
    }

    private void OnDisable()
    {
        _myInput.Player.Movimiento.performed -= OnMovementPerformed;
        _myInput.Player.Movimiento.canceled -= OnMovementCancelled;
        _myInput.Player.Jump.performed -= OnJumpPerformed;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2 (moveVector.x*speed,rb.velocity.y);
        
    }

    private void Update()
    {
        //DebugRaycast();
        isGrounded = CheckGrounded();

        bool isAlmostIdle = isGrounded && rb.velocity.magnitude < 0.1f;

        // Actualizar isIdle a true si el personaje está en reposo, de lo contrario, actualizar a false
        isIdle = isAlmostIdle;

        if (pausePlayer) 
        {
            moveVector = Vector2.zero;
        }

        if (!isGrounded)
        {
            colliders[1].enabled = true;
            colliders[0].enabled = false;

        }
        else
        {
            colliders[0].enabled = true;
            colliders[1].enabled = false;
        }

        //collider.sharedMaterial.friction = isGrounded ? 1 : 0; ;

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
            if (moveVector.x < 0f)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
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

    //private void OnAtackPerformed(InputAction.CallbackContext value)
    //{
    //    if (value.performed && TiempoSiguienteAtaque<=0)
    //    {
    //        animator.SetTrigger("Atacar");
    //        Golpe();
    //       TiempoSiguienteAtaque = TiempoEntreAtaque;
    //    } 
        
    //}



    //private void Golpe() 
    //{
    //    Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorGolpe.position, radioGolpe);
    //    foreach (Collider2D colicionador in objetos) {
    //        if (colicionador.CompareTag("Enemy"))
    //        {
    //            colicionador.transform.GetComponent<Enemigo>().ResivirDaño(dañoGolpe);
    //        }
    //    }
    //}


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
    //    Debug.DrawRay(raycastOrigin, Vector2.down * raycastDistance, Color.blue);
    //}


}
