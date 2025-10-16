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

        // 🔹 Cuando el progreso llegue a 100%, desbloquear el botón
        if (progress >= 1f && module.botonModulo != null && !module.botonModulo.estaDesbloqueado)
        {
            module.botonModulo.Desbloquear();
            Debug.Log($"Módulo '{module.moduleName}' completado. Botón desbloqueado.");
        }
    }
}