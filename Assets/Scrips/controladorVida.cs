using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class controladorVida : MonoBehaviour
{
    [SerializeField] private int vida;
    [SerializeField] private float tiempoAnimMuerte=2;
    private Slider slider;
    private int sceneIndex;

    private void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        slider = GetComponent<Slider>();
        slider.maxValue = vida;
        slider.value = vida;
    }
    private void Update()
    {
        if (slider.value <= 0) 
        {
            StartCoroutine(animacionMuerteTime());
        }
    }
    IEnumerator animacionMuerteTime()
    {
        
        Eventos.eve.PausarPlayer.Invoke();
        Eventos.eve.PausarPlayer2.Invoke();
        yield return new WaitForSeconds(tiempoAnimMuerte);
        Eventos.eve.PasarNivel.Invoke(sceneIndex);
    }

    private void OnEnable()
    {
        Eventos.eve.perderVida.AddListener(quitarVida);
    }
    private void OnDisable()
    {
        Eventos.eve.perderVida.RemoveListener(quitarVida);
    }
    private void quitarVida() 
    {
        slider.value--;
    }
}
