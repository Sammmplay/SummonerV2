using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CraftRecipe;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance;
    public List<CraftRecipe> recetas;// todas las recetas que tenemos en juego
    public GameObject recetaSlotPrefab; // prefab visual de cada receta
    public Transform contenedorRecetas; // el padre (scrollViw content) donde se instancian lso slots
    [SerializeField] CraftRecipe recetaSeleccionada; // guarda la receta que el jugador selecciona

    private void Awake() {
        Instance = this;
    }
    private void Start() {
        CargarReceta();
    }
    public void CargarReceta() {
        //limpia el scroll si ya hay algo
        foreach(Transform child in contenedorRecetas) {
            Destroy(child.gameObject);
        }

        // crear un slot para cada Receta
        foreach(CraftRecipe receta in recetas) {
            GameObject slot = Instantiate(recetaSlotPrefab, contenedorRecetas);
            //busca el itemData resultado por ID (esto depende de como se guarde la base de items)
            var itemResultado = InventoryManager.Instance._itemsBase.FirstOrDefault(x => x._id == receta.resultadoID);
            // le dice al slot visual que se configure con esta receta
            slot.GetComponent<UI_RecipeSlot>().Configurar(receta, itemResultado);
        }
        
    }
    public void SeleccionarReceta(CraftRecipe receta) {
        //guarda la receta elegida
        recetaSeleccionada = receta;
        Debug.Log("Receta seleccionada:" + receta.resultadoID);
    }
    public void Craftear(CraftRecipe receta, itemsData resultadoItem, int cantidadVeces = 1) {
        // verificcar si se puede craftear
        if (!PuedeCraftear(receta, cantidadVeces)) return;


        //descuenta los ingredientes del inventario
        foreach (Ingrediente ingrediente in receta.ingredientes) {
            InventoryManager.Instance.RemoveOrUpdateVisualSlot(ingrediente.itemID,ingrediente.cantidad*cantidadVeces);
        }
        //crear un nuevo itemsData con los datos del resultado 
        itemsData nuevoItem = ScriptableObject.CreateInstance<itemsData>();
        nuevoItem._id = resultadoItem._id;
        nuevoItem._type = resultadoItem._type;
        nuevoItem._precioCU = resultadoItem._precioCU;
        nuevoItem._sprite = resultadoItem._sprite;
        nuevoItem._cantItems = receta.resultadoCantidad * cantidadVeces;
        //lo agregamos al inventario
        InventoryManager.Instance.AddItem(nuevoItem);
    }
    public bool PuedeCraftear(CraftRecipe receta, int cantVeces) {
        foreach(Ingrediente ingrediente in receta.ingredientes) {
            saveData item = InventoryManager.Instance._items._items.Find(x => x._id == ingrediente.itemID);
            if (item == null || item._cant < ingrediente.cantidad * cantVeces)
                return false;
        }return true;
    }
    public void CraftearSelecionado() {
        if (recetaSeleccionada == null) {
            Debug.Log("No hay receta Selecionada");
            return;
        }
        itemsData itemResultado = InventoryManager.Instance._itemsBase.Find(x => x._id == recetaSeleccionada.resultadoID);

        if(itemResultado == null) {
            Debug.LogError("No se encontro el item Resultado");
            return;
        }
        int cantidadVeces = CalcularCantMax(recetaSeleccionada);
        if (cantidadVeces<=0 || !PuedeCraftear(recetaSeleccionada,cantidadVeces)) {
            Debug.Log("No hay suficientes Materiales");
            return;
        }
        Craftear(recetaSeleccionada, itemResultado);
    }
    public int CalcularCantMax(CraftRecipe receta) {
        int max = int.MaxValue;
        foreach(Ingrediente ingrediente in receta.ingredientes) {
            saveData item = InventoryManager.Instance._items._items.Find(x => x._id == ingrediente.itemID);
            if (item == null || item._cant < ingrediente.cantidad) return 0;

            int posibles = item._cant / ingrediente.cantidad;
            if (posibles < max) {
                max = posibles;
            }
        }
        return max;
    }
    /*
    public bool PuedeCraftear(CraftRecipe receta) {
        foreach (var ingrediente in receta.ingredientes) {
            var item = InventoryManager.Instance._items._items.FirstOrDefault(x => x._id == ingrediente.itemID);
            if(item == null || item._cant<ingrediente.cantidad) {
                return false;
            }
        }
        return true;
    }
    public int CalcularCantidadCrafteable(CraftRecipe receta) {
        int veces = int.MaxValue;
        foreach (var ingrediente in receta.ingredientes) {
            var item = InventoryManager.Instance._items._items.FirstOrDefault(X => X._id == ingrediente.itemID);
            if (item == null || item._cant < ingrediente.cantidad) 
                return 0;
            int posibles = item._cant / ingrediente.cantidad;
            veces = Mathf.Min(veces, posibles);
        }
        return veces;
    }
    public void Craftear(CraftRecipe receta, itemsData resultadoItemData, int cantidadVeces = 1) {
        if (CalcularCantidadCrafteable(receta) < cantidadVeces) {
            Debug.Log("No tienes suficientes materiales para craftear" + cantidadVeces + "Veces");
            return;
        }
        foreach (var ingrediente in receta.ingredientes) {
            var item = InventoryManager.Instance._items._items.FirstOrDefault(x => x._id == ingrediente.itemID);
            if (item != null) {
                item._cant -= ingrediente.cantidad * cantidadVeces;
            }
        }
        itemsData nuevo = ScriptableObject.CreateInstance<itemsData>();
        nuevo._id = resultadoItemData._id;
        nuevo._type = resultadoItemData._type;
        nuevo._cant = receta.resultadoCantidad * cantidadVeces;
        nuevo._sprite = resultadoItemData._sprite;
        nuevo._precioCU = resultadoItemData._precioCU;
        InventoryManager.Instance.AddItem(nuevo);
        Debug.Log("Crafteado con exito:" + cantidadVeces + "x " + nuevo._id);
    }*/
}
