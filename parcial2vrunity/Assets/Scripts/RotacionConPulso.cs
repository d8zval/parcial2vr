using UnityEngine;

public class RotacionConPulso : MonoBehaviour
{
    [Header("Rotación base")]
    public Vector3 ejeRotacion = Vector3.up;
    public float velocidadRotacion = 20f;

    [Header("Pulso")]
    public float intensidadPulso = 10f;   // Qué tanto aumenta la rotación
    public float frecuenciaPulso = 2f;    // Velocidad del pulso

    void Update()
    {
        // Pulso suave usando seno
        float pulso = Mathf.Sin(Time.time * frecuenciaPulso) * intensidadPulso;

        // Rotación base + pulso
        float velocidadFinal = velocidadRotacion + pulso;

        transform.Rotate(ejeRotacion * velocidadFinal * Time.deltaTime);
    }
}
