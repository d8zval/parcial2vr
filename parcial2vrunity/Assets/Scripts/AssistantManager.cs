using UnityEngine;

public class AssistantManager : MonoBehaviour
{
    public static AssistantManager Instance { get; private set; }

    [SerializeField] private GameObject assistantPrefab;
    [SerializeField] private Transform lookAtTarget; // ARCamera
    [SerializeField] private bool facePlayer = true;

    private GameObject currentAssistant;

    private void Awake()
    {
        Instance = this;
    }

    public void SummonAtSpotTransform(Transform spot)
    {
        if (spot == null || assistantPrefab == null) return;

        if (currentAssistant != null)
            Destroy(currentAssistant);

        currentAssistant = Instantiate(assistantPrefab, spot.position, spot.rotation);

        if (facePlayer && lookAtTarget != null)
        {
            Vector3 dir = lookAtTarget.position - currentAssistant.transform.position;
            dir.y = 0;
            if (dir.sqrMagnitude > 0.01f)
                currentAssistant.transform.rotation = Quaternion.LookRotation(dir);
        }
    }
}
