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

    //metodo para obtener los Ids segun la categoria
    public static IDDataBase InstanciaGlobal; // Referencia global manual
    public List<string> GetIdXCategoria(string categoria) {
        switch (categoria) {
            case "Inventario": return idInventory;
            case "Shop": return idShop;
            case "Craft": return IdsCraft;
            default: return new List<string>();
        }
    }
}

