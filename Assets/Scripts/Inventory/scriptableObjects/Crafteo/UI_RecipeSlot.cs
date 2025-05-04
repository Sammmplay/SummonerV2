using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_RecipeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon_Result;
    public TextMeshProUGUI _nameResult;
    public Button botonSeleccionar;
    [SerializeField] CraftRecipe _receta; // receta que representa este slot se agrega mediante script
    [Header("Ingredientes")]
    public Transform ingredientesParent; // contenedor de los iconos de ingredientes
    public GameObject ingredientePrefab; // prefab para cada Ingrediente

    public void Configurar(CraftRecipe receta, itemsData itemResult) {
        _receta = receta;
        icon_Result.sprite = itemResult._sprite;
        //_nameResult.text = itemResult._id;
        //asignamos el metodo al boton
        botonSeleccionar.onClick.RemoveAllListeners();
        botonSeleccionar.onClick.AddListener(() => {
            CraftingManager.Instance.SeleccionarReceta(receta);
        });
        //limpiar items viejos si el slot se reusa
        foreach(Transform child in ingredientesParent) {
            Destroy(child.gameObject);
        }
        // crear visual para cada ingrediente
        foreach(var ingrediente in receta.ingredientes) {
            GameObject icono = Instantiate(ingredientePrefab, ingredientesParent);

            itemsData itemBase = InventoryManager.Instance._items._itemsBase.Find(x => x._id == ingrediente.itemID);
            if(itemBase != null) {
                icono.GetComponent<Image>().sprite = itemBase._sprite;
                icono.GetComponentInChildren<TextMeshProUGUI>().text = ingrediente.cantidad.ToString();
            } else {
                Debug.Log("no asignado en itemsBase de tu managerInventario");
            }
        }
    }

public void OnPointerEnter(PointerEventData eventData) {
        LeanTween.scale(gameObject, Vector3.one / 1.1f, .1f);
    
}
public void OnPointerExit(PointerEventData eventData) {
        LeanTween.scale(gameObject, Vector3.one, .1f);
    }
}
