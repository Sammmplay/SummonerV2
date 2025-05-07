using UnityEngine;

public class QTE_Slider : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public RectTransform winZone;

    public float moveSpeed = 100f;

    private float direction = 1f; // 1 for moving towards B, -1 for moving towards A

    private RectTransform pointerTransform;

    private Vector3 targetPosition;

    [SerializeField] private CraftingManager craftController;


    void Start()
    {
        pointerTransform = GetComponent<RectTransform>();

        targetPosition = pointB.position;
    }

    private void Awake()
    {
        craftController = FindFirstObjectByType<CraftingManager>();
    }

    // Update is called once per frame
    void Update()
    {
        pointerTransform.position = Vector3.MoveTowards(pointerTransform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(pointerTransform.position, pointA.position) < 0.1f)
        {
            targetPosition = pointB.position;
            direction = 1f;
        }
        else if (Vector3.Distance(pointerTransform.position, pointB.position) < 0.1F)
        {
            targetPosition = pointA.position;
            direction = -1f;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckSuccess();
        }
    }

    private void CheckSuccess()
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(winZone, pointerTransform.position , null))
        {
            craftController.CraftearSelecionado();
        }
    }
}
