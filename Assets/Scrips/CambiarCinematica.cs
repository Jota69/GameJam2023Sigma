using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class CambiarCinematica : MonoBehaviour
{
    // Start is called before the first frame update
    public string sceneName; // Nombre de la escena a cargar (aseg�rate de que sea exacto y est� correctamente escrito)

    private void Awake()
    {
        sceneName = "Mar";
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadScene();
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}

