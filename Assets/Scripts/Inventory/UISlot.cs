using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UISlot : MonoBehaviour
{
    [SerializeField]saveData dataslot;
    public TextMeshProUGUI _text;
    public Image _sprite;
    private void Start() {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(SelectedItemventa);
    }
    void SelectedItemventa() {
        if (FindFirstObjectByType<Manager_canvas_Inventario>().tiendaActiva()) {
            SaleManager.instance.SeleccionarItem(dataslot);
        }
    }
    public void Configurar(saveData dataSlot) {
        
        dataslot = dataSlot;
        _sprite.sprite = dataSlot._sprite;
        dataslot._nameItem = dataSlot._nameItem;
        //dataslot._slotPosition = InventoryManager.Instance.GetFreeSlotPosition()-1;
        _text.text = dataSlot._cant.ToString();
    }
    public void ConfigurarPosicion(int pos) {
        dataslot._slotPosition=pos;
    }
    public void HideRefererence() {
        ReferenceItem _ref = FindFirstObjectByType<ReferenceItem>();
        _ref.transform.GetChild(0).gameObject.SetActive(true);
        _ref._name.text = dataslot._id;
        _ref._icon.sprite = dataslot._sprite;
        _ref._valor.text = dataslot._precioCU.ToString();
    }
    public void HideTooltip() {
        ReferenceItem _ref = FindFirstObjectByType<ReferenceItem>();
        _ref.transform.GetChild(0).gameObject.SetActive(false);
    }
    public saveData SaveData( ) {
        return dataslot;
    }
}
