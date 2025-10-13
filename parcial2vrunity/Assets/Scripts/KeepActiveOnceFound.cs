using UnityEngine;
using Vuforia;

public class KeepActiveOnceFound : MonoBehaviour
{
    private ObserverBehaviour observer;
    private bool wasFound = false;

    void Start()
    {
        observer = GetComponent<ObserverBehaviour>();
        observer.OnTargetStatusChanged += OnTargetStatusChanged;
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        if (status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED)
        {
            wasFound = true;
            gameObject.SetActive(true);
        }
        else if (wasFound)
        {
            // No desactivar si ya fue visto al menos una vez
            gameObject.SetActive(true);
        }
    }
}
