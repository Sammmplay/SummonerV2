using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public ManagerScripTableObjects _items;

    public List<itemsData> _itemsBase; // lista de todos los scriptableObjects base
    Dictionary<string,UISlot> _slotVisuales = new Dictionary<string,UISlot>(); // guardar en un diccionario para sumar directamente la cantidad de items que hay 
    public IDDataBase idDatabase; // ← arrastrás esto desde el Inspector
    public Transform _slotPather;
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
        _items._items.Clear();
        IDDataBase.InstanciaGlobal = idDatabase;
    }

    public void AddItem(itemsData data) {
        saveData existente = _items._items.Find(x => x._id == data._id);

        if (existente != null) {
            existente._cant += data._cantItems;
        } else {
            saveData nuevo = ConvertirASaveData(data);
            _items._items.Add(nuevo);
        }

        ActualizarVisualSlot(data._id);
    }
    public void ActualizarVisualSlot(string id) {
        var item = _items._items.Find(x => x._id == id);
        if (item == null) return;

        if (_slotVisuales.TryGetValue(id, out UISlot slot)) {
            slot._text.text = item._cant.ToString();
        } else {
            Transform pos = _slotPather.GetChild(GetFreeSlotPosition());
            GameObject instanceItemSlotUI = Instantiate(_items._slotPrefab, pos);
            UISlot nuevoSlot = instanceItemSlotUI.GetComponent<UISlot>();
            nuevoSlot.Configurar(item);
            _slotVisuales[item._id] = nuevoSlot;
        }
    }
    public void RemoveOrUpdateVisualSlot(string id, int cantidadARestar) {
        var item = _items._items.Find(x => x._id == id);
        if (item == null) return;

        item._cant -= cantidadARestar;

        if (item._cant <= 0) {
            _items._items.RemoveAll(x => x._id == id);

            if (_slotVisuales.TryGetValue(id, out UISlot slot)) {
                Destroy(slot.gameObject);
                _slotVisuales.Remove(id);
            }
        } else {
            if (_slotVisuales.TryGetValue(id, out UISlot slot)) {
                slot._text.text = item._cant.ToString();
            }
        }
    }

    public saveData ConvertirASaveData(itemsData data) {
        return new saveData {
            _id = data._id,
            _type = data._type,
            _cant = data._cantItems,
            _precioCU = data._precioCU,
            _description = data._description,
            _slotPosition = data._slotPosition,
            _sprite=data._sprite,
};
    }
   
    
    public int GetFreeSlotPosition() {
        for (int i = 0; i < _slotPather.childCount; i++) {
            Transform slotChild = _slotPather.GetChild(i);
            Debug.Log("Buscando slot vacio");
            if (slotChild.childCount == 0) {
                return i;
            }
        }
        return -1;
    }
}
