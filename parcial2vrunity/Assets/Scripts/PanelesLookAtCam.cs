using UnityEngine;
using Vuforia;

public class PanelesLookAtCam : MonoBehaviour
{
    [Tooltip("Cámara AR. Si se deja vacío, usará Camera.main.")]
    public Camera arCamera;

    [Tooltip("Velocidad de rotación (más alto = más rápido).")]
    public float rotationSpeed = 5f;

    [Tooltip("Si está activado, solo rota en el eje Y (mantiene verticalidad).")]
    public bool onlyRotateOnY = true;

    void Start()
    {
        if (arCamera == null)
        {
            arCamera = Camera.main;
            if (arCamera == null)
                Debug.LogWarning("[LookAtARCamera] No se encontró Camera.main. Asigna la AR Camera manualmente.");
        }
    }

    void Update()
    {
        if (arCamera == null) return;

        Vector3 direction = arCamera.transform.position - transform.position;

        if (onlyRotateOnY)
            direction.y = 0; // Mantenerlo vertical

        if (direction.sqrMagnitude < 0.001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}