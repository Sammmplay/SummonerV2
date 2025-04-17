#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CustomPropertyDrawer(typeof(IDSelectorAttribute))]
public class IDSelectorDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        IDSelectorAttribute idAttr = (IDSelectorAttribute)attribute;

        // Usar la instancia global en lugar de Resources.Load
        IDDataBase db = AssetDatabase.LoadAssetAtPath<IDDataBase>("Assets/Resources/IDDataBase.asset");
        if (db == null) {
            Debug.LogWarning("IDDataBase.InstanciaGlobal es NULL");
            EditorGUI.HelpBox(position, "Asigna IDDataBase.InstanciaGlobal desde el editor", MessageType.Warning);
            return;
        }

        List<string> ids = db.GetIdXCategoria(idAttr.categoria);
        if (ids.Count == 0) {
            EditorGUI.HelpBox(position, "No hay IDs definidos para esta categoría", MessageType.Info);
            return;
        }

        int index = Mathf.Max(0, ids.IndexOf(property.stringValue));
        string[] opciones = ids.ToArray();
        int newIndex = EditorGUI.Popup(position, label.text, index, opciones);
        property.stringValue = opciones[newIndex];
    }
}
#endif
