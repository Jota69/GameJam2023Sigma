using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogos2 : MonoBehaviour
{

    public float tiempoEntreChar;
    [SerializeField, TextArea(4, 5)] private string[] lineasDialogo;
    [SerializeField] private GameObject[] imagenesDialogo;
    [SerializeField] private Enemigo[] enemigosADetener;
    private Vector3[] posicionesEnemigos;
    [SerializeField] private GameObject vineta;
    private int LineIndex;
    private bool leido;
    private bool activeDialog;
    private void Start()
    {
        leido = false;
        posicionesEnemigos = new Vector3[enemigosADetener.Length];
        if(enemigosADetener != null)
        {
            for(int i = 0;i<enemigosADetener.Length;i++)
            {
                posicionesEnemigos[i] = enemigosADetener[i].gameObject.transform.position;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&&!leido)
        {
            leido = true;
            EmpezarDialogo();
        }
    }
    private void Update()
    {
       


        if (activeDialog && Input.GetKeyDown(KeyCode.Return))
        {

            if (vineta.GetComponentInChildren<TextMeshProUGUI>().text == lineasDialogo[LineIndex])
            {
                NextDialogLine();
            }
            else
            {
                StopAllCoroutines();
                vineta.GetComponentInChildren<TextMeshProUGUI>().text = lineasDialogo[LineIndex];
            }
        }
    }

    private void EmpezarDialogo()
    {

        Eventos.eve.IniciarDialogo2.RemoveListener(EmpezarDialogo);
        Eventos.eve.PausarPlayer.Invoke();
        Eventos.eve.PausarPlayer2.Invoke();
        if (enemigosADetener!=null)
        {
            for (int i = 0;i<enemigosADetener.Length;i++)
            {
                enemigosADetener[i].parado = true;
                enemigosADetener[i].transform.position = posicionesEnemigos[i];
            }
        }
        activeDialog = true;
        vineta.SetActive(true);
        vineta.GetComponent<Animator>().SetBool("abrir", true);
        LineIndex = 0;
        imagenesDialogo[0].SetActive(true);
        StartCoroutine(mostrarLinea());
    }
    private void NextDialogLine()
    {
        GameObject e=imagenesDialogo[0];
        LineIndex++;
        for (int i = 0; i < imagenesDialogo.Length; i++)
        {
            if (i == LineIndex)
            {
                imagenesDialogo[i].SetActive(true);
                e = imagenesDialogo[i];
            }
            else if (imagenesDialogo[i] != e)
            {
                imagenesDialogo[i].SetActive(false);
            }
        }
        if (LineIndex < lineasDialogo.Length)
        {
            StartCoroutine(mostrarLinea());
        }
        else
        {
            StartCoroutine(ocultar());
            activeDialog = false;
            Eventos.eve.DespausarPlayer.Invoke();
            Eventos.eve.DespausarPlayer2.Invoke();
            

        }
    }

    private void OnEnable()
    {
        //Eventos.eve.IniciarDialogo2.AddListener(EmpezarDialogo);
    }

    private IEnumerator mostrarLinea()
    {
        
        vineta.GetComponentInChildren<TextMeshProUGUI>().text = string.Empty;
        foreach (char line in lineasDialogo[LineIndex])
        {
            vineta.GetComponentInChildren<TextMeshProUGUI>().text += line;
            yield return new WaitForSeconds(tiempoEntreChar);
        }
    }
    private IEnumerator ocultar()
    {
        if (enemigosADetener != null)
        {
            foreach (Enemigo enemigo in enemigosADetener)
            {
                enemigo.parado = false;
            }
        }
        for (int i = 0; i < imagenesDialogo.Length; i++)
        {
            imagenesDialogo[i].SetActive(false);
        }
        
        vineta.GetComponent<Animator>().SetBool("abrir", false);
        yield return new WaitForSeconds(0.5f);
        vineta.SetActive(false);

    }


}