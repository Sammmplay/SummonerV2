using UnityEngine;
using UnityEngine.EventSystems;

public class DragItem : MonoBehaviour ,IBeginDragHandler,IDragHandler, IEndDragHandler{
    // Ira en cada UISlot este script permite arrastrar un item por el inventario

    public Canvas canvas;
    CanvasGroup canvasGroup;
    RectTransform rectTransform;

    [HideInInspector]
    public Transform originalParent;
    Vector2 originalPos;

    private void Awake() {
        if (canvas == null) {
            canvas = GetComponentInParent<Canvas>();
        }
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnBeginDrag(PointerEventData eventData) {
        originalParent = transform.parent;
        transform.SetParent(canvas.transform); // mover al canvas root
        canvasGroup.blocksRaycasts = false;
    }
    public void OnDrag(PointerEventData eventdata) {
        // Mueve el ítem con el mouse, ajustando por el factor de escala del Canvas
        rectTransform.anchoredPosition += eventdata.delta / canvas.scaleFactor;
    }
    public void OnEndDrag(PointerEventData eventData) {
        canvasGroup.blocksRaycasts = true;

        // Si no cambió de padre (no fue soltado en un slot válido), vuelve al original
        if (transform.parent == canvas.transform) {
            transform.SetParent(originalParent);
            rectTransform.anchoredPosition = Vector2.zero;
        }
    }
}
