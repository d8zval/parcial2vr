using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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

    [Header("Panel de Reinicio")]
    public GameObject panelReintentar; // Panel de mensaje "te has equivocado mucho"
    public Button botonAceptarReintentar; // Botón para volver a los módulos

    [Header("Vidas (Raw Images)")]
    public RawImage vidas3; // Imagen cuando tiene las 3 vidas
    public RawImage vidas2; // Imagen cuando tiene 2 vidas
    public RawImage vidas1; // Imagen cuando tiene 1 vida
    public RawImage vidas0; // Imagen cuando no tiene vidas

    [Header("Datos del Quiz")]
    public Modulo[] modulos = new Modulo[5];

    private int moduloActual = 0;
    private int indicePreguntaActual = 0;
    private int vidasActuales = 3;

    void Start()
    {
        panelQuiz.SetActive(false);
        panelModulos.SetActive(true);
        panelReintentar.SetActive(false);

        ActualizarVidasUI();
        botonAceptarReintentar.onClick.AddListener(VolverAModulos);
    }

    public void SeleccionarModulo(int moduloIndex)
    {
        moduloActual = moduloIndex;
        indicePreguntaActual = 0;
        vidasActuales = 3;

        panelModulos.SetActive(false);
        panelQuiz.SetActive(true);
        panelReintentar.SetActive(false);

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
        bool esCorrecto = opcionSeleccionada == modulos[moduloActual].preguntas[indicePreguntaActual].respuestaCorrectaIndex;
        Debug.Log(esCorrecto ? "✅ Correcto" : "❌ Incorrecto");

        if (!esCorrecto)
        {
            vidasActuales--;
            ActualizarVidasUI();

            if (vidasActuales <= 0)
            {
                MostrarPanelReintentar();
                return;
            }
        }

        indicePreguntaActual++;
        if (indicePreguntaActual < modulos[moduloActual].preguntas.Length)
        {
            MostrarPregunta();
        }
        else
        {
            Debug.Log("FIN DEL MÓDULO: " + modulos[moduloActual].nombreModulo);
            panelQuiz.SetActive(false);
            panelModulos.SetActive(true);
        }
    }

    void ActualizarVidasUI()
    {
        // Desactivar todas las imágenes primero
        vidas3.gameObject.SetActive(false);
        vidas2.gameObject.SetActive(false);
        vidas1.gameObject.SetActive(false);
        vidas0.gameObject.SetActive(false);

        // Activar solo la que corresponde
        if (vidasActuales >= 3)
            vidas3.gameObject.SetActive(true);
        else if (vidasActuales == 2)
            vidas2.gameObject.SetActive(true);
        else if (vidasActuales == 1)
            vidas1.gameObject.SetActive(true);
        else
            vidas0.gameObject.SetActive(true);
    }

    void MostrarPanelReintentar()
    {
        panelQuiz.SetActive(false);
        panelReintentar.SetActive(true);
    }

    void VolverAModulos()
    {
        panelReintentar.SetActive(false);
        panelQuiz.SetActive(false);
        panelModulos.SetActive(true);
    }
}
