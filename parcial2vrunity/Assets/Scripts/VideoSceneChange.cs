using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoSceneChange : MonoBehaviour
{
    [Header("Nombre de la siguiente escena")]
    [SerializeField] private string nextSceneName = "MainMenu";

    private VideoPlayer videoPlayer;

    void Start()
    {
        // Buscar el componente VideoPlayer en el mismo GameObject
        videoPlayer = GetComponent<VideoPlayer>();

        if (videoPlayer != null)
        {
            // Suscribirnos al evento que se dispara al finalizar el video
            videoPlayer.loopPointReached += OnVideoEnd;
        }
        else
        {
            Debug.LogError("No se encontró un componente VideoPlayer en este objeto.");
        }
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("Video finalizado. Cargando siguiente escena...");
        SceneManager.LoadScene(nextSceneName);
    }
}
