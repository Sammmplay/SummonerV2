using UnityEngine;

public class PickUpResource : MonoBehaviour, IPickUp
{
    public itemsData data;

    void IPickUp.PickUpResource(GameObject resource)
    {
        InventoryManager.Instance.AddItem(data);
        Destroy(this.gameObject);
    }
}
