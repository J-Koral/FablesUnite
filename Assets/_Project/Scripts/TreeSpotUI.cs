using UnityEngine;
using TMPro;

public class TreeSpotUI : MonoBehaviour
{
    public TMP_Text labelText;
    public TMP_Text oddsText; //

    public void Bind(TreeSpot spot, float totalWeight)
    {
        labelText.text = spot.label;
        float pct = totalWeight > 0f ? (spot.weight / totalWeight) * 100f : 0f;
        oddsText.text = pct.ToString("0.#") + "%";   // the odds helper, done for you
    }
}