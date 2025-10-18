using UnityEngine;

public class LaiaTP : MonoBehaviour
{
    [Header("Modelo que se teletransportará")]
    public GameObject modelo;  // Asigna el modelo en el Inspector

    [Header("Sonido al activarse (opcional)")]
    public AudioClip sonidoActivacion;
    public float volumen = 1f;

    private AudioSource audioSource;

    void Start()
    {
        // Crear o usar un AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
    }

    void OnTriggerEnter(Collider other)
    {
        // Solo reacciona si entra la cámara principal
        if (other.CompareTag("MainCamera"))
        {
            TeletransportarModelo();

            // Reproducir sonido si hay uno asignado
            if (sonidoActivacion != null)
                audioSource.PlayOneShot(sonidoActivacion, volumen);

            Debug.Log($"Modelo teletransportado a {name}");
        }
    }

    private void TeletransportarModelo()
    {
        if (modelo == null)
        {
            Debug.LogWarning("No se ha asignado el modelo para teletransportar.");
            return;
        }

        // Teletransportar el modelo al centro del trigger
        modelo.transform.position = transform.position;
    }
}