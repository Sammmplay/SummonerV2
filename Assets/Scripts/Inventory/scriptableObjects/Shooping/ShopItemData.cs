using Unity.FantasyKingdom;
using UnityEngine;
[CreateAssetMenu(menuName = "Tienda/Item de tienda")]
public class ShopItemData : ScriptableObject
{
    [IDSelector("Shop")]
    public string _id;
    [IDSelector("Names")]
    public string _nameItem;
    public string _description;
    public TypeItem type;
    public Sprite _icon;
    public int _precioCompra;
    public int _precioVenta;
    public int _cantidadXVenta;
}
