using UnityEngine;

public class ParallaxFloat : MonoBehaviour
{
    public float intensidad = 0.5f;
    public float suavizado = 5f;
    [HideInInspector] public bool parallaxActivo = false;

    private Vector3 posicionInicial;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        posicionInicial = transform.localPosition;
    }

    void Update()
    {
        if (cam == null) return;

        if (parallaxActivo)
        {
            Vector3 dir = cam.transform.position - transform.position;
            Vector3 desplazamiento = new Vector3(-dir.x, -dir.y, 0) * intensidad;
            Vector3 destino = posicionInicial + desplazamiento;
            transform.localPosition = Vector3.Lerp(transform.localPosition, destino, Time.deltaTime * suavizado);
        }
        else
        {
            // Vuelve suavemente a la posición inicial cuando no está activo
            transform.localPosition = Vector3.Lerp(transform.localPosition, posicionInicial, Time.deltaTime * suavizado);
        }
    }
}
