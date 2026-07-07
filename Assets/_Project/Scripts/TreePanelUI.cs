using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TreePanelUI : MonoBehaviour
{
    [Header("References")]
    public TreeView treeView;          // to find the landed node + its position
    public TMP_Text tokenLabel;
    public TMP_Text resultBanner;      // optional; leave unwired if unused

    [Header("Fly-to-collect")]
    public RectTransform flyIcon;      // a small icon that flies; starts hidden

    [Header("Fly destinations (drag counters OR nav buttons)")]
    public RectTransform campTarget;      // Gold flies here (Camp button)
    public RectTransform tokenTarget;     // Tree Tokens -> top-bar token counter
    public RectTransform turfWarsTarget;  // Raid Coins -> Turf Wars button
    public RectTransform rosterTarget;    // Fables + Shards -> Roster button

    public void ShowResult(TreeSpot s)
    {
        if (s.rarity >= Rarity.Rare) Handheld.Vibrate();   // no-op in editor, fine

        // Which spot did we land on? spots are shared objects, so IndexOf finds it.
        int index = (treeView != null && treeView.config != null)
            ? treeView.config.spots.IndexOf(s) : -1;

        if (treeView != null && index >= 0) treeView.PlayLanding(index);   // B4: glow

        if (resultBanner != null)
        {
            resultBanner.text = s.label;
            resultBanner.color = RarityColors.Of(s.rarity);
        }

        // B5: fly the reward from the tree spot to where it's used.
        RectTransform target = TargetFor(s.type);
        Vector3 start = (treeView != null && index >= 0)
            ? treeView.GetSpotWorldPos(index)
            : transform.position;
        if (flyIcon != null && target != null)
            StartCoroutine(Fly(start, target, RarityColors.Of(s.rarity)));

        RefreshTokens();
    }

    // Routes each reward type to the button or counter it belongs to.
    private RectTransform TargetFor(TreeRewardType type)
    {
        switch (type)
        {
            case TreeRewardType.Gold:        return campTarget;
            case TreeRewardType.TreeTokens:  return tokenTarget;
            case TreeRewardType.RaidCoins:   return turfWarsTarget;
            case TreeRewardType.Fable:       return rosterTarget;
            case TreeRewardType.FableShards: return rosterTarget;
            default:                         return null;   // Xp, Minigame just glow
        }
    }

    private IEnumerator Fly(Vector3 start, RectTransform target, Color color)
    {
        Image img = flyIcon.GetComponent<Image>();
        if (img != null) img.color = color;
        flyIcon.gameObject.SetActive(true);
        flyIcon.position = start;

        Vector3 end = target.position;
        float t = 0f, dur = 0.45f;
        while (t < dur)
        {
            t += Time.unscaledDeltaTime;
            float k = t / dur;
            flyIcon.position   = Vector3.Lerp(start, end, k);
            flyIcon.localScale = Vector3.one * Mathf.Lerp(1f, 0.05f, k);  // shrinks into the button
            yield return null;
        }
        flyIcon.gameObject.SetActive(false);
        flyIcon.localScale = Vector3.one;   // reset for next time
        RefreshTokens();
    }

    public void RefreshTokens()
    {
        if (tokenLabel != null && GameData.I != null)
            tokenLabel.text = "Tokens: " + GameData.I.player.treeTokens;
    }
}