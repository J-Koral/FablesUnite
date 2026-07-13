using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FableCardUI : MonoBehaviour
{
    public Image background;      // solid rarity color, fills the square
    public Image artImage;        // the Fable picture (fills the square)
    public Image elementIcon;     // small, top-right
    public TMP_Text rarityLetter; // top-left (C/R/E/L)
    public TMP_Text starsText;    // bottom-center
    public Image shardBar;        // optional thin bar: fill = shards / ShardsPerStar

    public OwnedFable Owned { get; private set; }

    public void Bind(OwnedFable owned, FableDefinition def, ElementIcons elementIcons)
    {
        Owned = owned;

        if (background != null) background.color = RarityColors.Of(def.rarity);

        if (artImage != null)
        {
            if (def.art != null) { artImage.sprite = def.art; artImage.color = Color.white; }
            else artImage.color = def.placeholderColor;
        }

        if (elementIcon != null && elementIcons != null) elementIcon.sprite = elementIcons.Get(def.element);
        if (rarityLetter != null) rarityLetter.text = RarityLetter(def.rarity);
        if (starsText != null) starsText.text = new string('*', Mathf.Clamp(owned.stars, 0, 6));
        if (shardBar != null)     shardBar.fillAmount = Mathf.Clamp01(owned.shards / (float)FableUpgrade.ShardsPerStar);
    }
    
    private string RarityLetter(Rarity r)
    {
    switch (r)
        {
            case Rarity.Common:    return "C";
            case Rarity.Rare:      return "R";
            case Rarity.Epic:      return "E";
            case Rarity.Legendary: return "L";
        }
        return "?";
    }
}
