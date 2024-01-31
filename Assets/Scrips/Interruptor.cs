using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Interruptor : MonoBehaviour
{

    [SerializeField] private int id;
    [SerializeField] private bool presionMantenida;
    [Header("SoundFX")]
    private AudioSource audioSource;
    [SerializeField] private AudioSource otherAudioSource;
    [SerializeField] private AudioClip audioActivar;
    [SerializeField] private AudioClip audioDesactivar;
    private bool primerTriggerYaEntr� = false;
    private Collider2D primerObjeto;
    private String tagPO;
   

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("ObInteract"))
        {
            if (!primerTriggerYaEntr�)
            {
                if (otherAudioSource != null)
                {
                    otherAudioSource.Play();
                }
                primerTriggerYaEntr� = true;
                primerObjeto = collision;
                tagPO = collision.tag;
                Eventos.eve.ActivarPlataforma?.Invoke(id);
                Eventos.eve.activarCuerda?.Invoke(id);
                audioSource.clip = audioActivar;
                audioSource.Play();
                // Aqu� va tu c�digo para cuando el primer trigger entra al collider
            }
        }
        
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.CompareTag("Player") || collision.CompareTag("ObInteract")) && presionMantenida)
        {
            if (primerObjeto == collision)
            {
                primerTriggerYaEntr� = false;
                Eventos.eve.DesactivarPlataforma.Invoke(id);
                audioSource.clip = audioDesactivar;
                audioSource.Play();

            }
            else if (tagPO==("Player") && collision.CompareTag("Player"))
            {
                primerTriggerYaEntr� = false;
                Eventos.eve.DesactivarPlataforma.Invoke(id);
                audioSource.clip = audioDesactivar;
                audioSource.Play();
            }
        }
        
        
    }

}
