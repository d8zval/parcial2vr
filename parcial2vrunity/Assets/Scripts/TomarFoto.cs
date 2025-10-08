using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using NativeGalleryNamespace;
using System;

public class TomarFotoYGuardar : MonoBehaviour
{
    [Header("Referencias")]
    public Camera camaraAR;              // AR Camera
    public GameObject panelPreview;      // Panel que muestra la foto (inicialmente inactivo)
    public RawImage vistaPrevia;         // RawImage dentro del panel
    public Button botonCerrar;           // Botón "X" dentro del panel
    public Button botonGuardar;          // Botón "Guardar" dentro del panel
    public AudioClip sonidoFoto;       // Sonido opcional al tomar la foto
    public AudioSource sonidoCamara;

    private Texture2D imagenCapturada;
    private string rutaTemporal;
    private AudioSource audioSource;

    void Start()
    {
        if (panelPreview != null) panelPreview.SetActive(false);

        if (botonCerrar != null) botonCerrar.onClick.AddListener(CerrarPreview);
        if (botonGuardar != null) botonGuardar.onClick.AddListener(GuardarEnGaleria);

        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
    }

    // Llama esto desde el botón de "Tomar foto"
    public void TomarFoto()
    {
        StartCoroutine(CapturarFotoCoroutine());
    }

    private IEnumerator CapturarFotoCoroutine()
    {
        yield return new WaitForEndOfFrame();

        if (sonidoCamara != null && sonidoFoto != null)
        {
            sonidoCamara.PlayOneShot(sonidoFoto);
        }

        // RenderTexture para capturar lo que la AR Camera renderiza (incluye filtros)
        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
        camaraAR.targetTexture = rt;
        camaraAR.Render();

        RenderTexture.active = rt;
        Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        tex.Apply();

        // limpiar
        camaraAR.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        imagenCapturada = tex;

        // Guardado temporal (opcional)
        byte[] bytesTemp = imagenCapturada.EncodeToPNG();
        rutaTemporal = Path.Combine(Application.persistentDataPath, $"foto_AR_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png");
        File.WriteAllBytes(rutaTemporal, bytesTemp);
        Debug.Log("📸 Guardada temporal: " + rutaTemporal);

        // Mostrar preview
        MostrarPreview(imagenCapturada);
    }

    private void MostrarPreview(Texture2D imagen)
    {
        if (vistaPrevia != null)
        {
            vistaPrevia.texture = imagen;
            vistaPrevia.SetNativeSize();
        }

        if (panelPreview != null) panelPreview.SetActive(true);
    }

    public void CerrarPreview()
    {
        if (panelPreview != null) panelPreview.SetActive(false);
    }

    public void GuardarEnGaleria()
    {
        if (imagenCapturada == null)
        {
            Debug.LogWarning("No hay imagen capturada para guardar.");
            return;
        }

        byte[] bytes = imagenCapturada.EncodeToPNG();
        string filename = $"foto_AR_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";

#if UNITY_ANDROID || UNITY_IOS
        // Versión que espera byte[] y usa callback (no devuelve Permission)
        NativeGallery.SaveImageToGallery(bytes, "FotosAR", filename, (success, path) =>
        {
            if (success)
                Debug.Log("✅ Foto guardada en la galería: " + path);
            else
                Debug.LogWarning("⚠️ No se pudo guardar la foto en la galería (o permiso denegado).");
        });
#else
        // Fallback para editor/PC: guardamos en persistentDataPath
        string ruta = Path.Combine(Application.persistentDataPath, filename);
        File.WriteAllBytes(ruta, bytes);
        Debug.Log("💾 Foto guardada localmente: " + ruta);
#endif
    }
}
