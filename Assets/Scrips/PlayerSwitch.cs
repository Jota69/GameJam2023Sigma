using Cinemachine;
using UnityEngine;

public class PlayerSwitch : MonoBehaviour
{

    public PlayerController2 player2;
    public bool player1Active = true;
    public Transform Player1;
    public Transform Player2;
    public CinemachineVirtualCamera virtualCamera;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SwitchPlayer();
        }
    }

    public void SwitchPlayer()
    {
        if (player1Active)
        {
            Eventos.eve.PausarPlayer1.Invoke();
            //player1.animator.enabled = false; // Desactivar el Animator o detener animaciones
            Eventos.eve.DespausarPlayer2.Invoke();
            //player2.animator.enabled = true; // Activar el Animator
            //player1.enabled = false;
            //player2.enabled = true;
            player1Active = false;
            virtualCamera.Follow = Player2; // Cambiar el objetivo de la cámara al personaje 2
        }
        else
        {
            Eventos.eve.DespausarPlayer1.Invoke();
            //player1.animator.enabled = true; // Activar el Animator
            Eventos.eve.PausarPlayer2.Invoke();
            //player2.animator.enabled = false; // Desactivar el Animator o detener animaciones
            //player1.enabled = true;
            //player2.enabled = false;
            player1Active = true;
            virtualCamera.Follow = Player1; // Cambiar el objetivo de la cámara al personaje 1
        }
    }
}
