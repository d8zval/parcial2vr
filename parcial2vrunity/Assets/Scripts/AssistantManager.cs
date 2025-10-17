using UnityEngine;
using System.Collections.Generic;

public class AssistantManager : MonoBehaviour
{
    public static AssistantManager Instance { get; private set; }

    [Header("Prefab del asistente")]
    [SerializeField] private GameObject assistantPrefab;

    [Header("Orientación al aparecer")]
    [SerializeField] private Transform lookAtTarget;   // ARCamera o jugador
    [SerializeField] private bool faceTargetOnSpawn = true;

    [Header("Pooling (opcional)")]
    [SerializeField] private bool usePooling = true;

    private readonly List<Transform> dynamicSpots = new List<Transform>();
    private GameObject currentAssistant;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        // DontDestroyOnLoad(gameObject); // si quieres que persista entre escenas
    }

    // ===== Registro dinámico de spots =====
    public void RegisterSpots(IEnumerable<Transform> newSpots)
    {
        if (newSpots == null) return;
        foreach (var t in newSpots)
            if (t != null && !dynamicSpots.Contains(t))
                dynamicSpots.Add(t);
    }

    public void UnregisterSpots(IEnumerable<Transform> removedSpots)
    {
        if (removedSpots == null) return;
        foreach (var t in removedSpots)
            dynamicSpots.Remove(t);
    }

    public int IndexOfSpot(Transform t) => dynamicSpots.IndexOf(t);

    public IReadOnlyList<Transform> GetRegisteredSpots() => dynamicSpots;

    // ===== Invocaciones =====
    public void SummonAtSpotID(int spotIndex)
    {
        if (spotIndex < 0 || spotIndex >= dynamicSpots.Count)
        {
            Debug.LogWarning($"[AssistantManager] spotIndex fuera de rango: {spotIndex}");
            return;
        }
        SpawnAt(dynamicSpots[spotIndex]);
    }

    public void SummonAtNearestTo(Vector3 position)
    {
        if (dynamicSpots.Count == 0) return;

        int bestIdx = -1; float bestSqr = float.MaxValue;
        for (int i = 0; i < dynamicSpots.Count; i++)
        {
            var s = dynamicSpots[i];
            if (s == null) continue;
            float d2 = (s.position - position).sqrMagnitude;
            if (d2 < bestSqr) { bestSqr = d2; bestIdx = i; }
        }
        if (bestIdx >= 0) SpawnAt(dynamicSpots[bestIdx]);
    }

    public void SummonAtNamed(string spotName)
    {
        for (int i = 0; i < dynamicSpots.Count; i++)
        {
            var s = dynamicSpots[i];
            if (s != null && s.name == spotName)
            {
                SpawnAt(s);
                return;
            }
        }
        Debug.LogWarning($"[AssistantManager] No se encontró spot con nombre '{spotName}'.");
    }

    // ===== Internos =====
    private void SpawnAt(Transform spot)
    {
        if (spot == null || assistantPrefab == null) return;

        if (usePooling)
        {
            if (currentAssistant == null) currentAssistant = Instantiate(assistantPrefab);
            currentAssistant.transform.SetPositionAndRotation(spot.position, spot.rotation);
            currentAssistant.SetActive(true);
        }
        else
        {
            if (currentAssistant != null) Destroy(currentAssistant);
            currentAssistant = Instantiate(assistantPrefab, spot.position, spot.rotation);
        }

        if (faceTargetOnSpawn && lookAtTarget != null)
        {
            Vector3 dir = lookAtTarget.position - currentAssistant.transform.position;
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.001f)
                currentAssistant.transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    public void DespawnCurrent()
    {
        if (currentAssistant == null) return;
        if (usePooling) currentAssistant.SetActive(false);
        else { Destroy(currentAssistant); currentAssistant = null; }
    }
}
