using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RecorridoUAO : MonoBehaviour
{
    [Header("Audio y Control")]
    public AudioSource audioSource;
    public AudioClip[] audios; // 6 audios en orden

    [Header("Imágenes")]
    public Image imagenMostrada;
    public Sprite[] imagenes; // 6 imágenes correspondientes a cada audio

    [Header("Subtítulos")]
    public Text subtituloText;

    [Header("Botón Omitir")]
    public Button botonOmitir;

    private bool omitido = false;

    void Start()
    {
        // Asignar evento al botón
        if (botonOmitir != null)
            botonOmitir.onClick.AddListener(OmitirRecorrido);

        StartCoroutine(ReproducirRecorrido());
    }

    void OmitirRecorrido()
    {
        omitido = true;
        audioSource.Stop();
        subtituloText.text = "";
        if (imagenMostrada != null)
            imagenMostrada.enabled = false;
        if (botonOmitir != null)
            botonOmitir.gameObject.SetActive(false);
    }

    IEnumerator ReproducirRecorrido()
    {
        // ---------- AUDIO 1 ----------
        yield return StartCoroutine(ReproducirAudio(0, new (float, float, string)[]
        {
            (0f, 9f, "¡Hola, hola! ¡Soy Laia, tu guía en este recorrido por la historia de la Universidad Autónoma de Occidente!"),
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
        if (botonOmitir != null)
            botonOmitir.gameObject.SetActive(false);
    }

    IEnumerator ReproducirAudio(int indice, (float inicio, float fin, string texto)[] lineas)
    {
        if (omitido) yield break;

        imagenMostrada.sprite = imagenes[indice];
        imagenMostrada.enabled = true;

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
                    subtituloText.text = texto;
                }
                else if (t >= fin)
                {
                    idx++;
                }
            }

            yield return null;
        }

        subtituloText.text = "";
        yield return null;
    }
}
