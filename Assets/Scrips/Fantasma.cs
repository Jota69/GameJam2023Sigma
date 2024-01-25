using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fantasma : MonoBehaviour
{
    [SerializeField] public int daño;
    [SerializeField] private int vida=30;
    [SerializeField] private float speed = 1;
    [SerializeField] private float detectionRange = 10.0f;
    [SerializeField] private float chargeDistance = 5.0f;
    [SerializeField] private float stunTime = 1;
    private Transform player;
    private Vector3 targetPosition;
    private bool isCharging = false;
    [SerializeField] private bool resibiendoDano;

    void Start()
    {
        resibiendoDano = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if(!resibiendoDano)
        {
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
        else
        {
            Vector3 oppositeDirection = transform.position - player.position;
            
            if (!isCharging)
            {
                targetPosition = transform.position + oppositeDirection.normalized * (chargeDistance);
                isCharging = true;
            }
            if (Vector3.Distance(transform.position, targetPosition) <= 1f)
            {
                isCharging = false;
                resibiendoDano = false;
            }
            transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
        }
        if (vida <= 0)
        {
            Destroy(gameObject);
        }

    }

    public void ResivirDaño(int daño)
    {
        vida -= daño;
        resibiendoDano = true;
        isCharging = false;
    }
}
