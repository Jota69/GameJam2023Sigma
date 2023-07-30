using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenúInGame : MonoBehaviour
{
    public GameObject MenuPausa;

    public void menuPausa()
    {
        Time.timeScale = 0;
        MenuPausa.SetActive(true);

    }

    public void Return()
    {
        Time.timeScale = 1;
        MenuPausa.SetActive(false);

    }
     
     
    public void SalirAlMenu()
    {

        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

    }
}
