#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class IDDatabaseAutoLoader {
    static IDDatabaseAutoLoader() {
        string path = "Assets/Resources/IDDataBase.asset"; // asegurate de que el archivo esté en esa ruta
        IDDataBase db = AssetDatabase.LoadAssetAtPath<IDDataBase>(path);
        if (db != null) {
            IDDataBase.InstanciaGlobal = db;
        }
        Debug.Log("IDDatabaseAutoLoader ejecutado");
    }
}
#endif
