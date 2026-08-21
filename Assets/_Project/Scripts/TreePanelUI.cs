using UnityEngine;
using TMPro;

public class TreePanelUI : MonoBehaviour
{
    [Header("References")]
    public TreeView treeView;
    public TMP_Text tokenLabel;
    public TMP_Text resultBanner;      // optional
    public TMP_Text pityLabel;         // NEW: "Guaranteed Fable in N"

    [Header("Marching-ants collect")]
    public FlyMote flyMotePrefab;      // the small UI-image prefab
    public Transform flyLayer;         // Canvas child that holds spawned motes
    public int moteCount = 8;
    public float moteStagger = 0.06f;  // gap between motes → the "marching" trail
    public float moteDuration = 0.5f;

    [Header("Fly destinations (drag counters OR nav buttons)")]
    public RectTransform campTarget;      // Gold -> Camp button
    public RectTransform tokenTarget;     // Tree Tokens -> top-bar token counter
    public RectTransform turfWarsTarget;  // Raid Coins -> Turf Wars button
    public RectTransform rosterTarget;    // Fables + Shards -> Roster button

    public void ShowResult(TreeSpot s)
    {
        if (s.rarity >= Rarity.Rare) Handheld.Vibrate();

        int index = (treeView != null && treeView.config != null)
            ? treeView.config.spots.IndexOf(s) : -1;

        if (treeView != null && index >= 0) treeView.PlayLanding(index);

        if (resultBanner != null)
        {
            resultBanner.text = s.label;
            resultBanner.color = RarityColors.Of(s.rarity);
        }

        RectTransform target = TargetFor(s.type);
        if (target != null && treeView != null && index >= 0)
        {
            Vector3 start = treeView.GetSpotWorldPos(index);
            Sprite icon   = treeView.GetSpotSprite(index);
            SpawnBurst(icon, start, target);
        }

        RefreshTokens();
    }

    private RectTransform TargetFor(TreeRewardType type)
    {
        switch (type)
        {
            case TreeRewardType.Gold:        return campTarget;
            case TreeRewardType.TreeTokens:  return tokenTarget;
            case TreeRewardType.RaidCoins:   return turfWarsTarget;
            case TreeRewardType.Fable:       return rosterTarget;
            case TreeRewardType.FableShards: return rosterTarget;
            default:                         return null;   // Xp, Minigame: no fly
        }
    }

    private void SpawnBurst(Sprite icon, Vector3 start, RectTransform target)
    {
        if (flyMotePrefab == null || flyLayer == null) return;
        Vector3 end = target.position;
        for (int i = 0; i < moteCount; i++)
        {
            FlyMote mote = Instantiate(flyMotePrefab, flyLayer);
            mote.Launch(icon, start, end, i * moteStagger, moteDuration);  // staggered = marching line
        }
    }

    public void RefreshTokens()
    {
        if (tokenLabel != null && GameData.I != null)
            tokenLabel.text = "Tokens: " + GameData.I.player.treeTokens;

        // NEW: pity readout
        if (pityLabel != null && GameData.I != null
            && treeView != null && treeView.config != null)
        {
            int remaining = Mathf.Max(0, treeView.config.fablePity - GameData.I.player.pityCounter);
            pityLabel.text = "Guaranteed Fable in " + remaining;
        }
    }

    private void Start()
    {
        RefreshTokens();
    }
}