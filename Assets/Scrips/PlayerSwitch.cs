using UnityEngine;

public class PlayerSwitch : MonoBehaviour
{
    public PlayerController player1;
    public PlayerController2 player2;
    public bool player1Active = true;

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
            player1.isActive = false;
            player1.animator.enabled = false; // Desactivar el Animator o detener animaciones
            player2.isActive = true;
            player2.animator.enabled = true; // Activar el Animator
            player1.enabled = false;
            player2.enabled = true;
            player1Active = false;
        }
        else
        {
            player1.isActive = true;
            player1.animator.enabled = true; // Activar el Animator
            player2.isActive = false;
            player2.animator.enabled = false; // Desactivar el Animator o detener animaciones
            player1.enabled = true;
            player2.enabled = false;
            player1Active = true;
        }
    }
}
