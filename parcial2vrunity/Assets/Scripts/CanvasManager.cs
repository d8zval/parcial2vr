using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance;  // Singleton de CanvasManager

    public Image[] starPartsImages;  // Imágenes de las partes de la estrella
    public RawImage[] starPartsRawImages;  // RawImages que se activan junto a cada parte (si es necesario)
    public GameObject emptyStar;        // El objeto vacío que contiene todas las partes de la estrella
    public Transform targetPosition;    // La posición donde la estrella debe moverse cuando esté completa

    public GameObject[] objectsToDisable;  // Objetos que se desactivarán antes de mover la estrella
    public Button[] otherButtons;          // Array de botones que se deshabilitarán al recolectar un objeto

    public Button starButton;         // Botón que llevará a la escena final
    public AudioClip collectSound;    // Sonido al recolectar un objeto
    public AudioClip starMoveSound;   // Sonido para la animación de la estrella

    private int collectedParts = 0;
    private AudioSource audioSource;  // Referencia al AudioSource

    void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Mantener el CanvasManager entre escenas
        }
        else
        {
            Destroy(gameObject); // Destruir si ya existe una instancia
        }
    }

    void Start()
    {
        // Verificar si el AudioSource está presente
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // Si no hay AudioSource, lo agregamos
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Desactivar todas las imágenes de las partes de la estrella al inicio
        foreach (Image img in starPartsImages)
        {
            img.gameObject.SetActive(false);
        }

        // Desactivar las RawImages si las hay
        foreach (RawImage img in starPartsRawImages)
        {
            img.gameObject.SetActive(false);
        }

        // Desactivar el botón para la escena final al principio
        if (starButton != null)
        {
            starButton.interactable = false;  // Hacer que el botón no sea clickeable al inicio
        }
    }

    // Método que se llama cuando se recoge un objeto recolectable
    public void ActivarParteDeLaEstrella(int moduleID)
    {
        // Activar la imagen correspondiente de la estrella
        if (moduleID >= 0 && moduleID < starPartsImages.Length)
        {
            // Activar la parte de la estrella correspondiente
            starPartsImages[moduleID].gameObject.SetActive(true);

            // También activamos el RawImage correspondiente si lo hay
            if (moduleID < starPartsRawImages.Length)
                starPartsRawImages[moduleID].gameObject.SetActive(true);

            // Reproducir sonido de recolección, si se ha asignado
            if (collectSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(collectSound);
            }

            collectedParts++;

            Debug.Log($"Parte {moduleID + 1} de la estrella activada");

            // Desactivar el botón correspondiente al objeto recolectado
            if (otherButtons != null && moduleID < otherButtons.Length)
            {
                otherButtons[moduleID].interactable = false;  // Desactivar solo el botón correspondiente
            }

            // Si todas las partes están recogidas, mover la estrella
            if (collectedParts == starPartsImages.Length)
            {
                StartCoroutine(MoveStarToTarget());
            }
        }
    }

    // Coroutine para mover la estrella a su posición final
    private IEnumerator MoveStarToTarget()
    {
        // Desactivar los objetos específicos antes de mover la estrella
        foreach (GameObject obj in objectsToDisable)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        // Reproducir sonido de la animación de la estrella
        if (starMoveSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(starMoveSound);
        }

        // Mover la estrella a la posición de destino
        Vector3 initialPosition = emptyStar.transform.position;
        Vector3 target = targetPosition.position;

        float duration = 2f;  // Duración del movimiento
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            emptyStar.transform.position = Vector3.Lerp(initialPosition, target, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Asegurarse de que la estrella esté exactamente en la posición de destino
        emptyStar.transform.position = target;

        // Una vez movida, habilitamos el Empty como un "botón" para ir a la escena final
        if (starButton != null)
        {
            starButton.interactable = true;  // Habilitar el botón para la siguiente escena

            // Asegurarnos de que el botón tiene la acción asociada para cargar la escena
            starButton.onClick.RemoveAllListeners();  // Limpiar cualquier listener anterior
            starButton.onClick.AddListener(LoadNextScene);  // Asignar la acción de cargar la escena
        }
    }

    // Método para cargar la siguiente escena
    private void LoadNextScene()
    {
        // Asumimos que ya tienes la escena final configurada
        SceneManager.LoadScene("FiltrosEscena");  // Cambia "SceneFinal" por el nombre de tu escena final
    }

}
