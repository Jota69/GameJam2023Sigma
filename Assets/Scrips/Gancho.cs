using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gancho : MonoBehaviour
{
    public float ganchoDistancia = 10f; // Distancia máxima del gancho
    public float ganchoVelocidad = 20f; // Velocidad de lanzamiento del gancho
    public LayerMask Terreno;   // Capa de terreno para detectar colisiones

    private bool enganchado = false;
    private Rigidbody2D rb;
    private DistanceJoint2D distanceJoint;




    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        distanceJoint = GetComponent<DistanceJoint2D>();
        distanceJoint.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Clic izquierdo detectado");
            LanzarGancho();
        }
        else if (Input.GetMouseButtonUp(0) && enganchado)
        {
            DejarGancho();
        }
    }

    void LanzarGancho()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 ganchoDir = (mousePos - (Vector2)transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, ganchoDir, ganchoDistancia, Terreno);

        // verificar pos de mouse
        Debug.Log("Mouse Pos: " + mousePos);
        Debug.Log("Gancho Dir: " + ganchoDir);
        Debug.Log("Hit Point: " + hit.point);

        if (hit.collider != null)
        {
            enganchado = true;
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0f;
            distanceJoint.connectedAnchor = hit.point;
            distanceJoint.distance = Vector2.Distance(transform.position, hit.point);
            distanceJoint.enabled = true;

        }
    }

    void DejarGancho()
    {
        enganchado = false;
        rb.gravityScale = 1f;
        distanceJoint.enabled = false;
    }

 



}

