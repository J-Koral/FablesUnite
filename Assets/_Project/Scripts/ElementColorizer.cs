using UnityEngine;

[RequireComponent(typeof(Unit))]
public class ElementColorizer : MonoBehaviour
{
    private void Start()
    {
        Unit unit = GetComponent<Unit>();
        Renderer r = GetComponentInChildren<Renderer>();
        if (r != null) r.material.color = ElementColors.Get(unit.element);
    }
}
