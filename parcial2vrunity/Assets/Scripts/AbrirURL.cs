using UnityEngine;

public class AbrirURL : MonoBehaviour
{
    public string perfilURL = "https://www.instagram.com/d8zval";

    public void AbrirPerfil()
    {
        Application.OpenURL(perfilURL);
    }
}
