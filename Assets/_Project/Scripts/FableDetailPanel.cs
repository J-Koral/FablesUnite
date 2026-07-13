using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FableDetailPanel : MonoBehaviour
{
    [Header("Root")]
    public GameObject panelRoot;      // the DetailPanel overlay
    public RosterView rosterView;     // to refresh the grid on close (e.g. after evolve)

    [Header("Header")]
    public Image bgTint;              // DetailPanel's own Image; tinted by rarity
    public Image artImage;
    public Image elementIcon;
    public TMP_Text nameText;
    public TMP_Text starsText;        // the star row under the name
    public TMP_Text ratingText;       // rarity letter
    public ElementIcons elementIcons;

    [Header("Stat pods")]
    public TMP_Text hpText;
    public TMP_Text dmgText;
    public TMP_Text spdText;

    [Header("Tabs (lower panels)")]
    public GameObject feedPanel;
    public GameObject skillPanel;
    public GameObject evolutionPanel;

    [Header("Feed panel widgets")]
    public TMP_Text levelText;
    public TMP_Text xpText;
    public Button feedButton;
    public TMP_Text starsLineText;
    public TMP_Text shardsText;
    public Button starButton;

    [Header("Evolution panel widgets")]
    public TMP_Text evolveText;
    public Button evolveButton;

    private int index = -1;
    private OwnedFable Current =>
        (GameData.I != null && index >= 0 && index < GameData.I.player.roster.Count)
            ? GameData.I.player.roster[index] : null;

    public void Open(OwnedFable owned)
    {
        if (GameData.I == null) return;
        index = GameData.I.player.roster.IndexOf(owned);
        if (panelRoot != null) panelRoot.SetActive(true);
        ShowFeed();      // default tab
        Refresh();
    }

    public void Close()
    {
        if (panelRoot != null) panelRoot.SetActive(false);
        if (rosterView != null) rosterView.Refresh();   // grid updates after an evolve
    }

    // ◀ ▶ browse the roster (wraps around)
    public void Next() => Move(1);
    public void Prev() => Move(-1);
    private void Move(int dir)
    {
        var roster = GameData.I.player.roster;
        if (roster.Count == 0) return;
        index = (index + dir + roster.Count) % roster.Count;
        Refresh();
    }

    // bottom-bar tabs: show one lower panel, hide the others
    public void ShowFeed()      => SetTab(0);
    public void ShowSkill()     => SetTab(1);
    public void ShowEvolution() => SetTab(2);
    private void SetTab(int i)
    {
        if (feedPanel != null)      feedPanel.SetActive(i == 0);
        if (skillPanel != null)     skillPanel.SetActive(i == 1);
        if (evolutionPanel != null) evolutionPanel.SetActive(i == 2);
    }

    private void Refresh()
    {
        var o = Current;
        if (o == null) return;
        var def = GameData.I.database.Get(o.definitionId);
        if (def == null) return;

        // header
        if (nameText != null)   nameText.text = def.displayName;
        if (starsText != null)  starsText.text = new string('*', Mathf.Clamp(o.stars, 0, 6));
        if (ratingText != null) ratingText.text = RarityLetter(def.rarity);
        if (elementIcon != null && elementIcons != null) elementIcon.sprite = elementIcons.Get(def.element);
        if (bgTint != null) { var c = RarityColors.Of(def.rarity); c.a = bgTint.color.a; bgTint.color = c; }
        if (artImage != null)
        {
            if (def.art != null) { artImage.sprite = def.art; artImage.color = Color.white; }
            else artImage.color = def.placeholderColor;
        }

        // stat pods
        if (hpText != null)  hpText.text  = FableStats.Health(def, o.level, o.stars).ToString("0");
        if (dmgText != null) dmgText.text = FableStats.Damage(def, o.level, o.stars).ToString("0");
        if (spdText != null) spdText.text = def.attacksPerSecond.ToString("0.0");

        // Feed tab
        if (levelText != null)     levelText.text     = "Lv " + o.level;
        if (xpText != null)        xpText.text        = o.xp + " / " + FableUpgrade.XpPerLevel + " XP";
        if (feedButton != null)    feedButton.interactable = o.xp >= FableUpgrade.XpPerLevel;
        if (starsLineText != null) starsLineText.text = o.stars + " / " + def.maxStars;
        if (shardsText != null)    shardsText.text    = o.shards + " / " + FableUpgrade.ShardsPerStar + " shards";
        if (starButton != null)    starButton.interactable = o.stars < def.maxStars && o.shards >= FableUpgrade.ShardsPerStar;

        // Evolution tab
        bool canEvolve = def.evolvesInto != null && o.stars >= def.maxStars;
        if (evolveButton != null) evolveButton.interactable = canEvolve;
        if (evolveText != null)
            evolveText.text = def.evolvesInto == null ? "This Fable has no evolution."
                            : (canEvolve ? "Ready to evolve!" : "Reach max stars to evolve.");

        if (GameData.I != null) GameData.I.Save();
    }

    public void OnFeed()   { var o = Current; if (o != null && FableUpgrade.TryLevelUp(o)) Refresh(); }
    public void OnStarUp() { var o = Current; if (o != null && FableUpgrade.TryStarUp(o, GameData.I.database.Get(o.definitionId))) Refresh(); }
    public void OnEvolve() { var o = Current; if (o != null && FableUpgrade.TryEvolve(o, GameData.I.database)) Refresh(); }

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
