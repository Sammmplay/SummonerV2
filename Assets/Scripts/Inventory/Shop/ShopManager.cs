using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;
    public List<ShopItemData> _itemsEnVenta;
    public GameObject _slptPrefab;
    public Transform _contentScroll;
    public float sizeContent = 240.0f;
    [SerializeField] private Button _botonComprar;
    [Header("PowerUpManager")]
    [SerializeField] private PowerUpManager powerUpManager;
    [Header("Seleccion de Item")]
    [SerializeField] private ShopSlotUI _itemSeleccionado;
    //[SerializeField] private TextMeshProUGUI _precioSeleccionado;
    
    //[SerializeField] private TMP_InputField _inputCantidad; // o botones + / -
    private void Awake() {
        Instance = this;
    }
    private void Start() {
        MostrarItems();
    }
    public void MostrarItems() {
        foreach(Transform child in _contentScroll) {
            Destroy(child); // destruimos los items que puedan haber en un principio en nuestro contend para mantener un entorno limpio
            Debug.Log("limpieza de items");
        }
        foreach (ShopItemData item in _itemsEnVenta) {
            GameObject slot = Instantiate(_slptPrefab, _contentScroll);
            RectTransform heigh = _contentScroll.GetComponent<RectTransform>();
            Vector2 size = heigh.sizeDelta; // obtener el tamaño actual
            size.y += sizeContent; // modificar la altura
            heigh.sizeDelta = size;

            slot.GetComponent<ShopSlotUI>().Configurar(item);
        }
        //ActualizarTextGold();
    }
    void Comprar(ShopSlotUI slot) {
        int cantidad = slot.GetCantidad(); // obtiene el inputfield
        int precioTotal = slot.data._precioCompra * cantidad;

        if (GoldManager.instance.obtenerOro() < precioTotal)
            return;
        GoldManager.instance.QuitarOro(precioTotal);

        //itemsData nuevo = ScriptableObject.CreateInstance<itemsData>();
        itemsData nuevo = InventoryManager.Instance._items._itemsBase.FirstOrDefault(
            x=> x._id == slot.data._id);
        if(nuevo == null) {
            Debug.LogWarning("Item no encontrado en:" + InventoryManager.Instance._items._itemsBase);
            return;
        }
        if(nuevo._type== TypeItem.PowerUp) {
            ActivarPowerUp(nuevo._id);
            return;
        }
        if(nuevo._type == TypeItem.Pet) {
            BuyPet(nuevo._id); return;  
        }
        nuevo._cantItems = cantidad;
        // agregamos a nuestro inventario
        InventoryManager.Instance.AddItem(nuevo);
        SaleManager.instance.CargarItemVenta();
        Debug.Log("ItemAgregado");
        nuevo._cantItems = 1;
    }
    public void ComprarSlotSeleccionado() {
        if(_itemSeleccionado != null) {
            Comprar(_itemSeleccionado);
            Debug.Log("Item Comprado");
        } else {
            _botonComprar.interactable = false;
        }
    }
    public void SelecionarItem(ShopSlotUI slot) {
        _itemSeleccionado = slot;

        _botonComprar.interactable = true;
    }
    void ActivarPowerUp(string id) {
        if(powerUpManager == null) {
            powerUpManager = FindFirstObjectByType<PowerUpManager>();
        }
        
        switch (id) {
            case "SpeedBoost":
                powerUpManager.ImproveVelocity();
                break;
            case "HealthUp":
                powerUpManager.ImproveHealth();
                break;
            case "Heal":
                powerUpManager.Healing();
                break;
            case "AreaUp":
                powerUpManager.ImproveArea();
                break;
            case "SummonAssassin":
                powerUpManager.SummonAssassin();
                break;
            case "SummonAttacker":
                powerUpManager.SummonAttacker();
                break;
            case "SummonDefender":
                powerUpManager.SummonDefender();
                break;
            default:
                Debug.LogWarning("PowerUp no reconocido: " + id);
                break;
        }
    }
    void BuyPet(string id) {
        if (powerUpManager == null) {
            powerUpManager = FindFirstObjectByType<PowerUpManager>();
        }
        switch (id) {
            case "SummonAssassin":
                powerUpManager.SummonAssassin();
                break;
            case "SummonAttacker":
                powerUpManager.SummonAttacker();
                break;
            case "SummonDefender":
                powerUpManager.SummonDefender();
                break;
            default:
                Debug.LogWarning("PowerUp no reconocido: " + id);
                break;
        }
    }
}
