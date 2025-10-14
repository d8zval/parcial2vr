using UnityEngine;

public class MenuFadeToggle : MonoBehaviour
{
    [Header("Panel del men� (debe tener CanvasGroup)")]
    public GameObject menuPanel;      // Tu panel (el objeto con Image)
    public float fadeSpeed = 5f;      // Velocidad de transici�n

    private CanvasGroup menuGroup;
    private bool estaVisible = false;

    void Start()
    {
        // Obtener o agregar autom�ticamente el CanvasGroup
        menuGroup = menuPanel.GetComponent<CanvasGroup>();
        if (menuGroup == null)
            menuGroup = menuPanel.AddComponent<CanvasGroup>();

        // Empieza oculto
        menuGroup.alpha = 0f;
        menuGroup.interactable = false;
        menuGroup.blocksRaycasts = false;
    }

    void Update()
    {
        float objetivo = estaVisible ? 1f : 0f;

        // Transici�n suave entre visible/invisible
        menuGroup.alpha = Mathf.Lerp(menuGroup.alpha, objetivo, Time.deltaTime * fadeSpeed);

        // Activar o desactivar la interacci�n seg�n el alpha
        bool activo = menuGroup.alpha > 0.05f;
        menuGroup.interactable = activo;
        menuGroup.blocksRaycasts = activo;
    }

    // Asigna esta funci�n al bot�n desde el inspector
    public void ToggleMenu()
    {
        estaVisible = !estaVisible;
    }
}
