using Cinemachine;
using UnityEngine;

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

    public float gapDistance = 5.0f;


    private void Start()
    {
        player2.tag = "Untagged";
        Material materialP2 = p2.GetComponent<Renderer>().material;
        Color colorP2 = materialP2.color;
        colorP2.a = 0.5f;
        materialP2.color = colorP2;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // Verificar si el personaje actual está en "modo ocio" antes de permitir el cambio
            if ((player1Active && player1.IsIdle) || (!player1Active && player2.IsIdle))
        {
            SwitchPlayer();
        }
        }


        // Mantener el desplazamiento constante entre los personajes
        if (player1Active)
        {

            Debug.Log("SwitchPlayer - Player 1 isIdle: " + player1.IsIdle);
            Vector3 targetPosition = Player1.position - Player1.forward * gapDistance;
            Player2.position = targetPosition;
        }
        else
        {
            Debug.Log("SwitchPlayer - Player 2 isIdle: " + player2.IsIdle);
            Vector3 targetPosition = Player2.position - Player2.forward * gapDistance;
            Player1.position = targetPosition;
        }

    }

    public void SwitchPlayer()
    {
        //if (player1Active)
        //{
        //    Eventos.eve.PausarPlayer1.Invoke();
        //    Eventos.eve.DespausarPlayer2.Invoke();
        //    player1Active = false;
        //    virtualCamera.Follow = Player2; // Cambiar el objetivo de la cámara al personaje 2
        //}
        //else
        //{
        //    Eventos.eve.DespausarPlayer1.Invoke();
        //    Eventos.eve.PausarPlayer2.Invoke();
        //    player1Active = true;
        //    virtualCamera.Follow = Player1; // Cambiar el objetivo de la cámara al personaje 1
        //}

        if (player1Active)
        {
            player1.isActive = false;
            player1.tag = "Untagged";

            player2.isActive = true;
            player2.tag = "Player";

            player2.enabled = true;
            player1Active = false;
            virtualCamera.Follow = Player2; // Cambiar el objetivo de la cámara al personaje 2


            Material materialP1 = p1.GetComponent<Renderer>().material;
            Color colorP1 = materialP1.color;
            colorP1.a = 0.25f;
            materialP1.color = colorP1;

            Material materialP2 = p2.GetComponent<Renderer>().material;
            Color colorP2 = materialP2.color;
            colorP2.a = 1.0f;
            materialP2.color = colorP2;

        }
        else
        {
            player1.isActive = true;
            player1.tag = "Player";
            
            player2.isActive = false;
            player2.tag = "Untagged";
            player1.enabled = true;
            
            player1Active = true;
            virtualCamera.Follow = Player1; // Cambiar el objetivo de la cámara al personaje 1

            Material materialP2 = p2.GetComponent<Renderer>().material;
            Color colorP2 = materialP2.color;
            colorP2.a = 0.25f;
            materialP2.color = colorP2;

            Material materialP1 = p1.GetComponent<Renderer>().material;
            Color colorP1 = materialP1.color;
            colorP1.a = 1.0f;
            materialP1.color = colorP1;


            // Teletransportar al personaje inactivo (pj1) al lugar correcto con el desfase
            Vector3 targetPosition = Player2.position - Player2.forward * gapDistance;
            Player1.position = targetPosition;
            Player1.rotation = Player2.rotation;
        }

    }
}
