using UnityEngine;

public class Placa : MonoBehaviour
{
    public int moduleID; // ID del módulo o stand al que pertenece (0-4)
    public Material defaultMaterial;
    public Material activatedMaterial;
    private bool activated = false;

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null && defaultMaterial != null)
            rend.material = defaultMaterial;
    }

    public void Activar()
    {
        if (activated) return;

        activated = true;
        if (rend != null && activatedMaterial != null)
            rend.material = activatedMaterial;

        // Notifica al administrador de progreso
        ProgressManager.Instance.UpdateProgress(moduleID);

        Debug.Log($"Placa {name} activada");
    }
}