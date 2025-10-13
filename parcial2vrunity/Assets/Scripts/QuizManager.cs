using UnityEngine;
using UnityEngine.UI;
using TMPro; // Si usas TextMeshPro
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
    public TextMeshProUGUI textoPreguntaUI; // O Text si no usas TMP
    public Button[] botonesOpciones;
    public GameObject panelQuiz;
    public GameObject panelModulos;

    [Header("Datos")]
    public Modulo[] modulos = new Modulo[5]; // 5 módulos

    private int moduloActual = 0;
    private int indicePreguntaActual = 0;

    void Start()
    {
        panelQuiz.SetActive(false);
        panelModulos.SetActive(true);
    }

    public void SeleccionarModulo(int moduloIndex)
    {
        moduloActual = moduloIndex;
        indicePreguntaActual = 0;
        panelModulos.SetActive(false);
        panelQuiz.SetActive(true);
        MostrarPregunta();
    }

    void MostrarPregunta()
    {
        Pregunta p = modulos[moduloActual].preguntas[indicePreguntaActual];
        textoPreguntaUI.text = p.textoPregunta;

        for (int i = 0; i < botonesOpciones.Length; i++)
        {
            int index = i;
            botonesOpciones[i].GetComponentInChildren<TextMeshProUGUI>().text = p.opciones[i]; // O Text si no usas TMP
            botonesOpciones[i].onClick.RemoveAllListeners();
            botonesOpciones[i].onClick.AddListener(() => VerificarRespuesta(index));
        }
    }

    void VerificarRespuesta(int opcionSeleccionada)
    {
        bool esCorrecto = opcionSeleccionada == modulos[moduloActual].preguntas[indicePreguntaActual].respuestaCorrectaIndex;
        Debug.Log(esCorrecto ? "Correcto" : "Incorrecto");

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
}
