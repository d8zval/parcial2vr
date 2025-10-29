using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class StarCompletionManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject[] starParts;  // 5 partes (Image u objs) que se activan por tag
    public GameObject[] rawImages;  // 5 RawImages que se activan junto a cada parte
    public GameObject emptyStar;    // Contenedor de la estrella completa

    [Header("Trigger Tags")]
    public string[] triggerTags = new string[5];  // Los tags de los triggers a detectar

    [Header("Audio")]
    public AudioClip collisionSound;   // Sonido que se reproduce cuando se colisiona con una parte de la estrella
    private AudioSource audioSource;   // Componente para reproducir el sonido

    [Header("Animación")]
    public Transform starTarget;      // Objetivo final (posición)
    public float moveDuration = 2f;   // Duración del Lerp
    public AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Acción final")]
    public Button starButton;         // Botón que llevará a la escena final
    public string finalSceneName = "SceneFinal"; // Nombre de la escena final

    private int collectedParts = 0;
    private bool starCompleted = false;
    private Vector3 startPos;

    void Start()
    {
        // Inicializar AudioSource
        audioSource = GetComponent<AudioSource>();

        // Guardamos la posición inicial de la estrella
        startPos = emptyStar != null ? emptyStar.transform.position : Vector3.zero;

        // Asegurarnos de que el botón está desactivado al principio
        if (starButton != null)
        {
            starButton.interactable = false;
            starButton.onClick.RemoveAllListeners();
            starButton.onClick.AddListener(LoadNextScene);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Revisa si el trigger tiene alguno de los 5 tags
        for (int i = 0; i < triggerTags.Length; i++)
        {
            if (!string.IsNullOrEmpty(triggerTags[i]) && other.CompareTag(triggerTags[i]))
            {
                ActivateStarPart(i);
                PlayCollisionSound(); // Reproduce el sonido de la colisión
                Destroy(other.gameObject);  // Destruye el objeto con el que colisiona

                if (collectedParts == 5 && !starCompleted)
                {
                    StartCoroutine(AnimateStarMovement());
                }
                break;
            }
        }
    }

    private void PlayCollisionSound()
    {
        // Reproduce el sonido de la colisión si está asignado
        if (audioSource != null && collisionSound != null)
        {
            audioSource.PlayOneShot(collisionSound);
        }
    }

    private void ActivateStarPart(int index)
    {
        if (index < 0 || index >= starParts.Length) return;

        // Evita contar dos veces la misma parte
        if (starParts[index] != null && !starParts[index].activeSelf)
        {
            starParts[index].SetActive(true);

            // Activar RawImage correspondiente
            if (rawImages != null && index < rawImages.Length && rawImages[index] != null)
                rawImages[index].SetActive(true);

            collectedParts++;
        }
    }

    private IEnumerator AnimateStarMovement()
    {
        starCompleted = true;

        if (emptyStar == null || starTarget == null) yield break;

        Vector3 from = emptyStar.transform.position;
        Vector3 to = starTarget.position;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.0001f, moveDuration);
            float k = moveCurve != null ? moveCurve.Evaluate(t) : t;
            emptyStar.transform.position = Vector3.LerpUnclamped(from, to, k);
            yield return null;
        }
        emptyStar.transform.position = to;

        // Habilita el botón para que el usuario avance cuando quiera
        if (starButton != null)
        {
            starButton.interactable = true;
        }
        else
        {
            Debug.LogWarning("[StarCompletionManager] No hay Button asignado. Asigna uno para navegar a la escena final.");
        }
    }

    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(finalSceneName))
            SceneManager.LoadScene(finalSceneName);
        else
            Debug.LogError("[StarCompletionManager] finalSceneName no asignado.");
    }
}
