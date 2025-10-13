using UnityEngine;
using Vuforia;

public class PanelesLookAtCam : MonoBehaviour
{
    [Tooltip("C�mara AR. Si se deja vac�o, usar� Camera.main.")]
    public Camera arCamera;

    [Tooltip("Velocidad de rotaci�n (m�s alto = m�s r�pido).")]
    public float rotationSpeed = 5f;

    [Tooltip("Si est� activado, solo rota en el eje Y (mantiene verticalidad).")]
    public bool onlyRotateOnY = true;

    void Start()
    {
        if (arCamera == null)
        {
            arCamera = Camera.main;
            if (arCamera == null)
                Debug.LogWarning("[LookAtARCamera] No se encontr� Camera.main. Asigna la AR Camera manualmente.");
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