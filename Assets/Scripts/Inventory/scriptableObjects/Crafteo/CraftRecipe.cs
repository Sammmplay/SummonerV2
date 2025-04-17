using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NuevaReceta", menuName = "Inventario/Receta")]
public class CraftRecipe : ScriptableObject {
    [IDSelector("Consumibles")]
    public string resultadoID; // ID del item que se va a crear
    public int resultadoCantidad;// cantidad que se va a crear

    [System.Serializable]
    public class Ingrediente {
        [IDSelector("Consumibles")]
        public string itemID; // Id del item Requerido
        public int cantidad; // cantidad requerida
    }
    public List<Ingrediente> ingredientes = new List<Ingrediente>();
        
}
