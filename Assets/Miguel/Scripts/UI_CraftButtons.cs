using Unity.VisualScripting;
using UnityEngine;

public class UI_CraftButtons : MonoBehaviour
{
    [Header("Buttons details")]
    [SerializeField] private BoxCollider2D craftWindow;
    [SerializeField] private CraftingManager craftController;
    [SerializeField] private int craftSteps;
    [SerializeField] private UI_CraftProcess[] craftButtons;


    private void Start()
    {
        craftButtons = GetComponentsInChildren<UI_CraftProcess>(true);
    }

    public void OpenUICraftButtons ()
    {
        foreach (UI_CraftProcess button in craftButtons)
        {
            button.gameObject.SetActive(true);

            float randomX = Random.Range(craftWindow.bounds.min.x, craftWindow.bounds.max.x);
            float randomY = Random.Range(craftWindow.bounds.min.y, craftWindow.bounds.max.y);

            button.transform.position = new Vector3(randomX, randomY, 1);
        }

        craftSteps = craftButtons.Length;
    }

    public void AttemptToCraft ()
    {
        if (craftSteps > 0)
            craftSteps--;
        if (craftSteps <= 0)
        {
            craftController.CraftearSelecionado();
        }   
    }
}
