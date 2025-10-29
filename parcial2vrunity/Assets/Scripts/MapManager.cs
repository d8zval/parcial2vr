using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Header("Posiciones donde se deben colocar los objetos recolectables")]
    public Transform[] spawnPoints; // Lista de puntos donde se colocarán los objetos recolectables

    // Instancia del Singleton
    private static MapManager instance;

    // Propiedad para acceder a la instancia
    public static MapManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MapManager>();  // Busca la instancia del MapManager
                if (instance == null)
                {
                    GameObject mapManagerObject = new GameObject("MapManager");
                    instance = mapManagerObject.AddComponent<MapManager>();
                    DontDestroyOnLoad(mapManagerObject);  // Mantiene el MapManager entre escenas
                }
            }
            return instance;
        }
    }

    // Método para colocar el objeto recolectable en la posición específica dentro del prefab del mapa
    public GameObject PlaceRecolectable(GameObject recolectablePrefab, int index)
    {
        if (index >= 0 && index < spawnPoints.Length)  // Asegurarse de que el índice esté dentro del rango
        {
            // Instanciamos el objeto en el punto de spawn correspondiente dentro del prefab
            GameObject recolectable = Instantiate(recolectablePrefab, spawnPoints[index].position, Quaternion.identity);

            // Aseguramos que el objeto recolectable sea hijo del prefab del mapa
            recolectable.transform.SetParent(spawnPoints[index].parent);  // Colocamos como hijo del mapa

            recolectable.SetActive(true); // Asegúrate de que el objeto esté activo
            Debug.Log($"Objeto recolectable colocado en: {spawnPoints[index].position}");
            return recolectable;
        }
        else
        {
            Debug.LogError("Índice fuera de rango. No se puede colocar el objeto recolectable.");
            return null;
        }
    }
}
