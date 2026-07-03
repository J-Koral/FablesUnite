using UnityEngine;

public class RosterView : MonoBehaviour
{
    public Transform content;          // the Scroll View Content
    public FableCardUI cardPrefab;
    public FableDetailPanel detailPanel;   // <-- ADD THIS FIELD

    private void OnEnable() => Refresh();

    public void Refresh()
    {
        foreach (Transform c in content) Destroy(c.gameObject);
        var data = GameData.I;
        foreach (var owned in data.player.roster)
        {
            var def = data.database.Get(owned.definitionId);
            if (def == null) continue;
            var card = Instantiate(cardPrefab, content);
            card.Bind(owned, def);

            // ADD THIS BLOCK: clicking the card opens the detail panel for THIS Fable
            var btn = card.GetComponent<UnityEngine.UI.Button>();
            if (btn != null)
            {
                var captured = owned;               // capture this loop's Fable
                btn.onClick.AddListener(() => detailPanel.Open(captured));
            }
        }
    }
}