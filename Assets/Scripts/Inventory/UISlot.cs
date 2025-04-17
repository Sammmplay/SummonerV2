using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISlot : MonoBehaviour
{
    saveData dataslot;
    public TextMeshProUGUI _text;
    public Image _sprite;
    public void Configurar(saveData dataSlot) {
        
        dataslot = dataSlot;
        _sprite.sprite = dataSlot._sprite;
        dataSlot._slotPosition = InventoryManager.Instance.GetFreeSlotPosition();
        _text.text = dataSlot._cant.ToString();
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
}
