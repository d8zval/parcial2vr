using UnityEngine;

public class RecolectableTrigger : MonoBehaviour
{
    // Referencia al ProgressManager para completar el módulo
    public int moduleIndex;  // Índice del módulo que se completará al recolectar este objeto

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si la colisión es con la MainCamera (o el jugador)
        if (other.CompareTag("MainCamera"))
        {
            // Completamos el módulo cuando el objeto es recolectado
            Debug.Log($"Objeto recolectado por: {other.name}");

            // Llamamos al ProgressManager para completar el módulo
            ProgressManager.Instance.UpdateProgress(moduleIndex);

            // Desactivar el objeto recolectable después de ser recogido
            gameObject.SetActive(false);
        }
    }
}
