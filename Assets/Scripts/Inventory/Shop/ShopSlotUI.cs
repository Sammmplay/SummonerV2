using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlotUI : MonoBehaviour
{
    
    public Image icon;
    public TextMeshProUGUI nameItem;
    public TextMeshProUGUI _precioText;
    public TextMeshProUGUI _description;
    public TMP_InputField inputCantidad;

    public ShopItemData data;

    public Button botonSumar;
    public Button botonRestar;
    public TextMeshProUGUI _costoTotal;
    private void Start() {
        // Escucha cuando cambia el input manualmente
        inputCantidad.onValueChanged.AddListener(delegate { ActualizarPrecioTotal(); Debug.Log("Delegate"); });
        botonSumar.onClick.AddListener(SumarCantidad);
        botonRestar.onClick.AddListener(RestarCantidad);
    }
    public void Configurar(ShopItemData item) {
        data = item;
        nameItem.text = item.name;
        _description.text = item._description;
        icon.sprite = item._icon;
        _precioText.text = item._precioCompra.ToString();
        inputCantidad.text = "1"; //default
        ActualizarPrecioTotal();
    }
    public int GetCantidad() {// Convierte el texto del input en un número. Si falla o es menor a 1, devuelve 1 como mínimo.
        int cantidad;
        if (!int.TryParse(inputCantidad.text, out cantidad))
            cantidad = 1;
        return (Mathf.Max(1, cantidad));
    }
    public void ActualizarPrecioTotal() {
        int cantidad = GetCantidad();
        int total = cantidad * data._precioCompra;
        _costoTotal.text = total.ToString();

        // limitamos la cantidad maxima segun oro del jugador
        int oro = GoldManager.instance.obtenerOro();
        int maxCantidad = oro / data._precioCompra;

        if(cantidad > maxCantidad) {
            inputCantidad.text = maxCantidad.ToString();
            _costoTotal.text = (maxCantidad * data._precioCompra).ToString();
        }
    }
    public void SumarCantidad() {
        int actual = GetCantidad();
        int oro = GoldManager.instance.obtenerOro();
        int maxCantidad = oro / data._precioCompra;
        Debug.Log("Maxima cantida es = " + maxCantidad);
        if (actual<maxCantidad) {
            inputCantidad.text = (actual + 1).ToString();
        }
        ActualizarPrecioTotal();
        Debug.Log("Sumar cantidad");
    }
    public void RestarCantidad() {
        int actual = GetCantidad();
        if (actual > 1) {
            inputCantidad.text = (actual - 1).ToString();
        }

        ActualizarPrecioTotal();
        Debug.Log("Restar Cantidad");
    }
    public void SelectedItem() {
        
        ShopManager.Instance.SelecionarItem(this);
    }
}
