using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenúInGame : MonoBehaviour
{
    public GameObject MenuPausa;
    public GameObject controlsMenu;
    public GameObject[] elementosInGame;
    public GameObject[] elementosInMenu;
    public GameObject[] elementosMenu;

    public void menuPausa()
    {
        Time.timeScale = 0;
        foreach (GameObject elemento in elementosInGame)
        {
            if (elemento != null)
            {
                elemento.SetActive(false);
            }
        }
        MenuPausa.SetActive(true);
        for (int i = 0; i < elementosInMenu.Length; i++)
        {
            if (i == 0)
            {
                elementosInMenu[i].SetActive(true);
            }
            else
            {
                elementosInMenu[i].SetActive(false);
            }
        }

    }
    public void controlsSettings()
    {
        elementosMenu[0].SetActive(true);
        elementosMenu[1].SetActive(false);
        for (int i = 0; i < elementosInMenu.Length; i++)
        {
            if(i == 3)
            {
                elementosInMenu[i].SetActive(true);
            }
            else
            {
                elementosInMenu[i].SetActive(false);
            }
        }
        
    }
    public void GeneralSettings()
    {
        elementosMenu[0].SetActive(false);
        elementosMenu[1].SetActive(true);
        for (int i = 0; i < elementosInMenu.Length; i++)
        {
            if (i == 1)
            {
                elementosInMenu[i].SetActive(true);
            }
            else
            {
                elementosInMenu[i].SetActive(false);
            }
        }
    }

    public void Return()
    {
        Time.timeScale = 1;
        MenuPausa.SetActive(false);
        foreach (GameObject elemento in elementosInGame)
        {
            if (elemento != null)
            {
                elemento.SetActive(true);
            }
        }
        foreach(GameObject elemento in elementosMenu)
        {
            if (elemento != null)
            {
                elemento.SetActive(false);
            }
        }
    }
     
     
    public void SalirAlMenu()
    {
        Time.timeScale = 1;
        Eventos.eve.PasarNivel?.Invoke(1);

    }
}
