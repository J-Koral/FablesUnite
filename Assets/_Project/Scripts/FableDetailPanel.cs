using UnityEngine;
using TMPro;

public class FableDetailPanel : MonoBehaviour
{
    public GameObject panelRoot;
    public TMP_Text nameText, statsText, reqText;
    private OwnedFable current;

    public void Open(OwnedFable owned)
    {
        current = owned;
        panelRoot.SetActive(true);
        Refresh();
    }

    public void Close() => panelRoot.SetActive(false);

    private void Refresh()
    {
        var def = GameData.I.database.Get(current.definitionId);
        nameText.text  = def.displayName;
        statsText.text = $"Lv {current.level}   {current.stars}\u2605\n" +
                         $"HP {FableStats.Health(def, current.level, current.stars):0}\n" +
                         $"DMG {FableStats.Damage(def, current.level, current.stars):0}";
        reqText.text   = $"XP {current.xp}/{FableUpgrade.XpPerLevel}    " +
                         $"Shards {current.shards}/{FableUpgrade.ShardsPerStar}";
        if (GameData.I != null) GameData.I.Save();
    }

    public void OnFeed()   { if (FableUpgrade.TryLevelUp(current)) Refresh(); }
    public void OnStarUp() { if (FableUpgrade.TryStarUp(current, GameData.I.database.Get(current.definitionId))) Refresh(); }
    public void OnEvolve() { if (FableUpgrade.TryEvolve(current, GameData.I.database)) Refresh(); }
}
