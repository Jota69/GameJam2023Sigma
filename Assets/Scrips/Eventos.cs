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
    public UnityEvent<int> ActivarPlataforma;
    public UnityEvent<int> DesactivarPlataforma;
    public UnityEvent<int> activarCuerda;
    public UnityEvent IniciarDialogo2;
    public UnityEvent PausarPlayer;
    public UnityEvent pausarP;
    public UnityEvent PausarPlayer2;
    public UnityEvent DespausarPlayer;
    public UnityEvent DespausarP;
    public UnityEvent DespausarPlayer2;
    public UnityEvent perderVida;
    public UnityEvent<int> PasarNivel;
    public UnityEvent disparoEnemigo;





}
