using UnityEngine;
using TMPro;
using System.Collections;

public class TreePanelUI : MonoBehaviour
{
    [Header("Token + result banner")]
    public TMP_Text tokenLabel;
    public TMP_Text resultBanner;     // flashes the reward in its rarity color

    [Header("Fly-to-collect")]
    public RectTransform flyIcon;     // a small icon that flies to a currency
    public RectTransform goldTarget;  // where Gold rewards fly to (top bar)
    public RectTransform tokenTarget; // where Tree Token rewards fly to
    public RectTransform coinTarget;  // where Raid Coin rewards fly to

    public void ShowResult(TreeSpot s)
    {
        if (s.rarity >= Rarity.Rare) Handheld.Vibrate();   // no-op in editor, fine

        if (resultBanner != null)
        {
            resultBanner.text = s.label;
            resultBanner.color = RarityColors.Of(s.rarity);
        }

        // pick the destination based on what was won
        RectTransform target = s.type switch
        {
            TreeRewardType.Gold        => goldTarget,
            TreeRewardType.TreeTokens  => tokenTarget,
            TreeRewardType.RaidCoins   => coinTarget,
            _                          => null
        };
        if (flyIcon != null && target != null)
            StartCoroutine(Fly(target, RarityColors.Of(s.rarity)));

        RefreshTokens();
    }

    private IEnumerator Fly(RectTransform target, Color color)
    {
        // start at the result banner (or panel center), fly to the target
        Vector3 start = resultBanner != null ? resultBanner.transform.position : transform.position;
        Vector3 end = target.position;

        var img = flyIcon.GetComponent<UnityEngine.UI.Image>();
        if (img != null) img.color = color;
        flyIcon.gameObject.SetActive(true);

        float t = 0f, dur = 0.45f;
        while (t < dur)
        {
            t += Time.unscaledDeltaTime;
            float k = t / dur;
            flyIcon.position = Vector3.Lerp(start, end, k);
            flyIcon.localScale = Vector3.one * Mathf.Lerp(1f, 0.4f, k); // shrink as it lands
            yield return null;
        }
        flyIcon.gameObject.SetActive(false);
        RefreshTokens();  // counts update right as it "lands"
    }

    public void RefreshTokens()
    {
        if (tokenLabel != null && GameData.I != null)
            tokenLabel.text = "Tokens: " + GameData.I.player.treeTokens;
    }
}
