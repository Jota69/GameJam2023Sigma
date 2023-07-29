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





}
