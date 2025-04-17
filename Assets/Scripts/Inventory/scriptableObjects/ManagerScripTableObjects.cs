using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Manager", menuName = "ScriptableObjectManager")]
public class ManagerScripTableObjects : ScriptableObject {

    public List<saveData> _items;
    public GameObject _slotPrefab;

    
}
