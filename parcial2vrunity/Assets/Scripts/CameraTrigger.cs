using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto con el que colisiona tiene el tag "Placa"
        if (other.CompareTag("Placa"))
        {
            // Busca el componente Placa en el objeto con el que colisionó
            Placa placa = other.GetComponent<Placa>();

            if (placa != null)
            {
                placa.Activar();
            }
        }
    }
}