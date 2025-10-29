using UnityEngine;

public class ObjetoRecolectable : MonoBehaviour
{
    public int moduleID;  // Módulo al que pertenece este objeto recolectable

    private bool collected = false;

    void OnTriggerEnter(Collider other)
    {
        // Si la cámara entra en contacto con el objeto recolectable
        if (other.CompareTag("MainCamera") && !collected)
        {
            Collect();
        }
    }

    void Collect()
    {
        collected = true;

        // Activar las imágenes y las partes de la estrella
        CanvasManager.Instance.ActivarParteDeLaEstrella(moduleID);

        // Destruir el objeto recolectable
        Destroy(gameObject);

        Debug.Log($"Objeto recolectable del módulo {moduleID} recogido");
    }
}
