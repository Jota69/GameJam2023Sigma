using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmaEnemigo : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private Transform pivote;
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        foreach (var hit in enemy.GetComponent<Enemigo>().hits)
        {
            if (enemy.GetComponent<Enemigo>().playerDetectado)
            {
                Vector3 direccion = hit.bounds.center - pivote.transform.position;
                float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
                pivote.transform.rotation = Quaternion.AngleAxis(angulo, Vector3.forward);
            }
            else
            {
                pivote.transform.rotation = Quaternion.Euler(0, 0, 45);
            }
        }

    }
}
