using UnityEngine;
using UnityEngine.InputSystem;

public class Manager_canvas_Inventario : MonoBehaviour
{
    [SerializeField] ActionUI UI_Input;

    [SerializeField] GameObject _panelInventory;
    [SerializeField] GameObject _panelCrafting;
    [SerializeField] GameObject _panelShop;
    [SerializeField] bool _inventory;
    [SerializeField] bool _crafting;

    //content del shoping es de 240* Nro de items
    private void OnEnable() {
        UI_Input = new ActionUI();
        UI_Input.UI.Enable();
        

        UI_Input.UI.Inventory.performed += ctx => Inventory();
        UI_Input.UI.Crafting.performed += ctx => Crafting();
    }
    private void OnDisable() {
        UI_Input.UI.Disable();
    }
    public void Inventory() {
        _inventory = !_inventory;

        _panelInventory.SetActive(_inventory);
    }
    public void Crafting() {
        _crafting = !_crafting;

        _panelCrafting.SetActive(_crafting);
    }
    public bool tiendaActiva() {
        return _panelShop.activeSelf;
    }
}
