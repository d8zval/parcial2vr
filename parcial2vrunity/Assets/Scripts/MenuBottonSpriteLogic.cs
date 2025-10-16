using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuButtonSpriteLogic : MonoBehaviour
{
    [Header("Sprites del botón")]
    public Sprite spriteBloqueado;
    public Sprite spriteActivo;

    [Header("Configuración de estados")]
    public bool estaDesbloqueado = false;     // Estado inicial
    public float duracionMensaje = 2f;        // Duración del mensaje de bloqueo

    [Header("Referencias")]
    public GameObject mensajeBloqueadoPanel;  // Panel o mensaje de "bloqueado"
    public QuizManager quizManager;           // Referencia al QuizManager
    public int moduloIndex = 0;               // Índice del módulo a abrir (0 = primer módulo)

    [Header("Sonidos")]
    public AudioSource audioSource;           // Fuente de audio (puede estar en el mismo objeto)
    public AudioClip sonidoBloqueado;         // Sonido al intentar clic bloqueado
    public AudioClip sonidoDesbloquear;       // Sonido al desbloquear el botón
    public AudioClip sonidoClickDesbloqueado; // Sonido al hacer clic desbloqueado

    private Image imagenBoton;
    private Button boton;
    private Coroutine mensajeCoroutine;
    private bool ultimoEstado;

    void Awake()
    {
        imagenBoton = GetComponent<Image>();
        boton = GetComponent<Button>();
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                // Crear automáticamente un AudioSource si no existe
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
            }
        }
    }

    void Start()
    {
        ultimoEstado = estaDesbloqueado;
        ActualizarSprite();
        boton.onClick.AddListener(OnClickBoton);
    }

    void Update()
    {
        // Detectar cambio manual en tiempo real desde el Inspector
        if (estaDesbloqueado != ultimoEstado)
        {
            ActualizarSprite();
            ultimoEstado = estaDesbloqueado;
        }
    }

    void OnClickBoton()
    {
        if (!estaDesbloqueado)
        {
            Debug.Log($"Botón bloqueado ({name}), mostrando mensaje...");
            ReproducirSonido(sonidoBloqueado);

            if (mensajeBloqueadoPanel != null)
            {
                if (mensajeCoroutine != null)
                    StopCoroutine(mensajeCoroutine);
                mensajeCoroutine = StartCoroutine(MostrarMensajeTemporal());
            }
        }
        else
        {
            Debug.Log($"Botón desbloqueado ({name}), iniciando módulo {moduloIndex}...");
            ReproducirSonido(sonidoClickDesbloqueado);

            if (quizManager != null)
            {
                quizManager.SeleccionarModulo(moduloIndex);
            }
            else
            {
                Debug.LogWarning("No se asignó el QuizManager en el botón.");
            }
        }
    }

    private IEnumerator MostrarMensajeTemporal()
    {
        mensajeBloqueadoPanel.SetActive(true);
        yield return new WaitForSeconds(duracionMensaje);
        mensajeBloqueadoPanel.SetActive(false);
    }

    private void ActualizarSprite()
    {
        if (imagenBoton == null) return;
        imagenBoton.sprite = estaDesbloqueado ? spriteActivo : spriteBloqueado;
    }

    public void Bloquear()
    {
        estaDesbloqueado = false;
        ActualizarSprite();
        Debug.Log("Botón bloqueado manualmente.");
    }

    public void Desbloquear()
    {
        estaDesbloqueado = true;
        ActualizarSprite();
        Debug.Log("Botón desbloqueado manualmente.");
        ReproducirSonido(sonidoDesbloquear);
    }

    public void CambiarEstado()
    {
        estaDesbloqueado = !estaDesbloqueado;
        ActualizarSprite();
        Debug.Log(estaDesbloqueado ? "Botón desbloqueado." : "Botón bloqueado.");

        if (estaDesbloqueado)
            ReproducirSonido(sonidoDesbloquear);
    }

    private void ReproducirSonido(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}