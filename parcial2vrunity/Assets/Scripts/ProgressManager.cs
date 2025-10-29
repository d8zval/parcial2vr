using UnityEngine;
using UnityEngine.UI;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager Instance;

    [System.Serializable]
    public class ModuleProgress
    {
        public string moduleName;
        public int totalPlates;
        [HideInInspector] public int platesActivated;
        public Image progressImage; // en vez del Slider
        public MenuButtonSpriteLogic botonModulo; // 🔹 botón que se desbloquea al completar

        public GameObject recolectablePrefab;  // Prefab del objeto recolectable
        public int spawnIndex; // Índice de la posición donde se debe colocar el objeto recolectable
    }

    public ModuleProgress[] modules;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateProgress(int moduleID)
    {
        if (moduleID < 0 || moduleID >= modules.Length)
            return;

        ModuleProgress module = modules[moduleID];

        module.platesActivated = Mathf.Min(module.platesActivated + 1, module.totalPlates);

        float progress = (float)module.platesActivated / module.totalPlates;

        if (module.progressImage != null)
            module.progressImage.fillAmount = progress;

        Debug.Log($"Progreso en {module.moduleName}: {progress * 100f}%");

        // Cuando el progreso llegue a 100%, desbloquear el botón y activar el objeto recolectable
        if (progress >= 1f && module.botonModulo != null && !module.botonModulo.estaDesbloqueado)
        {
            module.botonModulo.Desbloquear();
            Debug.Log($"Módulo '{module.moduleName}' completado. Botón desbloqueado.");

            // Instanciar el objeto recolectable solo si no ha sido instanciado aún
            if (module.recolectablePrefab != null)
            {
                // Llamamos al método de MapManager para colocar el objeto dentro del mapa en la posición correspondiente
                MapManager.Instance.PlaceRecolectable(module.recolectablePrefab, module.spawnIndex);
                Debug.Log($"Objeto recolectable instanciado: {module.recolectablePrefab.name}");
            }
            else
            {
                Debug.LogWarning("¡No se pudo instanciar el objeto recolectable porque el prefab no está asignado!");
            }
        }
    }
}
