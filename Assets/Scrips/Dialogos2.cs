using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogos2 : MonoBehaviour
{
    public float tiempoEntreChar;
    [SerializeField, TextArea(4, 5)] private string[] lineasDialogo;
    [SerializeField] private GameObject viñeta;
    private int LineIndex;
    private bool activeDialog;
    private void Update()
    {
        if (activeDialog && Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Joystick1Button1) && activeDialog)
        {

            if (viñeta.GetComponentInChildren<TextMeshPro>().text == lineasDialogo[LineIndex])
            {
                NextDialogLine();
                
                

            }
        }
    }

    private void EmpezarDialogo()
    {
        Eventos.eve.IniciarDialogo2.RemoveListener(EmpezarDialogo);
        Eventos.eve.PausarPersonaje.Invoke();
        activeDialog = true;
        viñeta.SetActive(true);
        viñeta.GetComponent<Animator>().SetBool("abrir", true);
        LineIndex = 0;
        StartCoroutine(mostrarLinea());
    }
    private void NextDialogLine()
    {
        LineIndex++;
        if (LineIndex < lineasDialogo.Length)
        {
            StartCoroutine(mostrarLinea());
        }
        else
        {
            StartCoroutine(ocultar());
            activeDialog = false;
            Eventos.eve.DespausarPersonaje.Invoke();

        }
    }

    private void OnEnable()
    {
        Eventos.eve.IniciarDialogo2.AddListener(EmpezarDialogo);
    }

    private IEnumerator mostrarLinea()
    {
        viñeta.GetComponentInChildren<TextMeshPro>().text = string.Empty;
        foreach (char line in lineasDialogo[LineIndex])
        {
            viñeta.GetComponentInChildren<TextMeshPro>().text += line;
            yield return new WaitForSeconds(tiempoEntreChar);
        }
    }
    private IEnumerator ocultar()
    {
        viñeta.GetComponent<Animator>().SetBool("abrir", false);
        yield return new WaitForSeconds(0.5f);
        viñeta.SetActive(false);

    }


}