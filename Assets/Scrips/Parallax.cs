using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private Vector2 velocidadMovimiento;

    private Vector2 offset;

    private Material material;
    private Rigidbody2D player1;


    private void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
      player1 = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    private void Update()
    {       
        offset = (player1.velocity.x*0.1f)*velocidadMovimiento* Time.deltaTime;
        material.mainTextureOffset += offset;
    }


}
 