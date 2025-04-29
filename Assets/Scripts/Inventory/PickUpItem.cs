using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public itemsData data;
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            InventoryManager.Instance.AddItem(data);
            SaleManager.instance.CargarItemVenta();
            Destroy(gameObject);
        }
    }
}
