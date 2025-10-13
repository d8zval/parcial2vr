using UnityEngine;

public class PanelRaycastController : MonoBehaviour
{
    public Camera mainCamera; // Cámara AR
    public float maxDistance = 10f;
    public LayerMask panelLayer; // Capa "Cartel"

    private GameObject cartelActual;

    void Update()
    {
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, panelLayer))
        {
            GameObject nuevoCartel = hit.collider.gameObject;

            if (nuevoCartel != cartelActual)
            {
                // Desactiva el parallax del anterior
                if (cartelActual != null)
                    CambiarParallax(cartelActual, false);

                // Activa el parallax del nuevo
                cartelActual = nuevoCartel;
                CambiarParallax(cartelActual, true);
            }
        }
        else
        {
            // Si no está mirando ningún cartel
            if (cartelActual != null)
            {
                CambiarParallax(cartelActual, false);
                cartelActual = null;
            }
        }
    }

    void CambiarParallax(GameObject cartel, bool activo)
    {
        var parallax = cartel.GetComponentsInChildren<ParallaxFloat>();
        foreach (var p in parallax)
        {
            p.parallaxActivo = activo;
        }
    }
}
