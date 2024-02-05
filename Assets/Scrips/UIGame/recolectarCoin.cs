using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class recolectarCoin : MonoBehaviour
{
    [SerializeField] private AnimationClip clipRecolect;
    private Animator animator;
    private Collider2D collider;
    // Start is called before the first frame update
    void Start()
    {
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
            Destroy(gameObject,clipRecolect.length);
        }
    }
}
