using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class QuizMusicController : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false; // Asegura que no suene sola
    }

    void OnEnable()
    {
        // Cuando el panel del quiz se activa → empieza la música
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    void OnDisable()
    {
        // Cuando el panel se desactiva → detiene la música
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}