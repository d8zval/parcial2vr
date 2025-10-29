using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CanvasManager : MonoBehaviour
{
    [Header("UI Elements")]
    public RawImage collectionImage;  // Imagen que muestra que se ha recolectado un objeto
    public Image starImage;  // La estrella que va por partes
    public GameObject[] starParts; // Las partes de la estrella que se activan por módulo
    public GameObject transitionButton;  // El botón que aparecerá al completar la estrella

    [Header("Audio")]
    public AudioClip collectionSound;  // Sonido de recolección

    private void Start()
    {
        // Desactivar el botón de transición al inicio
        transitionButton.SetActive(false);
    }

    // Esta función se llama cuando el jugador recoge un objeto
    public void OnObjectCollected(int moduleIndex)
    {
        // Reproducir sonido al recolectar el objeto
        if (collectionSound != null)
        {
            AudioSource.PlayClipAtPoint(collectionSound, Camera.main.transform.position);
        }

        // Activar la imagen de recolección
        collectionImage.gameObject.SetActive(true);

        // Activar la parte correspondiente de la estrella
        ActivateStarPart(moduleIndex);

        // Si la estrella está completa, moverla a la posición final
        if (IsStarCompleted())
        {
            StartCoroutine(MoveStarToFinalPosition());
        }
    }

    // Activar la parte de la estrella correspondiente al módulo que se recolectó
    private void ActivateStarPart(int moduleIndex)
    {
        if (moduleIndex >= 0 && moduleIndex < starParts.Length)
        {
            // Activar la parte de la estrella según el índice
            starParts[moduleIndex].SetActive(true);
        }
    }

    // Comprobar si todas las partes de la estrella están activas (completada)
    private bool IsStarCompleted()
    {
        foreach (GameObject part in starParts)
        {
            if (!part.activeSelf) return false;  // Si alguna parte está desactivada, no está completa
        }
        return true;
    }

    // Mover la estrella a la posición final con animación
    private IEnumerator MoveStarToFinalPosition()
    {
        // Posición final de la estrella (puedes asignar un empty para la posición final)
        Vector3 targetPosition = new Vector3(10, 10, 0);  // Ajusta la posición final

        // Aquí va la animación para mover la estrella completa a la posición final
        Vector3 startPosition = starParts[0].transform.position; // Posición inicial de la estrella
        float time = 0;
        float duration = 2.0f;  // Duración de la animación (puedes ajustarla)

        while (time < duration)
        {
            time += Time.deltaTime;
            float lerpFactor = time / duration;

            foreach (var part in starParts)
            {
                part.transform.position = Vector3.Lerp(startPosition, targetPosition, lerpFactor);
            }
            yield return null;
        }

        // Activar el botón de transición para ir a la siguiente escena
        ActivateTransitionButton();
    }

    // Activar el botón de transición
    private void ActivateTransitionButton()
    {
        transitionButton.SetActive(true);
        transitionButton.GetComponent<Button>().onClick.AddListener(GoToNextScene);
    }

    // Cambiar a la siguiente escena
    private void GoToNextScene()
    {
        // Aquí deberías cargar la siguiente escena (ajusta el nombre de la escena según sea necesario)
        UnityEngine.SceneManagement.SceneManager.LoadScene("NextScene");  // Cambia "NextScene" por el nombre real
    }
}
