using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CanvasManager : MonoBehaviour
{
    [Header("UI Elements")]
    public RawImage[] collectionImages;  // Las 5 imágenes de colección (correspondientes a cada objeto recolectable)
    public GameObject[] starParts; // Las partes de la estrella que se activan por módulo
    public GameObject transitionButton;  // El botón que aparecerá al completar la estrella

    [Header("Audio")]
    public AudioClip collectionSound;  // Sonido de recolección

    [Header("Posición Final")]
    public Transform finalPosition;  // Posición final para la animación de la estrella

    [Header("Objetos a Desactivar")]
    public GameObject[] objectsToDeactivate;  // 16 GameObjects a desactivar al final de la animación

    private void Start()
    {
        // Desactivar el botón de transición al inicio
        transitionButton.SetActive(false);

        // Desactivar todas las imágenes de colección al inicio
        foreach (var image in collectionImages)
        {
            image.gameObject.SetActive(false);
        }
    }

    // Esta función se llama cuando el jugador recoge un objeto
    public void OnObjectCollected(int moduleIndex)
    {
        // Reproducir sonido al recolectar el objeto
        if (collectionSound != null)
        {
            AudioSource.PlayClipAtPoint(collectionSound, Camera.main.transform.position);
        }

        // Activar la parte correspondiente de la estrella
        ActivateStarPart(moduleIndex);

        // Activar la imagen de recolección correspondiente
        ActivateCollectionImage(moduleIndex);

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
        // Posición final de la estrella (usando el Transform asignado en el Inspector)
        Vector3 targetPosition = finalPosition.position;

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

        // Desactivar los 16 objetos al finalizar la animación
        DeactivateObjects();

        // Activar el botón de transición para ir a la siguiente escena
        ActivateTransitionButton();
    }

    // Desactivar los 16 objetos al final de la animación
    private void DeactivateObjects()
    {
        foreach (var obj in objectsToDeactivate)
        {
            obj.SetActive(false);
        }
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

    // Activar la imagen de colección correspondiente
    private void ActivateCollectionImage(int index)
    {
        if (index >= 0 && index < collectionImages.Length)
        {
            collectionImages[index].gameObject.SetActive(true);
        }
    }
}
