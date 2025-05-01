using UnityEngine;
using UnityEngine.EventSystems;

public class DragItem : MonoBehaviour ,IBeginDragHandler,IDragHandler, IEndDragHandler{
    // Ira en cada UISlot este script permite arrastrar un item por el inventario

    public Canvas canvas;
    CanvasGroup canvasGroup;
    RectTransform rectTransform;

<<<<<<< Updated upstream
<<<<<<< Updated upstream
    Transform originalParent;
=======
   [HideInInspector]
public Transform originalParent;
>>>>>>> Stashed changes
=======
   [HideInInspector]
public Transform originalParent;
>>>>>>> Stashed changes
    Vector2 originalPos;

    private void Awake() {
        if (canvas == null) {
            canvas = GetComponentInParent<Canvas>();
        }
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnBeginDrag(PointerEventData eventData) {
        originalParent = transform.parent;              // Guarda el padre original del objeto
        originalPos = rectTransform.anchoredPosition;   // Guarda la posición inicial

        canvasGroup.blocksRaycasts = false;             // Desactiva raycasts para que no bloquee eventos de otros objetos
        transform.SetParent(canvas.transform);          //se  mueve por encima de todo
    }
    public void OnDrag(PointerEventData eventdata) {
        // Mueve el ítem con el mouse, ajustando por el factor de escala del Canvas
        rectTransform.anchoredPosition += eventdata.delta / canvas.scaleFactor;
    }
    public void OnEndDrag(PointerEventData eventData) {
        transform.SetParent(originalParent);             // Vuelve a poner el objeto en su padre original
        rectTransform.anchoredPosition = originalPos;   // Vuelve a la posición original (por ahora)
        canvasGroup .blocksRaycasts = true;              // Reactiva los raycasts
    }
}
