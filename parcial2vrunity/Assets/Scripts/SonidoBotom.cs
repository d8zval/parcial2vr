using UnityEngine;
using UnityEngine.UI;

public class SonidoBotom : MonoBehaviour
{
    [Header("Configuración de sonido")]
    public AudioClip sonidoClick;
    private AudioSource audioSource;
    public Button boton;

    void Start()
    {
        // Crear o usar un AudioSource existente
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // Asignar evento al botón
        if (boton != null)
            boton.onClick.AddListener(ReproducirSonido);
    }

    public void ReproducirSonido()
    {
        if (sonidoClick != null)
            audioSource.PlayOneShot(sonidoClick);
    }
}
