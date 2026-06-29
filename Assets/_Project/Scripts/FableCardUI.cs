using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FableCardUI : MonoBehaviour
{
    public Image artImage;
    public TMP_Text nameText;
    public TMP_Text levelText;

    public void Bind(OwnedFable owned, FableDefinition def)
    {
        nameText.text = def.displayName;
        levelText.text = $"Lv {owned.level}  {Stars(owned.stars)}";
        if (def.art != null) { artImage.sprite = def.art; artImage.color = Color.white; }
        else artImage.color = def.placeholderColor; // greybox fallback
    }

    private string Stars(int n) => new string('★', Mathf.Clamp(n, 0, 5));
}
