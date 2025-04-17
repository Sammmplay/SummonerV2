using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlotUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI _precioText;
    ShopItemData data;

    public void Configurar(ShopItemData item) {
        data = item;

        icon.sprite = item._icon;
        _precioText.text = item._precioCompra.ToString();
    }
}
