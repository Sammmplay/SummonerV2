using UnityEngine;

public class CameraLimits : MonoBehaviour
{
    [Header("Camera Limits Settings")]
    [SerializeField]
    private BoxCollider cameraLimits;
    [SerializeField]
    private GridGenerator gridGenerator;
    [SerializeField]
    private float xLimits;
    [SerializeField]
    private float yLimits = 50f;
    [SerializeField]
    private float zLimits;
    [SerializeField]
    private float lateralLimitsTweaks = 15f;
    [SerializeField]
    private float frontalLimitsTweaks = 15f;
    [SerializeField]
    private float centerLimitsTweaks = 2.5f;
    [SerializeField]
    private float zCenter;

    private void Awake()
    {
        cameraLimits = GetComponent<BoxCollider>();
        gridGenerator = FindFirstObjectByType<GridGenerator>();
    }

    private void Start()
    {
        xLimits = gridGenerator.totalColumns - lateralLimitsTweaks;
        zLimits = gridGenerator.totalRow - frontalLimitsTweaks;
        zCenter = -zLimits / 2 + centerLimitsTweaks;
    }

    private void Update()
    {
        CameraLimitsTweaks();
    }

    public void CameraLimitsTweaks()
    {
        cameraLimits.size = new Vector3 (xLimits, yLimits, zLimits);
        cameraLimits.center = new Vector3 (0f, 0f, zCenter);
    }
}
