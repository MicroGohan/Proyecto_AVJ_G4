using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad de movimiento
    public float minY = -3f; // L�mite inferior en el eje Y
    public float maxY = -2f; // L�mite superior en el eje Y

    void Update()
    {
        // Obt�n la entrada del jugador (teclado o joystick)
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        // Calcula la nueva posici�n
        Vector3 newPosition = transform.position + new Vector3(moveX, moveY, 0);

        // Aplica los l�mites en el eje Y
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        // Mueve al personaje
        transform.position = newPosition;
    }
}
