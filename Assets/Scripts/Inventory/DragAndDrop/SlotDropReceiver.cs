using UnityEngine;
using UnityEngine.EventSystems;

public class SlotDropReceiver : MonoBehaviour, IDropHandler
{
   public void OnDrop(PointerEventData eventData) {
        //Obtenemos el objeto arrastrado
        GameObject itemArrastrado = eventData.pointerDrag;
        if(itemArrastrado == null || itemArrastrado == this.gameObject) {
            return;
        }
        //intercambiar posiciones visuales
        Transform slotOrigen = itemArrastrado.transform.parent;
        Transform slotDestino = this.transform;

        itemArrastrado.transform.SetParent(slotDestino);
        itemArrastrado.transform.localPosition = Vector3.zero;

        if (slotOrigen.childCount > 0) {
            Transform otroItem = slotDestino.GetChild(0);
            otroItem.SetParent(slotOrigen);
            otroItem.localPosition = Vector3.zero;
        }

    }
}
