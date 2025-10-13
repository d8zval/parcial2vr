using System.Collections;
using UnityEngine;
using Vuforia;

public class KeepActiveOnceFound : MonoBehaviour
{
    private ObserverBehaviour observer;
    public GameObject modelo3D;
    private bool activado = false;

    void Start()
    {
        observer = GetComponent<ObserverBehaviour>();

        if (observer == null)
        {
            Debug.LogError("❌ No se encontró un ObserverBehaviour en este objeto.");
            return;
        }

        observer.OnTargetStatusChanged += OnTargetStatusChanged;

        if (modelo3D != null)
            modelo3D.SetActive(false);
        else
            Debug.LogError("❌ No asignaste un modelo 3D en el inspector.");
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        if (!activado && (status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED))
        {
            StartCoroutine(ActivarYFijarModeloDespues(behaviour));
            activado = true;
            observer.OnTargetStatusChanged -= OnTargetStatusChanged;
        }
    }

    private IEnumerator ActivarYFijarModeloDespues(ObserverBehaviour behaviour)
    {
        // Espera un poquito para que Vuforia actualice bien la posición
        yield return new WaitForSeconds(0.1f);

        if (modelo3D == null || behaviour == null)
        {
            Debug.LogError("❌ Faltan referencias al modelo o al observer.");
            yield break;
        }

        // Coloca el modelo temporalmente como hijo del target
        modelo3D.transform.SetParent(behaviour.transform);
        modelo3D.transform.localPosition = Vector3.zero;
        modelo3D.transform.localRotation = Quaternion.identity;

        // Activar el modelo
        modelo3D.SetActive(true);
        Debug.Log("✅ Modelo activado sobre el target.");

        // Guardar posición y rotación global
        Vector3 posicionFinal = modelo3D.transform.position;
        Quaternion rotacionFinal = modelo3D.transform.rotation;

        // Desvincular para que quede fijo en el mundo
        modelo3D.transform.SetParent(null);
        modelo3D.transform.position = posicionFinal;
        modelo3D.transform.rotation = rotacionFinal;
    }
}
