using UnityEngine;

public class PanelSwitcher : MonoBehaviour
{
    public GameObject[] panels;
    public void Show(GameObject panel)
    {
        foreach (var p in panels) p.SetActive(p == panel);
    }
    public void HideAll() { foreach (var p in panels) p.SetActive(false); }
}
