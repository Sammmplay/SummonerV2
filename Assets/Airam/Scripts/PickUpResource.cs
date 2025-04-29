using UnityEngine;

public class PickUpResource : MonoBehaviour, IPickUp
{
    public itemsData data;

    void IPickUp.PickUpResource(GameObject resource)
    {
        Debug.Log(gameObject + " collected");
        //InventoryManager.Instance.AddItem(data);
        Destroy(this.gameObject);
    }
}
