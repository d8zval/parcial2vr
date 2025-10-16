using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

[System.Serializable]
public class Pregunta
{
    public string textoPregunta;
    public string[] opciones = new string[4];
    public int respuestaCorrectaIndex;
}

[System.Serializable]
public class Modulo
{
    public string nombreModulo;
    public Pregunta[] preguntas = new Pregunta[4];
}

public class QuizManager : MonoBehaviour
{
    [Header("UI Referencias")]
    public TextMeshProUGUI textoPreguntaUI;
    public Button[] botonesOpciones;
    public GameObject panelQuiz;
    public GameObject panelModulos;

    [Header("Paneles de resultado")]
    public GameObject panelReintentar;
    public Button botonAceptarReintentar;

    public GameObject panelFelicitaciones;
    public Button botonAceptarFelicitaciones;

    public GameObject panelCompletadoConErrores;
    public Button botonAceptarCompletado;

    [Header("Vidas (Raw Images)")]
    public RawImage vidas3;
    public RawImage vidas2;
    public RawImage vidas1;
    public RawImage vidas0;

    [Header("Estrella de progreso (una punta por módulo)")]
    public GameObject estrellaContenedor;
    public GameObject[] puntasEstrella;
    public Button botonContinuarFinal;
    public Transform posicionFinalEstrella;

    [Header("Datos del Quiz")]
    public Modulo[] modulos = new Modulo[5];
    public GameObject[] modulosVisuales;

    [Header("Audio Clips")]
    public AudioSource audioSourceEfectos;       // Para efectos de sonido
    public AudioSource audioSourceMusicaFondo;   // Para la música de fondo

    [Space(10)]
    public AudioClip sonidoBoton;                // Cada vez que se presiona un botón
    public AudioClip sonidoVidaPerdida;          // Cuando pierde una vida
    public AudioClip sonidoRespuestaCorrecta;    // Cuando responde bien
    public AudioClip sonidoPanelReintentar;      // Cuando aparece panel de reintentar
    public AudioClip sonidoPanelFelicitaciones;  // Cuando aparece panel de felicitaciones
    public AudioClip sonidoAceptarReintentar;    // Botón aceptar en reintentar
    public AudioClip sonidoAceptarFelicitaciones;// Botón aceptar en felicitaciones
    public AudioClip sonidoEstrellaParte;        // Cuando se activa una parte de estrella
    public AudioClip sonidoEstrellaCompleta;     // Cuando la estrella se completa
    public AudioClip sonidoAnimacionEstrella;    // Durante animación final
    public AudioClip musicaDeFondo;              // Música del quiz

    private int moduloActual = 0;
    private int indicePreguntaActual = 0;
    private int vidasActuales = 3;
    private bool seEquivocoEnModulo = false;

    void Start()
    {
        // Iniciar música de fondo si existe
        if (audioSourceMusicaFondo != null && musicaDeFondo != null)
        {
            audioSourceMusicaFondo.clip = musicaDeFondo;
            audioSourceMusicaFondo.loop = true;
        }

        panelQuiz.SetActive(false);
        panelModulos.SetActive(true);
        panelReintentar.SetActive(false);
        panelFelicitaciones.SetActive(false);
        panelCompletadoConErrores.SetActive(false);
        botonContinuarFinal.gameObject.SetActive(false);

        ActualizarVidasUI();
        ActualizarEstrella();

        // Asignar botones
        botonAceptarReintentar.onClick.AddListener(() => {
            ReproducirSonido(sonidoAceptarReintentar);
            VolverAModulos();
        });

        botonAceptarFelicitaciones.onClick.AddListener(() => {
            ReproducirSonido(sonidoAceptarFelicitaciones);
            VolverAModulos();
        });

        botonAceptarCompletado.onClick.AddListener(() => {
            ReproducirSonido(sonidoBoton);
            VolverAModulos();
        });

        botonContinuarFinal.onClick.AddListener(() => {
            ReproducirSonido(sonidoBoton);
            IrAEscenaFinal();
        });
    }

    public void SeleccionarModulo(int moduloIndex)
    {
        audioSourceMusicaFondo.Play();
        moduloActual = moduloIndex;
        indicePreguntaActual = 0;
        vidasActuales = 3;
        seEquivocoEnModulo = false;

        panelModulos.SetActive(false);
        panelQuiz.SetActive(true);
        panelReintentar.SetActive(false);
        panelFelicitaciones.SetActive(false);
        panelCompletadoConErrores.SetActive(false);

        ActualizarVidasUI();
        MostrarPregunta();
    }

    void MostrarPregunta()
    {
        Pregunta p = modulos[moduloActual].preguntas[indicePreguntaActual];
        textoPreguntaUI.text = p.textoPregunta;

        for (int i = 0; i < botonesOpciones.Length; i++)
        {
            int index = i;
            botonesOpciones[i].GetComponentInChildren<TextMeshProUGUI>().text = p.opciones[i];
            botonesOpciones[i].onClick.RemoveAllListeners();
            botonesOpciones[i].onClick.AddListener(() =>
            {
                ReproducirSonido(sonidoBoton);
                VerificarRespuesta(index);
            });
        }
    }

    void VerificarRespuesta(int opcionSeleccionada)
    {
        Pregunta preguntaActual = modulos[moduloActual].preguntas[indicePreguntaActual];
        bool esCorrecto = opcionSeleccionada == preguntaActual.respuestaCorrectaIndex;
        Debug.Log(esCorrecto ? "Correcto" : "Incorrecto");

        if (!esCorrecto)
        {
            seEquivocoEnModulo = true;
            vidasActuales--;
            ReproducirSonido(sonidoVidaPerdida);
            ActualizarVidasUI();

            if (vidasActuales <= 0)
            {
                MostrarPanelReintentar();
                return;
            }

            MostrarPregunta();
            return;
        }

        ReproducirSonido(sonidoRespuestaCorrecta);
        indicePreguntaActual++;
        if (indicePreguntaActual < modulos[moduloActual].preguntas.Length)
        {
            MostrarPregunta();
        }
        else
        {
            FinalizarModulo();
        }
    }

    void FinalizarModulo()
    {
        Debug.Log($"Fin del módulo: {modulos[moduloActual].nombreModulo}");
        panelQuiz.SetActive(false);

        // 🔹 Si completó perfecto
        if (!seEquivocoEnModulo && vidasActuales == 3)
        {
            if (estrellaContenedor != null && !estrellaContenedor.activeSelf)
                estrellaContenedor.SetActive(true);

            ActivarPuntaDeModulo(moduloActual);
            panelFelicitaciones.SetActive(true);
            ReproducirSonido(sonidoPanelFelicitaciones);
        }
        else if (vidasActuales > 0)
        {
            panelCompletadoConErrores.SetActive(true);
        }
        else
        {
            MostrarPanelReintentar();
        }

        // Si la estrella ya está completa
        if (EstrellaCompleta())
        {
            ReproducirSonido(sonidoEstrellaCompleta);
            StartCoroutine(MostrarTransicionFinal());
        }
    }

    void ActualizarVidasUI()
    {
        vidas3.gameObject.SetActive(vidasActuales >= 3);
        vidas2.gameObject.SetActive(vidasActuales == 2);
        vidas1.gameObject.SetActive(vidasActuales == 1);
        vidas0.gameObject.SetActive(vidasActuales <= 0);
    }

    void MostrarPanelReintentar()
    {
        panelQuiz.SetActive(false);
        panelReintentar.SetActive(true);
        ReproducirSonido(sonidoPanelReintentar);
    }

    void VolverAModulos()
    {
        panelReintentar.SetActive(false);
        panelFelicitaciones.SetActive(false);
        panelCompletadoConErrores.SetActive(false);
        panelQuiz.SetActive(false);
        panelModulos.SetActive(true);
    }

    // ---------- LÓGICA DE ESTRELLA ----------
    void ActivarPuntaDeModulo(int index)
    {
        if (puntasEstrella == null || index >= puntasEstrella.Length)
            return;

        puntasEstrella[index].SetActive(true);
        ReproducirSonido(sonidoEstrellaParte);
        Debug.Log($"Punta del módulo {index + 1} activada.");
    }

    void ActualizarEstrella()
    {
        foreach (GameObject punta in puntasEstrella)
            punta.SetActive(false);
    }

    bool EstrellaCompleta()
    {
        foreach (GameObject punta in puntasEstrella)
        {
            if (!punta.activeSelf) return false;
        }
        return true;
    }

    // 🌟 ---------- ANIMACIÓN FINAL ----------
    IEnumerator MostrarTransicionFinal()
    {
        Debug.Log("Estrella completa, iniciando transición final...");
        ReproducirSonido(sonidoAnimacionEstrella);

        foreach (GameObject mod in modulosVisuales)
            mod.SetActive(false);

        yield return new WaitForSeconds(0.8f);

        Vector3 startPos = estrellaContenedor.transform.position;
        Vector3 targetPos = posicionFinalEstrella.position;
        float t = 0;
        float duracion = 2f;

        while (t < 1)
        {
            t += Time.deltaTime / duracion;
            estrellaContenedor.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        botonContinuarFinal.gameObject.SetActive(true);
        Debug.Log("Transición final completada.");
    }

    void IrAEscenaFinal()
    {
        Debug.Log("Cargando escena final...");
        SceneManager.LoadScene("FiltrosEscena");
    }

    // ---------- FUNCIONES DE AUDIO ----------
    void ReproducirSonido(AudioClip clip)
    {
        if (audioSourceEfectos != null && clip != null)
            audioSourceEfectos.PlayOneShot(clip);
    }
}
