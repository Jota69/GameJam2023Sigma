using Cinemachine;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerSwitch : MonoBehaviour
{
    public PlayerController player1;
    public PlayerController2 player2;
    public bool player1Active = true;
    public Transform Player1;
    public Transform Player2;
    public CinemachineVirtualCamera virtualCamera;

    public GameObject p1;
    public GameObject p2;
    public Material skin;


    public bool isGrounded;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float raycastDistance = 1f;

    private void Start()
    {
        p1.GetComponent<SpriteRenderer>().sortingOrder = 1;
        player2.tag = "Untagged";
        Material materialP2 = p2.GetComponent<Renderer>().material;
        Color colorP2 = materialP2.color;
        colorP2.a = 0.25f;
        materialP2.color = colorP2;
    }

    void Update()
    {
        isGrounded = CheckGrounded();
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // Verificar si el personaje actual está en "modo ocio" antes de permitir el cambio
            if ((player1.isGrounded) || (player2.isGrounded))
            {
                SwitchPlayer();
            }
        }


        // Mantener el desplazamiento constante entre los personajes
        if (player1Active)
        {

            Vector3 targetPosition = Player1.position;
            Player2.position = targetPosition;
        }
        else
        {
            Vector3 targetPosition = Player2.position;
            Player1.position = targetPosition;
        }

    }

    private bool CheckGrounded()
    {
        Vector2 raycastOrigin = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, raycastDistance, groundLayer);
        return (hit.collider != null);
    }

    public void SwitchPlayer()
    {
        if (player1Active)
        {
            virtualCamera.Follow = Player2; // Cambiar el objetivo de la cámara al personaje 2

        }
        else
        {
            virtualCamera.Follow = Player1; // Cambiar el objetivo de la cámara al personaje 1
        }
         

        if (player1Active)
        {
            player2.enabled = true;
            //p2.GetComponent<CapsuleCollider2D>().enabled = true;
            player2.isActive = true;
            player2.tag = "Player";
            p2.GetComponent<SpriteRenderer>().sortingOrder = 1;


            


            Material materialP1 = p1.GetComponent<Renderer>().material;
            Color colorP1 = materialP1.color;
            colorP1.a = 0.25f;
            materialP1.color = colorP1;

            Material materialP2 = p2.GetComponent<Renderer>().material;
            Color colorP2 = materialP2.color;
            colorP2.a = 1.0f;
            materialP2.color = colorP2;
            player1Active = false;
            player1.isActive = false;
            player1.tag = "Untagged";
            p1.GetComponent<SpriteRenderer>().sortingOrder = 0;
            //p1.GetComponent<CapsuleCollider2D>().enabled = false;
        }
        else
        {
            player1.enabled = true;
            //p1.GetComponent<CapsuleCollider2D>().enabled = true;
            player1Active = true;
            player1.isActive = true;
            player1.tag = "Player";
            p1.GetComponent<SpriteRenderer>().sortingOrder = 1;

            

         

            Material materialP2 = p2.GetComponent<Renderer>().material;
            Color colorP2 = materialP2.color;
            colorP2.a = 0.25f;
            materialP2.color = colorP2;

            Material materialP1 = p1.GetComponent<Renderer>().material;
            Color colorP1 = materialP1.color;
            colorP1.a = 1.0f;
            materialP1.color = colorP1;


            // Teletransportar al personaje inactivo (pj1) al lugar correcto con el desfase
            Vector3 targetPosition = Player2.position;
            Player1.position = targetPosition;
            Player1.rotation = Player2.rotation;
            player2.isActive = false;
            player2.tag = "Untagged";
            p2.GetComponent<SpriteRenderer>().sortingOrder = 0;
            //p2.GetComponent<CapsuleCollider2D>().enabled = false;
        }

    }
}
