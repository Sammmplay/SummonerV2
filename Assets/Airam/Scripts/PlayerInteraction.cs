using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField]
    private InteractInputAction inputAction;
    [SerializeField]
    private Transform currentResource; 
    [SerializeField]
    private float interactionRadius;

    private void Awake()
    {
        inputAction = GetComponent<InteractInputAction>();
    }

    private void Start()
    {
        
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

            foreach (var collider in resourcesColliders)
            {
                if (collider.TryGetComponent<IPickUp>(out IPickUp pickUpResource))
                {
                    currentResource = collider.transform;
                    pickUpResource.PickUpResource(this.gameObject);
                }
            }

            Debug.Log("E key pressed");
            inputAction.playerInteract = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }


}
