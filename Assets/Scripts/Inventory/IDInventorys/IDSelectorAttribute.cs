using UnityEngine;

namespace Unity.FantasyKingdom
{
    public class IDSelectorAttribute : PropertyAttribute {
        public string categoria;

        public IDSelectorAttribute(string categoria) {
            this.categoria = categoria;
        }
    }
}
