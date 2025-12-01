using UnityEngine;

public class MostrarAlEntrarTrigger : MonoBehaviour
{
    [Header("Objeto que aparecerá dentro del trigger")]
    public GameObject objetoMostrar;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            objetoMostrar.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            objetoMostrar.SetActive(false);
        }
    }
}
