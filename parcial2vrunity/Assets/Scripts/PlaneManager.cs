using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class PlaneManager : MonoBehaviour
{
    [SerializeField] private ARPlaneManager arPlaneManager;
    [SerializeField] private ARRaycastManager arRaycastManager;
    [SerializeField] private GameObject model3DPrefab;
    [SerializeField] private Camera arCamera;

    [Header("Configuración de posición")]
    [SerializeField] private float distancia = 1.0f; // Distancia frente al usuario

    private GameObject model3DPlaced;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void OnEnable()
    {
        if (arPlaneManager != null)
            arPlaneManager.planesChanged += OnPlanesChanged;
    }

    private void OnDisable()
    {
        if (arPlaneManager != null)
            arPlaneManager.planesChanged -= OnPlanesChanged;
    }

    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        if (model3DPlaced != null) return;

        // Dirección frente al usuario, manteniendo horizontal
        Vector3 forward = arCamera.transform.forward;
        forward.y = 0;
        forward.Normalize();

        // Punto inicial frente al usuario
        Vector3 puntoFrente = arCamera.transform.position + forward * distancia;

        // Lanzamos un raycast desde arriba hacia abajo para encontrar el piso
        Vector3 origenRaycast = puntoFrente + Vector3.up * 2f;

        if (arRaycastManager.Raycast(new Ray(origenRaycast, Vector3.down), hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            // ✅ Colocar modelo apoyado sobre el piso
            model3DPlaced = Instantiate(model3DPrefab, hitPose.position, Quaternion.LookRotation(forward));

            // Ajuste para asegurar que el modelo toque el piso (por su tamaño)
            Renderer rend = model3DPlaced.GetComponentInChildren<Renderer>();
            if (rend != null)
            {
                Bounds bounds = rend.bounds;
                float distanciaAlPiso = bounds.extents.y; // mitad de la altura total
                model3DPlaced.transform.position += Vector3.up * distanciaAlPiso;
            }

            Debug.Log($"Modelo colocado sobre el piso en {hitPose.position}");
            StopPlaneDetection();
        }
        else
        {
            Debug.Log("⚠No se detectó piso, colocando modelo frente a cámara como respaldo.");
            Vector3 fallbackPos = puntoFrente;
            model3DPlaced = Instantiate(model3DPrefab, fallbackPos, Quaternion.LookRotation(forward));
        }
    }

    private void StopPlaneDetection()
    {
        if (arPlaneManager == null) return;

        arPlaneManager.requestedDetectionMode = PlaneDetectionMode.None;

        foreach (var plane in arPlaneManager.trackables)
            plane.gameObject.SetActive(false);
    }
}
