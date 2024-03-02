using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class recolectarCoin : MonoBehaviour
{
    [SerializeField] private AnimationClip clipRecolect;
    private Animator animator;
    private Collider2D collider;
    AudioSource audioSource;
    [SerializeField] private AudioClip clipAudio;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collider.enabled = false;
            Eventos.eve.coinsCount.Invoke(1);
            animator.SetBool("Recolectado", true);
            if (clipAudio != null)
            {
                audioSource.clip = clipAudio;
                audioSource.Play();
            }
            Destroy(gameObject,clipRecolect.length);
        }
    }
}
