using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (cam != null)
        {
            Vector3 direccion = transform.position - cam.transform.position;
            direccion.y = 0; // evita que se incline hacia arriba o abajo
            transform.rotation = Quaternion.LookRotation(direccion);
        }
    }
}
