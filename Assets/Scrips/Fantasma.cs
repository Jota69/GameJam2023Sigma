using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fantasma : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f; // Velocidad del enemigo
    [SerializeField] private float detectionRange = 10.0f; // Rango de detección del jugador
    [SerializeField] private float chargeDistance = 5.0f; // Distancia que el enemigo cargará

    private Transform player; // Referencia al jugador
    private Vector3 targetPosition; // Posición objetivo a la que se moverá el enemigo
    private bool isCharging = false; // Indica si el enemigo está cargando

    void Start()
    {
        // Encuentra al jugador en la escena
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // Calcula la distancia al jugador
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Si el jugador está dentro del rango de detección y el enemigo no está cargando
        if (distanceToPlayer <= detectionRange && !isCharging)
        {
            // Establece la posición objetivo en la dirección del jugador y marca al enemigo como cargando
            targetPosition = player.position + (player.position - transform.position).normalized * chargeDistance;
            isCharging = true;
        }

        // Si el enemigo ha alcanzado la posición objetivo, deja de cargar
        if (Vector3.Distance(transform.position, targetPosition) <= 0.1f)
        {
            isCharging = false;
        }

        // Mueve al enemigo hacia la posición objetivo
        transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
    }
}
