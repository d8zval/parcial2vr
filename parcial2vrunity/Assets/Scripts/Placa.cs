using UnityEngine;

public class Placa : MonoBehaviour
{
    public int moduleID; // ID del m�dulo o stand al que pertenece (0-4)
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

    void OnTriggerEnter(Collider other)
    {
        // Detecta si el objeto que pasa es la c�mara del usuario
        if (other.CompareTag("MainCamera") && !activated)
        {
            activated = true;
            rend.material = activatedMaterial;

            // Notifica al administrador de progreso
            ProgressManager.Instance.UpdateProgress(moduleID);
        }
    }
}
