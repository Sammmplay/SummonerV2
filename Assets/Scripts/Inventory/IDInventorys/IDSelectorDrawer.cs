using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Unity.FantasyKingdom
{
    [CustomPropertyDrawer(typeof(IDSelectorAttribute))]
    public class IDSelectorDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            IDSelectorAttribute selector = (IDSelectorAttribute)attribute;

            if (IDDataBase.InstanciaGlobal == null) {
                EditorGUI.LabelField(position, label.text, "IDDatabase no asignado");
                return;
            }

            List<string> ids = IDDataBase.InstanciaGlobal.GetIdXCategoria(selector.categoria);
            if (ids == null || ids.Count == 0) {
                EditorGUI.LabelField(position, label.text, "No hay IDs disponibles");
                return;
            }

            int index = Mathf.Max(0, ids.IndexOf(property.stringValue));
            index = EditorGUI.Popup(position, label.text, index, ids.ToArray());
            property.stringValue = ids[index];
        }
    }

}
