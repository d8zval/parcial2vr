using UnityEngine;

public class DesactivarPanel : MonoBehaviour
{
    [Header("Panel a desactivar")]
    public GameObject panel;

    public void OcultarPanel()
    {
        if (panel != null)
            panel.SetActive(false);
        else
            Debug.LogWarning("No se asign� ning�n panel en el inspector.");
    }
}
