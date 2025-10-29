using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuButtonSpriteLogic : MonoBehaviour
{
    [Header("Sprites del botón")]
    public Sprite spriteBloqueado;
    public Sprite spriteActivo;

    [Header("Configuración de estados")]
    public bool estaDesbloqueado = false;
    public float duracionMensaje = 1.2f;

    [Header("Referencias")]
    public GameObject mensajeBloqueadoPanel;
    public GameObject textoDesbloqueado;
    public int moduloIndex = 0;

    [Header("Sonidos")]
    public AudioSource audioSource;
    public AudioClip sonidoBloqueado;
    public AudioClip sonidoDesbloquear;
    public AudioClip sonidoClickDesbloqueado;

    private Image imagenBoton;
    private Button boton;
    private Coroutine mensajeCoroutine;
    private bool ultimoEstado;

    // 🔹 Variable estática para controlar si hay un texto desbloqueado activo
    private static bool textoMostrandose = false;

    void Awake()
    {
        imagenBoton = GetComponent<Image>();
        boton = GetComponent<Button>();

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
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

        if (textoDesbloqueado != null)
            textoDesbloqueado.SetActive(false);
    }

    void Update()
    {
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
            Debug.Log($"Botón desbloqueado ({name}), realizando acción para el módulo {moduloIndex}...");
            ReproducirSonido(sonidoClickDesbloqueado);
            MostrarMensajeAccion(moduloIndex);

            // 🔹 Mostrar texto solo si no hay otro texto visible
            if (textoDesbloqueado != null && !textoMostrandose)
                StartCoroutine(MostrarTextoDesbloqueado());
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

    private IEnumerator MostrarTextoDesbloqueado()
    {
        textoMostrandose = true; // marcar que ya hay texto en pantalla
        textoDesbloqueado.SetActive(true);

        yield return new WaitForSeconds(2f);

        textoDesbloqueado.SetActive(false);
        textoMostrandose = false; // liberar el “bloqueo” de texto
    }

    private void ReproducirSonido(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }

    private void MostrarMensajeAccion(int moduloIndex)
    {
        Debug.Log($"Módulo {moduloIndex} completado. Acción realizada.");
    }
}
