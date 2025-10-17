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
            Debug.LogWarning("No se asignó ningún panel en el inspector.");
    }
}
