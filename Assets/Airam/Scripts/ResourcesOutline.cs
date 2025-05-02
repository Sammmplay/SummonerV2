using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesOutline : MonoBehaviour
{
    [Header("Outline Settings")]
    [SerializeField]
    private Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    /// <summary>
    /// Función que controla si el objeto tiene el activo outline o no
    /// </summary>
    /// <param name="selectable"></param>
    public void IsSelectable(bool selectable)
    {
        if (outline != null)
        {
            outline.enabled = selectable;
        }
    }
}
