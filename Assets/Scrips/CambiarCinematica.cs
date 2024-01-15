using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class CambiarCinematica : MonoBehaviour
{
    [SerializeField] private int sceneName;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Eventos.eve.PasarNivel?.Invoke(sceneName);
        }
    }
}

