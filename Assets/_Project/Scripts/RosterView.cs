using UnityEngine;
using UnityEngine.UI;

public class RosterView : MonoBehaviour
{
    public Transform content;             // the Scroll View Content (Grid Layout Group)
    public FableCardUI cardPrefab;
    public FableDetailPanel detailPanel;
    public ElementIcons elementIcons;

    private void OnEnable() => Refresh();  // rebuild when the scene/roster opens

    public void Refresh()
    {
        if (content == null || GameData.I == null) return;
        foreach (Transform c in content) Destroy(c.gameObject);

        foreach (var owned in GameData.I.player.roster)
        {
            var def = GameData.I.database.Get(owned.definitionId);
            if (def == null) continue;

            var card = Instantiate(cardPrefab, content);
            card.Bind(owned, def, elementIcons);

            var btn = card.GetComponent<Button>();
            if (btn != null)
            {
                var captured = owned;                          // capture per-card (avoids closure bug)
                btn.onClick.AddListener(() => detailPanel.Open(captured));
            }
        }
    }
}
