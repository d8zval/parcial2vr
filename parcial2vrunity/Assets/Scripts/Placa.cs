using UnityEngine;

public class Placa : MonoBehaviour
{
    public int moduleID;
    public Material defaultMaterial;
    public Material activatedMaterial;
    public AudioClip sonidoActivacion;  // 🎵 Sonido que se reproducirá al activarse
    public float volumen = 1f;

    private bool activated = false;
    private Renderer rend;
    private AudioSource audioSource;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null && defaultMaterial != null)
            rend.material = defaultMaterial;

        // Crear o usar un AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
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

        // Cambiar material
        if (rend != null && activatedMaterial != null)
            rend.material = activatedMaterial;

        // Reproducir sonido
        if (sonidoActivacion != null)
        {
            audioSource.PlayOneShot(sonidoActivacion, volumen);
        }

        // Actualizar progreso
        ProgressManager.Instance.UpdateProgress(moduleID);
        Debug.Log($"Placa {name} activada");
    }
}
