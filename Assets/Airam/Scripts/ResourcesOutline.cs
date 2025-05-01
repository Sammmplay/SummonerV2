using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesOutline : MonoBehaviour
{
    [Header("Outline Settings")]
    [SerializeField]
    private Outline outline;

    private void Start()
    {
        outline = GetComponent<Outline>();
    }

    public void IsSelectable(bool selectable)
    {
        if (outline != null)
        {
            outline.enabled = selectable;
        }
    }
}
