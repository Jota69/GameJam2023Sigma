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


    private void Start()
    {
        Material materialP2 = p2.GetComponent<Renderer>().material;
        Color colorP2 = materialP2.color;
        colorP2.a = 0.5f;
        materialP2.color = colorP2;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SwitchPlayer();
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
            
            player2.isActive = true;
           
           
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

            
            player2.isActive = false;
            
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
        }

    }
}
