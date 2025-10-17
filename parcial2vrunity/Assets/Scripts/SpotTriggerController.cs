using System;
using UnityEngine;
using UnityEngine.UI;

public class SpotTriggerController : MonoBehaviour
{
    public GameObject boton;



    private void Awake()
    {
        boton = GetComponent<GameObject>();
        boton.SetActive(false);
    }
    void OnTriggerEnter(Collider other)
    {
        // Si entra la cámara (o el jugador)
        if (other.CompareTag("MainCamera"))
        {
            boton.SetActive(true);
        }
        else
        {
            boton.SetActive(false);
        }
    }



}
