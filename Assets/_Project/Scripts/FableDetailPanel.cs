using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FableDetailPanel : MonoBehaviour
{
    [Header("Root")]
    public GameObject panelRoot;          // the overlay; starts inactive

    [Header("Header")]
    public Image artImage;
    public Image elementIcon;
    public Image rarityFrame;
    public TMP_Text nameText;
    public TMP_Text statsText;
    public ElementIcons elementIcons;

    [Header("Axis 1 - Level")]
    public Button feedButton;
    public TMP_Text levelText;
    public TMP_Text xpText;

    [Header("Axis 2 - Stars")]
    public Button starButton;
    public TMP_Text starsText;
    public TMP_Text shardsText;

    [Header("Axis 3 - Evolution")]
    public Button evolveButton;
    public TMP_Text evolveText;

    // Axis 4 - Skills: reserved 'coming soon' section, no logic yet.

    private OwnedFable current;

    public void Open(OwnedFable owned)
    {
        current = owned;
        if (panelRoot != null) panelRoot.SetActive(true);
        Refresh();
    }

    public void Close() { if (panelRoot != null) panelRoot.SetActive(false); }

    private void Refresh()
    {
        var def = GameData.I.database.Get(current.definitionId);
        if (def == null) return;

        // header
        if (nameText != null) nameText.text = def.displayName;
        if (artImage != null)
        {
            if (def.art != null) { artImage.sprite = def.art; artImage.color = Color.white; }
            else artImage.color = def.placeholderColor;
        }
        if (elementIcon != null && elementIcons != null) elementIcon.sprite = elementIcons.Get(def.element);
        if (rarityFrame != null) rarityFrame.color = RarityColors.Of(def.rarity);
        if (statsText != null)
            statsText.text = $"HP {FableStats.Health(def, current.level, current.stars):0}    " +
                             $"DMG {FableStats.Damage(def, current.level, current.stars):0}";

        // axis 1 - level
        if (levelText != null) levelText.text = "Lv " + current.level;
        if (xpText != null)    xpText.text    = current.xp + " / " + FableUpgrade.XpPerLevel + " XP";
        if (feedButton != null) feedButton.interactable = current.xp >= FableUpgrade.XpPerLevel;

        // axis 2 - stars
        if (starsText != null)  starsText.text  = current.stars + " / " + def.maxStars + " \u2605";
        if (shardsText != null) shardsText.text = current.shards + " / " + FableUpgrade.ShardsPerStar + " shards";
        if (starButton != null) starButton.interactable =
            current.stars < def.maxStars && current.shards >= FableUpgrade.ShardsPerStar;

        // axis 3 - evolution
        bool canEvolve = def.evolvesInto != null && current.stars >= def.maxStars;
        if (evolveButton != null) evolveButton.interactable = canEvolve;
        if (evolveText != null)
            evolveText.text = def.evolvesInto == null ? "No evolution"
                            : (canEvolve ? "Ready to evolve!" : "Reach max stars");

        if (GameData.I != null) GameData.I.Save();
    }

    public void OnFeed()   { if (FableUpgrade.TryLevelUp(current)) Refresh(); }
    public void OnStarUp() { if (FableUpgrade.TryStarUp(current, GameData.I.database.Get(current.definitionId))) Refresh(); }
    public void OnEvolve() { if (FableUpgrade.TryEvolve(current, GameData.I.database)) Refresh(); }
}
