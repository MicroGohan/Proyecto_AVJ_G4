using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [Tooltip("Fuerza del movimiento horizontal del personaje.")]
    public float movimientoFuerza = 12f;  // Fuerza de movimiento horizontal

    [Header("Configuración de Salto")]
    [Tooltip("Fuerza del salto del personaje.")]
    public float saltoFuerza = 10f;  // Fuerza del salto

    private Rigidbody2D miCuerpoRigido;
    private bool estaEnSuelo;  // Verifica si el personaje está en el suelo

    // Layer para el suelo
    public Transform chequeoSuelo;
    public LayerMask capaSuelo;

    // Start is called before the first frame update
    void Start()
    {
        miCuerpoRigido = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float movimientoX = Input.GetAxisRaw("Horizontal"); // Esta variable puede ser -1, 0 o 1
        Vector2 posicionJug = transform.position;

        // Movimiento horizontal
        posicionJug = posicionJug + new Vector2(movimientoX, 0f) * movimientoFuerza * Time.deltaTime;

        // Saltar
        if (Input.GetButtonDown("Jump") && estaEnSuelo)  // Verifica si se presiona el botón de salto y si está en el suelo
        {
            miCuerpoRigido.velocity = new Vector2(miCuerpoRigido.velocity.x, saltoFuerza);  // Establece la velocidad de salto
        }

        transform.position = posicionJug;

        // Verificar si el personaje está en el suelo
        estaEnSuelo = Physics2D.OverlapCircle(chequeoSuelo.position, 0.2f, capaSuelo);  // Verifica si está tocando el suelo
    }
}
