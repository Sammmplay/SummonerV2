using System;
using UnityEngine;
public enum TypeItem {
    None,
    Hierbas,
    posion
}

[Serializable]
public class saveData { // esta clase se usa para serializar si lo queremos hacer a futuro
    [IDSelector("Inventario")] public string _id;// categoria definida en IDDataBase
    public String _description;
    public string _nameItem;
    public TypeItem _type;
    public int _cant;
    public int _precioCU;
    public int _slotPosition;
    public Sprite _sprite;
}
[CreateAssetMenu(fileName = "Inventory",  menuName = "Inventory/item")]
public class itemsData : ScriptableObject {
    [IDSelector("Inventario")]
    public string _id;
    [IDSelector("Names")]
    public string _nameItem;
    public String _description;
    public TypeItem _type;
    public int _cantItems;
    public int _precioCU;
    public int _slotPosition;
    public Sprite _sprite;
}
