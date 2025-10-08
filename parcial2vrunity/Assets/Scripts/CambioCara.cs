using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CambioCara : MonoBehaviour
{

    [Header("Configuración del filtro")]
    public string nombreFiltro;                // Nombre del filtro (objeto hijo del ARFace)
    public AudioClip sonidoActivar;            // Sonido al activar
    public AudioClip sonidoDesactivar;         // Sonido al desactivar (opcional)
    private AudioSource audioSource;           // Fuente de audio interna

    [Header("Referencias")]
    public ARFaceManager faceManager;          // Referencia al ARFaceManager
    public Button botonActivar;                // Botón que activa el filtro
    public Button botonDesactivar;             // Botón gris que desactiva el filtro

    void Start()
    {
        if (faceManager == null)
            faceManager = FindObjectOfType<ARFaceManager>();

        // Crear o encontrar AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // Asignar eventos de botones
        if (botonActivar != null)
            botonActivar.onClick.AddListener(ActivarFiltro);

        if (botonDesactivar != null)
            botonDesactivar.onClick.AddListener(DesactivarFiltro);

        // Solo el botón de activar visible al inicio
        if (botonActivar != null) botonActivar.gameObject.SetActive(true);
        if (botonDesactivar != null) botonDesactivar.gameObject.SetActive(false);
    }

    public void ActivarFiltro()
    {
        foreach (ARFace cara in faceManager.trackables)
        {
            var filtro = cara.transform.Find(nombreFiltro);
            if (filtro != null)
                filtro.gameObject.SetActive(true);
            else
                Debug.LogWarning("No se encontró el filtro: " + nombreFiltro + " en " + cara.name);
        }

        // Reproducir sonido
        if (sonidoActivar != null)
            audioSource.PlayOneShot(sonidoActivar);

        // Cambiar visibilidad de botones
        botonActivar.gameObject.SetActive(false);
        botonDesactivar.gameObject.SetActive(true);
    }

    public void DesactivarFiltro()
    {
        foreach (ARFace cara in faceManager.trackables)
        {
            var filtro = cara.transform.Find(nombreFiltro);
            if (filtro != null)
                filtro.gameObject.SetActive(false);
        }

        // Reproducir sonido
        if (sonidoDesactivar != null)
            audioSource.PlayOneShot(sonidoDesactivar);

        // Cambiar visibilidad de botones
        botonActivar.gameObject.SetActive(true);
        botonDesactivar.gameObject.SetActive(false);
    }
}
