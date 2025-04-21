using UnityEngine;

public class PickUpResource : MonoBehaviour, IPickUp
{
    void IPickUp.PickUpResource(GameObject resource)
    {
        Debug.Log(gameObject + " collected");
        Destroy(this.gameObject);
    }
}
