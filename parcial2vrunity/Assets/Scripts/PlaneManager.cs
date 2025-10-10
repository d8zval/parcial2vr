using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaneManager : MonoBehaviour
{
    [SerializeField] private ARPlaneManager arPlaneManager;
    [SerializeField] private GameObject model3DPrefab;

    private GameObject model3DPlaced;

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

        foreach (var plane in args.added)
        {
            if (plane == null || !plane.gameObject.activeInHierarchy)
                continue;

            if (plane.extents.x * plane.extents.y >= 0.05f)
            {
                Vector3 worldPos = plane.transform.TransformPoint(plane.center);

                model3DPlaced = Instantiate(model3DPrefab, worldPos, Quaternion.identity);

                // ✅ Alinear al piso
                Renderer rend = model3DPlaced.GetComponentInChildren<Renderer>();
                if (rend != null)
                {
                    Bounds bounds = rend.bounds;
                    float pivotToBottom = bounds.extents.y;
                    model3DPlaced.transform.position -= Vector3.up * pivotToBottom;
                }

                Debug.Log("✅ Prefab colocado en el piso: " + model3DPlaced.transform.position);

                StopPlaneDetection();
                break;
            }
        }
    }

    private void StopPlaneDetection()
    {
        if (arPlaneManager == null) return;

        arPlaneManager.requestedDetectionMode = PlaneDetectionMode.None;

        foreach (var plane in arPlaneManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }
    }
}
