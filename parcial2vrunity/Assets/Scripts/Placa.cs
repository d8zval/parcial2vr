using UnityEngine;

public class Placa : MonoBehaviour
{
    public int moduleID;
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
        // Si entra la cámara (o el jugador)
        if (other.CompareTag("MainCamera") && !activated)
        {
            Activar();
        }
    }

    public void Activar()
    {
        activated = true;
        if (rend != null && activatedMaterial != null)
            rend.material = activatedMaterial;

        ProgressManager.Instance.UpdateProgress(moduleID);
        Debug.Log($"Placa {name} activada");
    }
}