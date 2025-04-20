using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="IDDataBase",menuName ="Inventario/ID DataBase")]
public class IDDataBase : ScriptableObject
{
    [Header("IDs de Consumibles")]
    public List<string> idInventory;
    [Header("IDs de la Tienda")]
    public List<string> idShop;
    [Header("Ids de Craft")]
    public List<string> IdsCraft;
    [Header("NameItems")]
    public List<string> names;
    //metodo para obtener los Ids segun la categoria
    public static IDDataBase InstanciaGlobal; // Referencia global manual
    public List<string> GetIdXCategoria(string categoria) {
        switch (categoria) {
            case "Inventario": return idInventory;
            case "Shop": return idShop;
            case "Craft": return IdsCraft;
            case "Names": return names;
            default: return new List<string>();
        }
    }
}

