using System.Collections.Generic;
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

    [Header("Near Resources")]
    [SerializeField]
    private List<ResourcesOutline> selectableResources = new List<ResourcesOutline>();
    [SerializeField]
    ResourcesOutline closestResource;

    private void Awake()
    {
        inputAction = GetComponent<InteractInputAction>();
    }

    private void Update()
    {
        SelectableResource();
        PickUpResource();
    }

    /// <summary>
    /// Funci�n que detecta en un �rea los objetos que tengan la interface y selecciona al m�s cercano al jugador
    /// </summary>
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

                Debug.Log("Recolectando recurso m�s cercano:" + closestResource.name);
            }

            inputAction.playerInteract = false;
        }
    }

    /// <summary>
    /// Funci�n parecida a la anterior, que activa o desactiva el outline al estar dentro del �rea y ser el objeto m�s cercano
    /// </summary>
    private void SelectableResource()
    {
        selectableResources.Clear();

        Collider[] resourcesColliders = Physics.OverlapSphere(transform.position, interactionRadius);

        ResourcesOutline newClosestResource = null;
        float interactionArea = Mathf.Infinity;

        foreach (var collider in resourcesColliders)
        {
            if (collider.TryGetComponent<IPickUp>(out IPickUp pickUpResource) && collider.TryGetComponent<ResourcesOutline>(out ResourcesOutline selectableResource))
            {
                selectableResources.Add(selectableResource);
                float distanceToResource = Vector3.Distance(transform.position, collider.transform.position);

                if (distanceToResource < interactionArea)
                {
                    newClosestResource = selectableResource;
                    interactionArea = distanceToResource;    
                }
            }
        }
        if (newClosestResource != closestResource)
        {
            if (closestResource != null)
                closestResource.IsSelectable(false);

            if (newClosestResource != null)
                newClosestResource.IsSelectable(true);

            closestResource = newClosestResource;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
