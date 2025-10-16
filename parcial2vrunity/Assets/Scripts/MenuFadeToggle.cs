using UnityEngine;

public class MenuFadeToggle : MonoBehaviour
{
    [Header("Panel del menú (debe tener CanvasGroup)")]
    public GameObject menuPanel;      // Tu panel (el objeto con Image)
    public float fadeSpeed = 5f;      // Velocidad de transición

    [Header("Audio")]
    public AudioSource audioSource;   // Asignar desde el Inspector
    public AudioClip sonidoBoton;     // Sonido al hacer clic
    [Range(0f, 1f)] public float volumen = 1f;

    private CanvasGroup menuGroup;
    private bool estaVisible = false;

    void Start()
    {
        // Obtener o agregar automáticamente el CanvasGroup
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
        menuGroup.alpha = Mathf.Lerp(menuGroup.alpha, objetivo, Time.deltaTime * fadeSpeed);

        bool activo = menuGroup.alpha > 0.05f;
        menuGroup.interactable = activo;
        menuGroup.blocksRaycasts = activo;
    }

    // Alterna visibilidad del menú
    public void ToggleMenu()
    {
        estaVisible = !estaVisible;
        ReproducirSonido();
    }

    // Reproduce el sonido usando el AudioSource asignado
    public void ReproducirSonido()
    {
        if (audioSource != null && sonidoBoton != null)
        {
            audioSource.PlayOneShot(sonidoBoton, volumen);
        }
        else
        {
            Debug.LogWarning("⚠️ Falta asignar el AudioSource o el sonido en MenuFadeToggle.");
        }
    }
}
