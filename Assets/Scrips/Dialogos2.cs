using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogos2 : MonoBehaviour
{

    public float tiempoEntreChar;
    [SerializeField, TextArea(4, 5)] private string[] lineasDialogo;
    [SerializeField] private GameObject vineta;
    private int LineIndex;
    private bool leido;
    private bool activeDialog;


    private void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&&!leido) 
        {
            EmpezarDialogo();
        }
    }
    private void Update()
    {
       


        if (activeDialog && Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Joystick1Button1) && activeDialog)
        {

            if (vineta.GetComponentInChildren<TextMeshProUGUI>().text == lineasDialogo[LineIndex])
            {
                NextDialogLine();
                
                

            }
        }
    }

    private void EmpezarDialogo()
    {

        Eventos.eve.IniciarDialogo2.RemoveListener(EmpezarDialogo);
        Eventos.eve.PausarPlayer1.Invoke();
        Eventos.eve.PausarPlayer2.Invoke();
        activeDialog = true;
        vineta.SetActive(true);
        vineta.GetComponent<Animator>().SetBool("abrir", true);
        LineIndex = 0;
        mostrarLinea();
    }
    private void NextDialogLine()
    {
        LineIndex++;
        if (LineIndex < lineasDialogo.Length)
        {
            mostrarLinea();
        }
        else
        {
            StartCoroutine(ocultar());
            activeDialog = false;
            Eventos.eve.DespausarPlayer1.Invoke();
            Eventos.eve.DespausarPlayer2.Invoke();
            

        }
    }

    private void OnEnable()
    {
        //Eventos.eve.IniciarDialogo2.AddListener(EmpezarDialogo);
    }

    private void mostrarLinea()
    {
        vineta.GetComponentInChildren<TextMeshProUGUI>().text = lineasDialogo[LineIndex];
        
    }
    private IEnumerator ocultar()
    {
        leido = true;
        vineta.GetComponent<Animator>().SetBool("abrir", false);
        yield return new WaitForSeconds(0.5f);
        vineta.SetActive(false);

    }


}