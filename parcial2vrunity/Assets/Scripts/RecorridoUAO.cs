using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RecorridoUAO : MonoBehaviour
{
    [Header("Audio y Control")]
    public AudioSource audioSource;
    public AudioClip[] audios; // 6 audios

    [Header("Imágenes")]
    public GameObject[] imagenes; // 6 GameObjects diferentes (uno por audio)

    [Header("Subtítulos")]
    public Text subtituloText;

    [Header("Botón Omitir")]
    public Button botonOmitir;

    [Header("Panel Principal del Recorrido")]
    public GameObject panelRecorrido; // Panel que se desactiva al final o al omitir

    private bool omitido = false;

    void Start()
    {
        if (botonOmitir != null)
            botonOmitir.onClick.AddListener(OmitirRecorrido);

        StartCoroutine(ReproducirRecorrido());
    }

    void OmitirRecorrido()
    {
        omitido = true;
        audioSource.Stop();
        subtituloText.text = "";
        DesactivarTodasLasImagenes();
        if (panelRecorrido != null) panelRecorrido.SetActive(false);
    }

    IEnumerator ReproducirRecorrido()
    {
        // ---------- AUDIO 1 ----------
        yield return StartCoroutine(ReproducirAudio(0, new (float, float, string)[]
        {
            (0f, 9f, "¡Hola, hola! ¡Soy LaIA, tu guía en este recorrido por la historia de la Universidad Autónoma de Occidente!"),
            (9f, 21f, "Hoy te invito a acompañarme para celebrar los 50 años de la UAO, una historia llena de sueños, logros y mucho corazón."),
            (21f, 33f, "Durante este recorrido, encontrarás varios stands con información sobre diferentes momentos y proyectos importantes de la universidad.")
        }));

        if (omitido) yield break;

        // ---------- AUDIO 2 ----------
        yield return StartCoroutine(ReproducirAudio(1, new (float, float, string)[]
        {
            (0f, 5f, "Para guardar tu progreso, solo tienes que pasar por encima de las placas rojas."),
            (5f, 9f, "¡Verás cómo se vuelven verdes cuando las pises!"),
            (9f, 22f, "Cuando todas las placas de un stand estén verdes, habrás alcanzado el 100% de progreso en ese stand y se desbloqueará un quiz con preguntas sobre lo que aprendiste.")
        }));

        if (omitido) yield break;

        // ---------- AUDIO 3 ----------
        yield return StartCoroutine(ReproducirAudio(2, new (float, float, string)[]
        {
            (0f, 6f, "Hay cinco stands en total, así que ¡prepárate para explorar mucho!"),
            (6f, 16f, "Pero ojo, si pierdes una vida, tendrás que volver a hacer el quiz de ese stand para poder completar el recorrido de esa década.")
        }));

        if (omitido) yield break;

        // ---------- AUDIO 4 ----------
        yield return StartCoroutine(ReproducirAudio(3, new (float, float, string)[]
        {
            (0f, 5f, "Cada vez que superes un quiz, ganarás una parte de la Estrella Autónoma."),
            (5f, 17f, "Y cuando completes sus 5 partes, podrás acceder a un espacio especial para tomarte una foto conmemorativa de los 50 años de la UAO.")
        }));

        if (omitido) yield break;

        // ---------- AUDIO 5 ----------
        yield return StartCoroutine(ReproducirAudio(4, new (float, float, string)[]
        {
            (0f, 5f, "Desde ahí podrás descargar tu foto y descubrir más contenido,"),
            (5f, 11f, "como un podcast y videos muy interesantes sobre nuestra universidad.")
        }));

        if (omitido) yield break;

        // ---------- AUDIO 6 ----------
        yield return StartCoroutine(ReproducirAudio(5, new (float, float, string)[]
        {
            (0f, 1.5f, "¡Así que vamos!"),
            (1.5f, 10f, "Aprende, juega y celebra conmigo estos 50 años de autonomía, conocimiento y orgullo UAO.")
        }));

        subtituloText.text = "";
        if (panelRecorrido != null) panelRecorrido.SetActive(false);
    }

    IEnumerator ReproducirAudio(int indice, (float inicio, float fin, string texto)[] lineas)
    {
        if (omitido) yield break;

        DesactivarTodasLasImagenes();
        imagenes[indice].SetActive(true);

        audioSource.clip = audios[indice];
        audioSource.Play();

        float tiempoInicio = Time.time;
        int idx = 0;

        while (audioSource.isPlaying && !omitido)
        {
            float t = Time.time - tiempoInicio;

            if (idx < lineas.Length)
            {
                var (inicio, fin, texto) = lineas[idx];
                if (t >= inicio && t < fin)
                {
                    // Mostrar texto con efecto adaptado al tiempo disponible
                    yield return StartCoroutine(MostrarTextoEscribiendo(texto, fin - inicio));
                    // Esperar a que se cumpla el tiempo restante si sobró
                    float tiempoRestante = fin - (Time.time - tiempoInicio);
                    if (tiempoRestante > 0) yield return new WaitForSeconds(tiempoRestante);
                    idx++;
                }
            }

            yield return null;
        }

        subtituloText.text = "";
    }

    IEnumerator MostrarTextoEscribiendo(string texto, float duracion)
    {
        subtituloText.text = "";

        if (string.IsNullOrEmpty(texto) || duracion <= 0)
            yield break;

        float tiempoPorLetra = duracion / texto.Length;

        for (int i = 0; i < texto.Length; i++)
        {
            if (omitido) yield break;
            subtituloText.text = texto.Substring(0, i + 1);
            yield return new WaitForSeconds(tiempoPorLetra);
        }
    }

    void DesactivarTodasLasImagenes()
    {
        foreach (GameObject img in imagenes)
        {
            if (img != null)
                img.SetActive(false);
        }
    }
}
