using UnityEngine;

public class PickUpResource : MonoBehaviour, IPickUp
{
    [Header("Effects Settings")]
    [SerializeField]
    private GameObject pickUpEffect;
    [SerializeField]
    private AudioClip[] pickUpSounds;
    public itemsData data;

    void IPickUp.PickUpResource(GameObject resource)
    {
        InventoryManager.Instance.AddItem(data);

        int soundsIndex = Random.Range(0, pickUpSounds.Length);
        AudioClip soundEffect = pickUpSounds[soundsIndex];
        AudioSource.PlayClipAtPoint(soundEffect, transform.position);
        Instantiate(pickUpEffect, transform.position, pickUpEffect.transform.rotation);

        Destroy(this.gameObject);
    }
}
