using UnityEngine;

public class RosterView : MonoBehaviour
{
    public Transform content;          // the Scroll View Content
    public FableCardUI cardPrefab;

    private void OnEnable() => Refresh();  // rebuild every time the panel opens

    public void Refresh()
    {
        foreach (Transform c in content) Destroy(c.gameObject); // clear old
        var data = GameData.I;
        foreach (var owned in data.player.roster)
        {
            var def = data.database.Get(owned.definitionId);
            if (def == null) continue;
            var card = Instantiate(cardPrefab, content);
            card.Bind(owned, def);
        }
    }
}
