using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float velocidad;
    public Rigidbody2D rigidbody2D; // Referencia al Rigidbody2D

    void Awake()
    {
        // Obtener la referencia al Rigidbody2D al iniciar el script
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        ProcesarMovimiento();
    }

    void ProcesarMovimiento()
    {
        // Lógica de movimiento
        float inputMovimiento = Input.GetAxis("Horizontal"); // Corregido el error tipográfico

        rigidbody2D.velocity = new Vector2(inputMovimiento * velocidad, rigidbody2D.velocity.y);
    }
}
