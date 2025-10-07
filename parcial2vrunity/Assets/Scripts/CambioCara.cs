using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CambioCara : MonoBehaviour
{

    public ARFaceManager faceManager;

    public string nombreFiltro1 = "Filtro Brujita";
    public string nombreFiltro2 = "Filtro Payasito";

    [Header("Sonidos")]
    public AudioSource sonidoFiltro1;
    public AudioSource sonidoFiltro2;

    void Start()
    {
        faceManager = FindObjectOfType<ARFaceManager>();
    }

    public void ActivarModo1()
    {
        foreach (ARFace cara in faceManager.trackables)
        {
            var filtro1 = cara.transform.Find(nombreFiltro1);
            var filtro2 = cara.transform.Find(nombreFiltro2);

            if (filtro1 != null)
            {
                filtro1.gameObject.SetActive(true);
                if (sonidoFiltro1 != null) sonidoFiltro1.Play();
            }
            else Debug.LogWarning("No se encontró Filtro1 en " + cara.name);

            if (filtro2 != null) filtro2.gameObject.SetActive(false);
        }
    }

    public void ActivarModo2()
    {
        foreach (ARFace cara in faceManager.trackables)
        {
            var filtro1 = cara.transform.Find(nombreFiltro1);
            var filtro2 = cara.transform.Find(nombreFiltro2);

            if (filtro1 != null) filtro1.gameObject.SetActive(false);

            if (filtro2 != null)
            {
                filtro2.gameObject.SetActive(true);
                if (sonidoFiltro2 != null) sonidoFiltro2.Play();
            }
            else Debug.LogWarning("No se encontró Filtro2 en " + cara.name);
        }
    }


    public void DesactivarFiltros()
    {
        foreach (ARFace cara in faceManager.trackables)
        {
            var filtro1 = cara.transform.Find(nombreFiltro1);
            var filtro2 = cara.transform.Find(nombreFiltro2);

            if (filtro1 != null) filtro1.gameObject.SetActive(false);
            if (filtro2 != null) filtro2.gameObject.SetActive(false);
        }
    }
}
