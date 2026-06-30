using UnityEngine;
using TMPro;

public class TokenLabel : MonoBehaviour
{
    public TMP_Text label;
    private void OnEnable()
    {
        if (GameData.I != null) label.text = "Tokens: " + GameData.I.player.treeTokens;
    }
}