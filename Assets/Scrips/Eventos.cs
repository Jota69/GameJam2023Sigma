using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Eventos : MonoBehaviour
{
    // Start is called before the first frame update
    public static Eventos eve;

    private void Awake()
    { 
        if (eve != null && eve != this)
        {
            Destroy(this.gameObject);

        }
        else
        {
            eve = this;

        }
    }

    public UnityEvent enemigoMuertoCount;
    public UnityEvent<int> moverPlataforma;
    public UnityEvent IniciarDialogo2;
    public UnityEvent PausarPlayer1;
    public UnityEvent PausarPlayer2;
    public UnityEvent DespausarPlayer1;
    public UnityEvent DespausarPlayer2;
    public UnityEvent perderVida;





}
