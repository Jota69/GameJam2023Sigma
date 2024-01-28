using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectil : MonoBehaviour
{
    private Vector3 direccion;
    private float velocidadBala;

    public void SetDireccion(Vector3 dir,float vel)
    {
        direccion = dir;
        velocidadBala = vel;
    }

    void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(direccion* velocidadBala, ForceMode2D.Impulse);
        GetComponent<Transform>().rotation = Quaternion.Euler(direccion.x,0,direccion.z);
    }
}
