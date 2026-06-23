using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData I { get; private set; }   // global access point

    [Tooltip("Drag your FableDatabase asset here.")]
    public FableDatabase database;

    public PlayerData player = new PlayerData();

    private void Awake()
    {
        if (I != null) { Destroy(gameObject); return; }  // only one survives
        I = this;
        DontDestroyOnLoad(gameObject);                    // persists across scenes
        if (player.roster.Count == 0) GiveStarterRoster();
    }

    private void GiveStarterRoster()
    {
        // Hand the player their first two Fables.
        if (database == null || database.all.Count == 0) return;
        player.roster.Add(new OwnedFable(database.all[0].name));
        player.roster.Add(new OwnedFable(database.all[1].name));
        Debug.Log($"Starter roster: {player.roster.Count} Fables, {player.treeTokens} tokens.");
    }
}
