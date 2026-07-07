using UnityEngine;
using TMPro;

public class OddsPopup : MonoBehaviour
{
    public static OddsPopup I { get; private set; }

    public GameObject root;   // the popup panel (starts inactive)
    public TMP_Text text;

    private void Awake() => I = this;
    private void OnDestroy() { if (I == this) I = null; }

    public void Show(Vector3 worldPos, string label, float pct, Color color)
    {
        if (root == null) return;
        root.SetActive(true);
        root.transform.position = worldPos + new Vector3(0f, 80f, 0f); // float above the item
        if (text != null)
        {
            text.text = label + "\n" + pct.ToString("0.#") + "%";
            text.color = color;
        }
    }

    public void Hide() { if (root != null) root.SetActive(false); }
}
