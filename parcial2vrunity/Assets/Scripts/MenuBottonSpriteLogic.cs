using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuButtonSpriteLogic : MonoBehaviour
{
    [Header("Sprites del bot�n")]
    public Sprite spriteBloqueado;
    public Sprite spriteActivo;

    [Header("Configuraci�n de estados")]
    public bool estaDesbloqueado = false;     // Estado inicial
    public float duracionMensaje = 2f;        // Duraci�n del mensaje de bloqueo

    [Header("Referencias")]
    public GameObject mensajeBloqueadoPanel;  // Panel o mensaje de "bloqueado"
    public QuizManager quizManager;           // Referencia al QuizManager
    public int moduloIndex = 0;               // �ndice del m�dulo a abrir (0 = primer m�dulo)

    [Header("Sonidos")]
    public AudioSource audioSource;           // Fuente de audio (puede estar en el mismo objeto)
    public AudioClip sonidoBloqueado;         // Sonido al intentar clic bloqueado
    public AudioClip sonidoDesbloquear;       // Sonido al desbloquear el bot�n
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
                // Crear autom�ticamente un AudioSource si no existe
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
            Debug.Log($"Bot�n bloqueado ({name}), mostrando mensaje...");
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
            Debug.Log($"Bot�n desbloqueado ({name}), iniciando m�dulo {moduloIndex}...");
            ReproducirSonido(sonidoClickDesbloqueado);

            if (quizManager != null)
            {
                quizManager.SeleccionarModulo(moduloIndex);
            }
            else
            {
                Debug.LogWarning("No se asign� el QuizManager en el bot�n.");
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
        Debug.Log("Bot�n bloqueado manualmente.");
    }

    public void Desbloquear()
    {
        estaDesbloqueado = true;
        ActualizarSprite();
        Debug.Log("Bot�n desbloqueado manualmente.");
        ReproducirSonido(sonidoDesbloquear);
    }

    public void CambiarEstado()
    {
        estaDesbloqueado = !estaDesbloqueado;
        ActualizarSprite();
        Debug.Log(estaDesbloqueado ? "Bot�n desbloqueado." : "Bot�n bloqueado.");

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