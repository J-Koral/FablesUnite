using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FableCardUI : MonoBehaviour
{
    public Image artImage;       // the Fable picture
    public Image elementIcon;    // small element badge
    public Image rarityFrame;    // frame/border tinted by rarity
    public TMP_Text nameText;
    public TMP_Text levelText;
    public TMP_Text starsText;

    public OwnedFable Owned { get; private set; }

    public void Bind(OwnedFable owned, FableDefinition def, ElementIcons elementIcons)
    {
        Owned = owned;

        if (nameText  != null) nameText.text  = def.displayName;
        if (levelText != null) levelText.text = "Lv " + owned.level;
        if (starsText != null) starsText.text = new string('\u2605', Mathf.Clamp(owned.stars, 0, 6));

        if (artImage != null)
        {
            if (def.art != null) { artImage.sprite = def.art; artImage.color = Color.white; }
            else artImage.color = def.placeholderColor;   // fall back to color if no art
        }
        if (elementIcon != null && elementIcons != null) elementIcon.sprite = elementIcons.Get(def.element);
        if (rarityFrame != null) rarityFrame.color = RarityColors.Of(def.rarity);
    }
}
