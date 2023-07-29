using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ImputPlayer : MonoBehaviour
{

    private Mapa _myInput;
    public Vector2 moveVector = Vector2.zero;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private float TiempoEntreAtaque;
    private float TiempoSiguienteAtaque;


    [SerializeField] private AnimationClip atrackClip;
    [SerializeField] private Transform controladorGolpe;
    [SerializeField] private float radioGolpe;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float raycastDistance = 1f;
    [SerializeField] private float raycastDistance2 = 1f;
    [SerializeField] private float jumpForce;
    [SerializeField] private float speed;
    [SerializeField] private int dañoGolpe;


    private void Awake()
    {
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

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2 (moveVector.x*speed,rb.velocity.y);
        
    }

    private void Update()
    {
        DebugRaycast();
        isGrounded = CheckGrounded();
        
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

        if (isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce);
        }
        
        
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>();
        animator.SetBool("Corriendo",true);
        

        if ((moveVector.x < 0 && transform.rotation.y>=0) || (moveVector.x>0 && transform.rotation.y<0))
        {
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        }

    }

    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        moveVector = Vector2.zero;
        animator.SetBool("Corriendo", false);
  
    }

    private void OnAtackPerformed(InputAction.CallbackContext value)
    {
        if (value.performed && TiempoSiguienteAtaque<=0)
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


    private void OnDrawGizmos()
    {
       Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(controladorGolpe.position, radioGolpe);
    }


    void DebugRaycast()
    {
        Vector2 raycastOrigin = transform.position;
        Debug.DrawRay(raycastOrigin, Vector2.down * raycastDistance2, Color.red);
    }


}
