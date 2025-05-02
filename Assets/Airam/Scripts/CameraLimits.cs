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
    private float yLimits = 25f;
    [SerializeField]
    private float zLimits;
    [SerializeField]
    private float yCenter = 12.5f;
    [SerializeField]
    private float zCenter = -17.5f;

    private void Awake()
    {
        cameraLimits = GetComponent<BoxCollider>();
        gridGenerator = FindFirstObjectByType<GridGenerator>();
    }

    /// <summary>
    /// Guarda los límites de la cámara según el tamaño del grid y un valor calculado
    /// </summary>
    private void Start()
    {
        xLimits = gridGenerator.totalColumns - 15f;
        zLimits = gridGenerator.totalRows - 12.5f;
    }

    private void Update()
    {
        CameraLimitsTweaks();
    }

    /// <summary>
    /// Función para aplicar los límites a la cámara
    /// </summary>
    public void CameraLimitsTweaks()
    {
        cameraLimits.size = new Vector3 (xLimits, yLimits, zLimits);
        cameraLimits.center = new Vector3 (0f, yCenter, zCenter);
    }
}
