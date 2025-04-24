using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField]
    private InteractInputAction inputAction;
    [SerializeField]
    private float interactionRadius;

    private void Awake()
    {
        inputAction = GetComponent<InteractInputAction>();
    }

    private void Update()
    {
        PickUpResource();
    }

    private void PickUpResource()
    {
       if (inputAction.playerInteract)
        {

            Collider[] resourcesColliders = Physics.OverlapSphere(transform.position, interactionRadius);

            IPickUp closestPickUp = null;
            Transform closestResource = null;
            float interactionArea = Mathf.Infinity;

            foreach (var collider in resourcesColliders)
            {
                if (collider.TryGetComponent<IPickUp>(out IPickUp pickUpResource))
                {
                    float distanceToResource = Vector3.Distance(transform.position, collider.transform.position);

                    if (distanceToResource < interactionArea)
                    {
                        interactionArea = distanceToResource;
                        closestResource = collider.transform; 
                        closestPickUp = pickUpResource;
                    }
                }
            }
            if (closestResource != null)
            {
                closestPickUp.PickUpResource(this.gameObject);

                Debug.Log("Recolectando recurso más cercano:" + closestResource.name);
            }

            inputAction.playerInteract = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }


}
