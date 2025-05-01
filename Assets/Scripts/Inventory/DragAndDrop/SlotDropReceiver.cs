using UnityEngine;
using UnityEngine.EventSystems;

public class SlotDropReceiver : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData) {
        GameObject objetoSoltado = eventData.pointerDrag;
        if (objetoSoltado == null) return;

        Transform objetoArrastrado = objetoSoltado.transform;
        Transform slotOrigen = objetoArrastrado.GetComponent<DragItem>().originalParent;
        Transform slotDestino = transform;
        if (!objetoSoltado.TryGetComponent<UISlot>(out var uiSlot)) return;

        Transform targetSlot = transform;
        Transform originSlot = uiSlot.transform.parent;

        // Si el slot destino ya tiene un hijo, intercambiamos
        if (targetSlot.childCount > 0)
        {
            Transform otroItem = targetSlot.GetChild(0);
            // intercambiar: el que esta en destino va al origen
            otroItem.SetParent(originSlot);
            otroItem.localPosition = Vector3.zero;

            // Actualizamos posición lógica
            if (otroItem.TryGetComponent<UISlot>(out var uiSlotOtro))
            {
                uiSlotOtro.SaveData()._slotPosition = originSlot.GetSiblingIndex();
            }
        }

        // Mover el item arrastrado al nuevo slot
        objetoArrastrado.SetParent(targetSlot);
        objetoArrastrado.localPosition = Vector3.zero;

        // Actualizamos también el dato del inventario
        uiSlot.SaveData()._slotPosition = targetSlot.GetSiblingIndex();
        Debug.Log("Dato de inventario Actualizado");
    }

}
