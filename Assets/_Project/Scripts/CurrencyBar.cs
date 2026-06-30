using UnityEngine;
using TMPro;

public class CurrencyBar : MonoBehaviour
{
    public TMP_Text goldText;
    public TMP_Text tokenText;
    public TMP_Text coinText;

    private void Update()
    {
        if (GameData.I == null) return;
        var p = GameData.I.player;
        if (goldText  != null) goldText.text  = p.gold.ToString();
        if (tokenText != null) tokenText.text = p.treeTokens.ToString();
        if (coinText  != null) coinText.text  = p.raidCoins.ToString();
    }
}
