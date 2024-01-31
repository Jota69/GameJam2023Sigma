using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmaEnemigo : MonoBehaviour
{
    private Vector3 direccion;
    [SerializeField] private GameObject enemy;
    [SerializeField] private Transform pivote;
    [SerializeField] private GameObject bala;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float velocidadBala;
    private bool proyectilDisparado = false;

    void Update()
    {
        foreach (var hit in enemy.GetComponent<Enemigo>().hits)
        {
            if ((enemy.GetComponent<Enemigo>().playerDetectado && !proyectilDisparado) || enemy.GetComponent<Enemigo>().detectandoPlayer)
            {
                direccion = hit.bounds.center - pivote.transform.position;
                float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.AngleAxis(angulo, Vector3.forward);
                // Ajusta la velocidad de rotación según sea necesario
                pivote.transform.rotation = Quaternion.Lerp(pivote.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
            else
            {
                pivote.transform.rotation = Quaternion.Lerp(pivote.transform.rotation, enemy.transform.rotation, Time.deltaTime * rotationSpeed);
            }
        }
    }

    private void Shoot()
    {
        var balaa = Instantiate(bala, transform.position, pivote.transform.rotation);
        balaa.GetComponent<Proyectil>().SetDireccion(direccion,velocidadBala); // Pasa la dirección al proyectil
        balaa.transform.parent = null; // Desvincula el proyectil del objeto padre
        Destroy(balaa, 2);
    }

    private void OnEnable()
    {
        Eventos.eve.disparoEnemigo.AddListener(Shoot);
    }
    private void OnDisable()
    {
        Eventos.eve.disparoEnemigo.RemoveListener(Shoot);
    }
}
