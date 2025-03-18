using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target; // Referencia al personaje (objetivo)
    public float smoothSpeed = 0.125f; // Suavizado del movimiento
    public Vector3 offset; // Ajuste de la posición de la cámara

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("No hay un objetivo asignado para la cámara.");
            return;
        }

        // Calcula la posición deseada de la cámara (solo en el eje X)
        Vector3 desiredPosition = new Vector3(target.position.x + offset.x, transform.position.y, transform.position.z);

        // Interpola suavemente hacia la posición deseada
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Asigna la nueva posición a la cámara
        transform.position = smoothedPosition;
    }
}
