using UnityEngine;
using Vuforia;

public class PanelesLookAtCam : MonoBehaviour
{
    [Header("Configuración")]
    public Transform[] paneles;       // Paneles de fotos o relieves
    public Camera arCamera;           // Cámara principal de Vuforia
    private ObserverBehaviour target; // El ImageTarget

    void Start()
    {
        // Obtener el ImageTarget más cercano en el objeto
        target = GetComponentInParent<ObserverBehaviour>();

        // Si no asignas cámara manualmente, usa la principal
        if (arCamera == null)
            arCamera = Camera.main;

        if (target != null)
            target.OnTargetStatusChanged += OnTargetStatusChanged;
    }

    private void OnDestroy()
    {
        if (target != null)
            target.OnTargetStatusChanged -= OnTargetStatusChanged;
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        // Solo rotamos si el target está visible o en tracking
        bool visible = status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED;

        if (visible)
            enabled = true;
        else
            enabled = false;
    }

    void Update()
    {
        if (arCamera == null) return;

        foreach (var panel in paneles)
        {
            // Rota suavemente hacia la cámara
            Vector3 dir = panel.position - arCamera.transform.position;
            Quaternion lookRot = Quaternion.LookRotation(dir, Vector3.up);
            panel.rotation = Quaternion.Slerp(panel.rotation, lookRot, Time.deltaTime * 5f);
        }
    }
}