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
    private float yLimits;
    [SerializeField]
    private float zLimits;
    [SerializeField]
    private float zCenter;

    private void Awake()
    {
        cameraLimits = GetComponent<BoxCollider>();
        gridGenerator = FindFirstObjectByType<GridGenerator>();
    }

    private void Start()
    {
        xLimits = gridGenerator.totalColumns;
        zLimits = gridGenerator.totalRow;
        zCenter = -zLimits / 2;
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
