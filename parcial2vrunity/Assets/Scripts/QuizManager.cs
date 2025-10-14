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
    public GameObject estrellaContenedor; //Contenedor de la estrella completa
    public GameObject[] puntasEstrella;   //Cada punta corresponde a un módulo
    public Button botonContinuarFinal;    //Botón que aparece cuando la estrella está completa
    public Transform posicionFinalEstrella; //Punto donde la estrella debe moverse (Transform vacío en la escena)

    [Header("Datos del Quiz")]
    public Modulo[] modulos = new Modulo[5];
    public GameObject[] modulosVisuales;  //Sprites o paneles visuales de cada módulo

    private int moduloActual = 0;
    private int indicePreguntaActual = 0;
    private int vidasActuales = 3;
    private bool seEquivocoEnModulo = false;

    void Start()
    {
        panelQuiz.SetActive(false);
        panelModulos.SetActive(true);
        panelReintentar.SetActive(false);
        panelFelicitaciones.SetActive(false);
        panelCompletadoConErrores.SetActive(false);
        botonContinuarFinal.gameObject.SetActive(false);

        ActualizarVidasUI();
        ActualizarEstrella();

        // Asignar botones
        botonAceptarReintentar.onClick.AddListener(VolverAModulos);
        botonAceptarFelicitaciones.onClick.AddListener(VolverAModulos);
        botonAceptarCompletado.onClick.AddListener(VolverAModulos);
        botonContinuarFinal.onClick.AddListener(IrAEscenaFinal);
    }

    public void SeleccionarModulo(int moduloIndex)
    {
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
            botonesOpciones[i].onClick.AddListener(() => VerificarRespuesta(index));
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
            ActualizarVidasUI();

            if (vidasActuales <= 0)
            {
                MostrarPanelReintentar();
                return;
            }

            // Reintentar la misma pregunta
            MostrarPregunta();
            return;
        }

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
            // Asegurarse de que la estrella esté activa para poder mostrar la punta
            if (estrellaContenedor != null && !estrellaContenedor.activeSelf)
            {
                estrellaContenedor.SetActive(true);
                Debug.Log("Activando contenedor de estrella automáticamente.");
            }

            ActivarPuntaDeModulo(moduloActual);
            panelFelicitaciones.SetActive(true);
        }
        else if (vidasActuales > 0)
        {
            // Completado con errores
            panelCompletadoConErrores.SetActive(true);
        }
        else
        {
            // Perdió todas las vidas
            MostrarPanelReintentar();
        }

        // Si la estrella ya está completa, ejecutar animación final
        if (EstrellaCompleta())
        {
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
    }

    void VolverAModulos()
    {
        panelReintentar.SetActive(false);
        panelFelicitaciones.SetActive(false);
        panelCompletadoConErrores.SetActive(false);
        panelQuiz.SetActive(false);
        panelModulos.SetActive(true);
    }

    // ---------- LÓGICA DE ESTRELLA POR MÓDULO ----------
    void ActivarPuntaDeModulo(int index)
    {
        if (puntasEstrella == null || index >= puntasEstrella.Length)
            return;

        puntasEstrella[index].SetActive(true);
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

        // Desaparecer módulos visuales
        foreach (GameObject mod in modulosVisuales)
            mod.SetActive(false);

        // Espera breve antes de mover la estrella
        yield return new WaitForSeconds(0.8f);

        // Mover suavemente la estrella hacia la posición final
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

        // Espera y luego mostrar botón final
        yield return new WaitForSeconds(0.5f);
        botonContinuarFinal.gameObject.SetActive(true);
        Debug.Log("Transición final completada.");
    }

    void IrAEscenaFinal()
    {
        Debug.Log("Cargando escena final...");
        SceneManager.LoadScene("FiltrosEscena"); //Cambia por el nombre real de tu escena
    }
}
