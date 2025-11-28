using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class VideosPanel : MonoBehaviour
{
    [Header("Botón que aparece al entrar al trigger")]
    [SerializeField] private GameObject verVideosButton;

    [Header("Texto del botón Ver Videos")]
    [SerializeField] private TMP_Text verVideosButtonText;

    [Header("Panels de video")]
    [SerializeField] private GameObject panelNormal;
    [SerializeField] private GameObject panelFullscreen;

    [Header("VideoPlayers")]
    [SerializeField] private VideoPlayer normalPlayer;
    [SerializeField] private VideoPlayer fullscreenPlayer;

    [Header("Textos (opcional)")]
    [SerializeField] private TMP_Text infoText;        // Mensaje tipo "no hay videos"
    [SerializeField] private TMP_Text decadeTitleText; // Título de la década

    [Header("Listas por década")]
    [SerializeField] private List<VideoClip> videos70s = new List<VideoClip>();
    [SerializeField] private List<VideoClip> videos80s = new List<VideoClip>();
    [SerializeField] private List<VideoClip> videos90s = new List<VideoClip>();
    [SerializeField] private List<VideoClip> videos00s = new List<VideoClip>();
    [SerializeField] private List<VideoClip> videos10s = new List<VideoClip>();

    [Header("Botones (normal y fullscreen usan los mismos métodos)")]
    [SerializeField] private Button playPauseButtonNormal;
    [SerializeField] private Button nextButtonNormal;
    [SerializeField] private Button prevButtonNormal;
    [SerializeField] private Button fullscreenButton;

    [SerializeField] private Button playPauseButtonFullscreen;
    [SerializeField] private Button nextButtonFullscreen;
    [SerializeField] private Button prevButtonFullscreen;
    [SerializeField] private Button exitFullscreenButton;

    // Estado interno
    private List<VideoClip> currentPlaylist = new List<VideoClip>();
    private int currentIndex = 0;
    private bool hasVideos = false;
    private string currentDecadeTag = "";
    private bool isFullscreen = false;

    private VideoPlayer ActivePlayer => isFullscreen ? fullscreenPlayer : normalPlayer;

    private void Start()
    {
        if (verVideosButton != null)
            verVideosButton.SetActive(false);

        panelNormal.SetActive(false);
        panelFullscreen.SetActive(false);

        if (infoText) infoText.text = "";
        if (decadeTitleText) decadeTitleText.text = "";
        if (verVideosButtonText != null)
            verVideosButtonText.text = "Ver videos";

        // Asignar eventos de botones
        if (playPauseButtonNormal) playPauseButtonNormal.onClick.AddListener(OnPlayPause);
        if (nextButtonNormal) nextButtonNormal.onClick.AddListener(OnNextVideo);
        if (prevButtonNormal) prevButtonNormal.onClick.AddListener(OnPrevVideo);
        if (fullscreenButton) fullscreenButton.onClick.AddListener(OnEnterFullscreen);

        if (playPauseButtonFullscreen) playPauseButtonFullscreen.onClick.AddListener(OnPlayPause);
        if (nextButtonFullscreen) nextButtonFullscreen.onClick.AddListener(OnNextVideo);
        if (prevButtonFullscreen) prevButtonFullscreen.onClick.AddListener(OnPrevVideo);
        if (exitFullscreenButton) exitFullscreenButton.onClick.AddListener(OnExitFullscreen);
    }

    // Este script va en la MainCamera (con un collider "normal", y los triggers en otros objetos)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("video70s") ||
            other.CompareTag("video80s") ||
            other.CompareTag("video90s") ||
            other.CompareTag("video00s") ||
            other.CompareTag("video10s"))
        {
            currentDecadeTag = other.tag;
            ConfigurarPlaylistSegunTag(currentDecadeTag);

            if (verVideosButton != null)
                verVideosButton.SetActive(true);

            if (verVideosButtonText != null)
                verVideosButtonText.text = "Ver videos";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == currentDecadeTag)
        {
            currentDecadeTag = "";
            CerrarTodo(); // aquí sí queremos cerrar y ocultar el botón
        }
    }

    private void ConfigurarPlaylistSegunTag(string tagDecada)
    {
        currentPlaylist.Clear();

        switch (tagDecada)
        {
            case "video70s":
                currentPlaylist.AddRange(videos70s);
                if (decadeTitleText) decadeTitleText.text = "Década de 1970";
                break;
            case "video80s":
                currentPlaylist.AddRange(videos80s);
                if (decadeTitleText) decadeTitleText.text = "Década de 1980";
                break;
            case "video90s":
                currentPlaylist.AddRange(videos90s);
                if (decadeTitleText) decadeTitleText.text = "Década de 1990";
                break;
            case "video00s":
                currentPlaylist.AddRange(videos00s);
                if (decadeTitleText) decadeTitleText.text = "Década de 2000";
                break;
            case "video10s":
                currentPlaylist.AddRange(videos10s);
                if (decadeTitleText) decadeTitleText.text = "Década de 2010";
                break;
        }

        hasVideos = currentPlaylist.Count > 0;
        currentIndex = 0;

        if (!hasVideos && tagDecada == "video70s")
        {
            if (infoText) infoText.text = "No hay videos disponibles para esta década.";
        }
        else
        {
            if (infoText) infoText.text = "";
        }
    }

    /// <summary>
    /// Método que se asigna al botón "Ver videos" en el OnClick.
    /// Ahora funciona como toggle: abre/cierra el panel.
    /// </summary>
    public void AbrirPanelVideos()
    {
        // Si ya hay un panel activo → este mismo botón sirve como "Cerrar"
        if (panelNormal.activeSelf || panelFullscreen.activeSelf)
        {
            CerrarPanelesManteniendoBoton();
            return;
        }

        // Si por alguna razón no estamos dentro de ninguna década, no hacemos nada
        if (string.IsNullOrEmpty(currentDecadeTag))
            return;

        isFullscreen = false;
        panelNormal.SetActive(true);
        panelFullscreen.SetActive(false);

        if (verVideosButtonText != null)
            verVideosButtonText.text = "Cerrar";

        if (hasVideos)
        {
            CargarClipEnAmbos(reiniciarTiempo: true, autoPlay: true, tiempoAnterior: 0);
        }
        else
        {
            if (normalPlayer) normalPlayer.Stop();
            if (fullscreenPlayer) fullscreenPlayer.Stop();
        }
    }

    /// <summary>
    /// Cierra panels y oculta el botón (cuando salimos del trigger).
    /// </summary>
    public void CerrarTodo()
    {
        CerrarPanelesBase();

        if (verVideosButton != null)
            verVideosButton.SetActive(false);
    }

    /// <summary>
    /// Cierra panels, pero mantiene visible el botón y lo deja en "Ver videos".
    /// (Se usa cuando el usuario presiona el mismo botón para cerrar.)
    /// </summary>
    private void CerrarPanelesManteniendoBoton()
    {
        CerrarPanelesBase();

        if (verVideosButton != null)
            verVideosButton.SetActive(true);

        if (verVideosButtonText != null)
            verVideosButtonText.text = "Ver videos";
    }

    /// <summary>
    /// Lógica compartida para cerrar panels y detener videos.
    /// </summary>
    private void CerrarPanelesBase()
    {
        panelNormal.SetActive(false);
        panelFullscreen.SetActive(false);

        if (normalPlayer) normalPlayer.Stop();
        if (fullscreenPlayer) fullscreenPlayer.Stop();

        if (infoText) infoText.text = "";
        if (decadeTitleText) decadeTitleText.text = "";
    }

    // Carga el clip actual en ambos VideoPlayers (útil al cambiar de video)
    private void CargarClipEnAmbos(bool reiniciarTiempo, bool autoPlay, double tiempoAnterior)
    {
        if (!hasVideos || currentPlaylist.Count == 0) return;

        if (currentIndex < 0) currentIndex = 0;
        if (currentIndex >= currentPlaylist.Count) currentIndex = currentPlaylist.Count - 1;

        VideoClip clip = currentPlaylist[currentIndex];

        if (normalPlayer)
        {
            normalPlayer.clip = clip;
            normalPlayer.time = reiniciarTiempo ? 0 : tiempoAnterior;
            if (autoPlay) normalPlayer.Play(); else normalPlayer.Pause();
        }

        if (fullscreenPlayer)
        {
            fullscreenPlayer.clip = clip;
            fullscreenPlayer.time = reiniciarTiempo ? 0 : tiempoAnterior;
            if (autoPlay) fullscreenPlayer.Play(); else fullscreenPlayer.Pause();
        }
    }

    // ===== Controles compartidos =====

    public void OnPlayPause()
    {
        if (!hasVideos || ActivePlayer == null) return;

        if (ActivePlayer.isPlaying)
            ActivePlayer.Pause();
        else
            ActivePlayer.Play();
    }

    public void OnNextVideo()
    {
        if (!hasVideos || currentPlaylist.Count == 0) return;

        bool estabaReproduciendo = ActivePlayer != null && ActivePlayer.isPlaying;
        currentIndex++;
        if (currentIndex >= currentPlaylist.Count) currentIndex = 0;

        CargarClipEnAmbos(reiniciarTiempo: true, autoPlay: estabaReproduciendo, tiempoAnterior: 0);
    }

    public void OnPrevVideo()
    {
        if (!hasVideos || currentPlaylist.Count == 0) return;

        bool estabaReproduciendo = ActivePlayer != null && ActivePlayer.isPlaying;
        currentIndex--;
        if (currentIndex < 0) currentIndex = currentPlaylist.Count - 1;

        CargarClipEnAmbos(reiniciarTiempo: true, autoPlay: estabaReproduciendo, tiempoAnterior: 0);
    }

    public void OnEnterFullscreen()
    {
        if (!hasVideos || normalPlayer == null || fullscreenPlayer == null) return;

        isFullscreen = true;

        double tiempoActual = normalPlayer.time;
        bool estabaReproduciendo = normalPlayer.isPlaying;

        fullscreenPlayer.clip = currentPlaylist[currentIndex];
        fullscreenPlayer.time = tiempoActual;

        if (estabaReproduciendo) fullscreenPlayer.Play();
        else fullscreenPlayer.Pause();

        panelNormal.SetActive(false);
        panelFullscreen.SetActive(true);
    }

    public void OnExitFullscreen()
    {
        if (!hasVideos || normalPlayer == null || fullscreenPlayer == null) return;

        isFullscreen = false;

        double tiempoActual = fullscreenPlayer.time;
        bool estabaReproduciendo = fullscreenPlayer.isPlaying;

        normalPlayer.clip = currentPlaylist[currentIndex];
        normalPlayer.time = tiempoActual;

        if (estabaReproduciendo) normalPlayer.Play();
        else normalPlayer.Pause();

        panelFullscreen.SetActive(false);
        panelNormal.SetActive(true);
    }
}
