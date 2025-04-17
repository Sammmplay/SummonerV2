using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public itemsData data;
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            InventoryManager.Instance.AddItem(data);
            Destroy(gameObject);
        }
    }
}
