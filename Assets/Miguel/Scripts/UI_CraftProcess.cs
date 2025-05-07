using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftProcess : MonoBehaviour , IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        FindFirstObjectByType<UI_CraftButtons>()?.AttemptToCraft();

        gameObject.SetActive(false);
    }
}
