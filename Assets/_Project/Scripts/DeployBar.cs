using UnityEngine;

public class DeployBar : MonoBehaviour
{
    public Deployer deployer;
    public Transform buttonRow;            // the FableButtonRow (Horizontal Layout Group)
    public DeployFableButton buttonPrefab;

    private void OnEnable() => Build();

    public void Build()
    {
        if (buttonRow == null || GameData.I == null) return;
        foreach (Transform c in buttonRow) Destroy(c.gameObject);
        var roster = GameData.I.player.roster;
        for (int i = 0; i < roster.Count; i++)
        {
            var def = GameData.I.database.Get(roster[i].definitionId);
            Instantiate(buttonPrefab, buttonRow).Setup(i, def, deployer);
        }
    }
}
