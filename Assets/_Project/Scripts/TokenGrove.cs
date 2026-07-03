using UnityEngine;

public class TokenGrove : MonoBehaviour
{
    public int tokensPerCollect = 10;

    // Wire a "COLLECT" button on the Camp panel to this.
    public void Collect()
    {
        GameData.I.player.treeTokens += tokensPerCollect;
        GameData.I.Save();
    }
}
