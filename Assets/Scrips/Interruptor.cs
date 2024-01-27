using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interruptor : MonoBehaviour
{

    [SerializeField] private int id;
    [SerializeField] private bool presionMantenida;
    [Header("SoundFX")]
    private AudioSource audioSource;
    [SerializeField] private AudioClip audioActivar;
    [SerializeField] private AudioClip audioDesactivar;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")|| collision.CompareTag("ObInteract"))
        {
            Eventos.eve.ActivarPlataforma?.Invoke(id);
            Eventos.eve.activarCuerda?.Invoke(id);
            audioSource.clip = audioActivar;
            audioSource.Play();

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.CompareTag("Player")|| collision.CompareTag("ObInteract")) &&presionMantenida)
        {
            Eventos.eve.DesactivarPlataforma.Invoke(id);
            audioSource.clip = audioDesactivar;
            audioSource.Play();
        }
    }

}
