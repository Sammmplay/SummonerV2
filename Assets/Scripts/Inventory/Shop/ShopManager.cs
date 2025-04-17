using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;
    public List<ShopItemData> _itemsEnVenta;
    public GameObject _slptPrefab;
    public Transform _contentScroll;
    public int _goldPlayer = 20;

    [SerializeField] private Button _botonComprar;

    public TextMeshProUGUI _textGold;
    [Header("Seleccion de Item")]
    [SerializeField] private ShopItemData _itemSeleccionado;
    [SerializeField] private TextMeshProUGUI _precioSeleccionado;
    
    [SerializeField] private TMP_InputField _inputCantidad; // o botones + / -
    private void Awake() {
        Instance = this;
    }
    private void Start() {
        MostrarItems();
    }
    public void MostrarItems() {
        foreach(Transform child in _contentScroll) {
            Destroy(child); // destruimos los items q   ue puedan haber en un principio en nuestro contend para mantener un entorno limpio
        }
        foreach (ShopItemData item in _itemsEnVenta) {
            GameObject slot = Instantiate(_slptPrefab, _contentScroll);
            slot.GetComponent<ShopSlotUI>().Configurar(item);
        }
        //ActualizarTextGold();
    }
    public void Comprar(ShopItemData item) {
        if (_goldPlayer < item._precioCompra) {
            return;
        }
        _goldPlayer -= item._precioCompra;

        //creamos itemData para nuestro inventario

        itemsData nuevo = ScriptableObject.CreateInstance<itemsData>();

        nuevo._id = item._id;
        nuevo._type = item.type;
        nuevo._sprite = item._icon;
        nuevo._precioCU = item._precioCompra;
        nuevo._cantItems = item._cantidadXVenta;

        // agregamos a nuestro inventario
        InventoryManager.Instance.AddItem(nuevo);
    }
    public void ActualizarTextGold() {
        _textGold.text = _goldPlayer.ToString();
    }
    public void SelecionarItem(ShopItemData item) {
        _itemSeleccionado = item;

        _precioSeleccionado.text = item._precioCompra.ToString();
        _inputCantidad.text = "1"; // por defecto
        _botonComprar.interactable = true;
    }
}
