using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FableDetailPanel : MonoBehaviour
{
    [Header("Root & screen")]
    public GameObject panelRoot;
    public Image heroBackdrop;      // top-half backing, tinted by rarity

    [Header("Hero")]
    public Image artImage;
    public Image elementIcon;
    public TMP_Text nameText;
    public TMP_Text starsText;      // ★ filled + ☆ empty, up to maxStars
    public ElementIcons elementIcons;

    [Header("Stats")]
    public TMP_Text hpText;
    public TMP_Text dmgText;
    public TMP_Text spdText;

    [Header("Tabs (order: Feed, Skill, Evolve)")]
    public GameObject feedPanel;
    public GameObject skillPanel;
    public GameObject evolutionPanel;
    public Image[] tabBackgrounds;
    public TMP_Text[] tabLabels;
    public GameObject[] tabIndicators;   // optional under-line per tab
    public Color tabActiveColor   = new Color(0.184f, 0.165f, 0.388f);
    public Color tabInactiveColor = new Color(0.10f, 0.10f, 0.13f);
    public Color tabActiveText    = Color.white;
    public Color tabInactiveText  = new Color(0.53f, 0.52f, 0.56f);

    [Header("Feed tab")]
    public TMP_Text levelText;
    public TMP_Text xpText;
    public Image xpBar;             // Image Type = Filled, Horizontal
    public Button feedButton;

    [Header("Evolve tab")]
    public TMP_Text starsLineText;
    public TMP_Text shardsText;
    public Image shardBar;          // Image Type = Filled, Horizontal
    public Button starButton;
    public GameObject starGlow;     // shown + pulses when a star-up is affordable
    public TMP_Text evolveText;
    public Button evolveButton;

    [Header("Grid refresh")]
    public RosterView rosterView;   // refresh the grid on close (e.g. after evolve)

    private int index = -1;

    private OwnedFable Current =>
        (GameData.I != null && index >= 0 && index < GameData.I.player.roster.Count)
            ? GameData.I.player.roster[index] : null;

    public void Open(OwnedFable owned)
    {
        if (GameData.I == null) return;
        index = GameData.I.player.roster.IndexOf(owned);
        if (panelRoot != null) panelRoot.SetActive(true);
        ShowFeed();
        Refresh();
    }

    public void Close()
    {
        if (panelRoot != null) panelRoot.SetActive(false);
        if (rosterView != null) rosterView.Refresh();
    }

    // ◀ ▶ flip through the roster (wraps)
    public void Next() => Move(1);
    public void Prev() => Move(-1);
    private void Move(int dir)
    {
        var roster = GameData.I.player.roster;
        if (roster.Count == 0) return;
        index = (index + dir + roster.Count) % roster.Count;
        Refresh();
    }

    // bottom-bar tabs
    public void ShowFeed()      => SetTab(0);
    public void ShowSkill()     => SetTab(1);
    public void ShowEvolution() => SetTab(2);

    private void SetTab(int i)
    {
        if (feedPanel != null)      feedPanel.SetActive(i == 0);
        if (skillPanel != null)     skillPanel.SetActive(i == 1);
        if (evolutionPanel != null) evolutionPanel.SetActive(i == 2);

        if (tabBackgrounds != null)
            for (int t = 0; t < tabBackgrounds.Length; t++)
                if (tabBackgrounds[t]) tabBackgrounds[t].color = (t == i) ? tabActiveColor : tabInactiveColor;

        if (tabLabels != null)
            for (int t = 0; t < tabLabels.Length; t++)
                if (tabLabels[t]) tabLabels[t].color = (t == i) ? tabActiveText : tabInactiveText;

        if (tabIndicators != null)
            for (int t = 0; t < tabIndicators.Length; t++)
                if (tabIndicators[t]) tabIndicators[t].SetActive(t == i);
    }

    private void Refresh()
    {
        var o = Current;
        if (o == null) return;
        var def = GameData.I.database.Get(o.definitionId);
        if (def == null) return;

        // Hero
        if (nameText != null)  nameText.text = def.displayName;
        if (starsText != null) starsText.text = StarString(o.stars, def.maxStars);
        if (elementIcon != null && elementIcons != null) elementIcon.sprite = elementIcons.Get(def.element);
        if (heroBackdrop != null) heroBackdrop.color = RarityColors.Of(def.rarity);
        if (artImage != null)
        {
            if (def.art != null) { artImage.sprite = def.art; artImage.color = Color.white; }
            else artImage.color = def.placeholderColor;
        }

        // Stats
        if (hpText != null)  hpText.text  = FableStats.Health(def, o.level, o.stars).ToString("0");
        if (dmgText != null) dmgText.text = FableStats.Damage(def, o.level, o.stars).ToString("0");
        if (spdText != null) spdText.text = def.attacksPerSecond.ToString("0.0");

        // Feed tab
        if (levelText != null) levelText.text = "Level " + o.level;
        if (xpText != null)    xpText.text    = o.xp + " / " + FableUpgrade.XpPerLevel + " XP";
        if (xpBar != null)     xpBar.fillAmount = Mathf.Clamp01(o.xp / (float)FableUpgrade.XpPerLevel);
        if (feedButton != null) feedButton.interactable = o.xp >= FableUpgrade.XpPerLevel;

        // Evolve tab — star-up
        bool canStar = o.stars < def.maxStars && o.shards >= FableUpgrade.ShardsPerStar;
        if (starsLineText != null) starsLineText.text = "Stars " + o.stars + " / " + def.maxStars;
        if (shardsText != null)    shardsText.text    = o.shards + " / " + FableUpgrade.ShardsPerStar + " shards";
        if (shardBar != null)      shardBar.fillAmount = Mathf.Clamp01(o.shards / (float)FableUpgrade.ShardsPerStar);
        if (starButton != null)    starButton.interactable = canStar;
        if (starGlow != null)      starGlow.SetActive(canStar);   // glow appears only when ready

        // Evolve tab — evolution
        bool canEvolve = def.evolvesInto != null && o.stars >= def.maxStars;
        if (evolveButton != null) evolveButton.interactable = canEvolve;
        if (evolveText != null)
            evolveText.text = def.evolvesInto == null ? "This Fable has no evolution."
                            : (canEvolve ? "Ready to evolve!" : "Reach max stars to evolve.");

        if (GameData.I != null) GameData.I.Save();
    }

    // makes the Star Up glow gently pulse while it's active
    private void Update()
    {
        if (starGlow != null && starGlow.activeSelf)
        {
            float s = 1f + 0.06f * Mathf.Sin(Time.unscaledTime * 6f);
            starGlow.transform.localScale = new Vector3(s, s, 1f);
        }
    }

    public void OnFeed()   { var o = Current; if (o != null && FableUpgrade.TryLevelUp(o)) Refresh(); }
    public void OnStarUp() { var o = Current; if (o != null && FableUpgrade.TryStarUp(o, GameData.I.database.Get(o.definitionId))) Refresh(); }
    public void OnEvolve() { var o = Current; if (o != null && FableUpgrade.TryEvolve(o, GameData.I.database)) Refresh(); }

    private string StarString(int stars, int max)
    {
        stars = Mathf.Clamp(stars, 0, max);
        return new string('\u2605', stars) + new string('\u2606', Mathf.Max(0, max - stars));
    }
}
