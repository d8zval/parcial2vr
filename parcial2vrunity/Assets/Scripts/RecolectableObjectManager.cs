using UnityEngine;

public class RecolectableObjectManager : MonoBehaviour
{
    private Vector3 targetPosition;  // Almacenará la posición en la que debe aparecer el objeto
    private bool isPlaced = false;   // Para asegurarnos de que el objeto solo se coloque una vez

    // Método para establecer la posición en la que se debe colocar el objeto
    public void SetPosition(Vector3 position)
    {
        targetPosition = position;
    }

    // Este método se llama para colocar el objeto en la escena cuando el progreso llegue al 100%
    public void PlaceObject()
    {
        if (!isPlaced)
        {
            transform.position = targetPosition; // Coloca el objeto en la posición que se ha establecido
            gameObject.SetActive(true);           // Activa el objeto recolectable
            isPlaced = true;                      // Asegura que el objeto solo se coloque una vez
            Debug.Log($"Objeto recolectable colocado en {targetPosition}");
        }
    }

    // Opcional: Si necesitas hacer algún tipo de comportamiento al instanciar el objeto (ej. rotación)
    public void SetRotation(Quaternion rotation)
    {
        transform.rotation = rotation;  // Establece la rotación deseada
    }
}
