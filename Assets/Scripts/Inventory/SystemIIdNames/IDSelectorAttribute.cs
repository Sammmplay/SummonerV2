using UnityEngine;

// Paso 2: Atributo personalizado para marcar campos con categoría
public class IDSelectorAttribute : PropertyAttribute {
    public string categoria;

    public IDSelectorAttribute(string categoria) {
        this.categoria = categoria;
    }

}
