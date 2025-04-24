using Mono.Cecil;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaleManager : MonoBehaviour
{
    public static SaleManager instance;
    [Header("Slots de ItemsEn venta")]
    public GameObject ventaSlotPrefab;
    public Transform _content;
    [Header("PanelInferior (Detalles de item)")]
    public Image iconoSeleccionado;
    public TextMeshProUGUI nombreTexto;
    public TextMeshProUGUI descripcionTexto;
    public TextMeshProUGUI precioUnitarioTexto;
    public TextMeshProUGUI precioTotalTexto;

    public TMP_InputField inputCantidad;
    [Header("Botones")]
    public Button botonSumar;
    public Button botonRestar;
    public Button botonVender;

    saveData itemSeleccionado;
    int cantidadSeleccionada = 0;
    private void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(instance);
        }
    }
    private void OnEnable() {
        CargarItemVenta();
        LimpiarPanelInferior();
        botonSumar.onClick.AddListener(SumarCantidad);
        botonRestar.onClick.AddListener(RestarCantidad);
        botonVender.onClick.AddListener(Vender);
        inputCantidad.onValueChanged.AddListener(delegate { ActualizarPrecioTotal(); });
    }
    public void CargarItemVenta() {
        foreach(Transform child in _content) {
            for (int i = child.childCount-1; i >= 0; i--) {
                Destroy(child.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < _content.childCount; i++) {
            Transform child = _content.GetChild(i);
            if (child.childCount == 0) {
                GameObject slotGO = Instantiate(ventaSlotPrefab, child);
                if(i>= InventoryManager.Instance._items._items.Count) {
                    return;
                }
            }
        }
        
    }
    public void SeleccionarItem(saveData data) {
        itemSeleccionado = data;
        cantidadSeleccionada = 1;
        iconoSeleccionado.sprite = data._sprite;
        nombreTexto.text = data._id;
        descripcionTexto.text = "(Aquí va una descripción si la tenés)";
        //precioUnitarioTexto.text = data._precioCU.ToString();
        inputCantidad.text = "1";

        ActualizarPrecioTotal();
    }
    public void ActualizarPrecioTotal() {
        if (itemSeleccionado == null) return;

        int cantidad = GetCantidad();
        int total = cantidad * itemSeleccionado._precioCU;
        precioTotalTexto.text = total.ToString();
    }
    int GetCantidad() {
        if (int.TryParse(inputCantidad.text, out int cant)) {
            return Mathf.Clamp(cant, 1, itemSeleccionado._cant);
        }
        return 1;
    }
    void SumarCantidad() {
        int actual = GetCantidad();
        if (actual >= itemSeleccionado._cant) {
            inputCantidad.text = "0"; // rotar
        } else {
            inputCantidad.text = (actual + 1).ToString();
        }
        ActualizarPrecioTotal();
    }
    void RestarCantidad() {
        int actual = GetCantidad();
        if (actual <= 1) {
            inputCantidad.text = itemSeleccionado._cant.ToString(); // rotar
        } else {
            inputCantidad.text = (actual - 1).ToString();
        }
        ActualizarPrecioTotal();
    }
    public void Vender() {
        if (itemSeleccionado == null) return;

        int cantidad = GetCantidad();
        int total = cantidad * itemSeleccionado._precioCU;

        // Agrega el oro
        GoldManager.instance.AgregarOro(total);

        // Resta del inventario
        InventoryManager.Instance.RemoveOrUpdateVisualSlot(itemSeleccionado._id, cantidad);

        // Reiniciar panel
        LimpiarPanelInferior();
        CargarItemVenta();
    }
    void LimpiarPanelInferior() {
        iconoSeleccionado.sprite = null;
        nombreTexto.text = "";
        descripcionTexto.text = "";
        //precioUnitarioTexto.text = "";
        precioTotalTexto.text = "";
        inputCantidad.text = "";
    }
}
