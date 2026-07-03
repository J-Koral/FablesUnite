using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData I { get; private set; }   // global access point

    [Tooltip("Drag your FableDatabase asset here.")]
    public FableDatabase database;

    public PlayerData player = new PlayerData();

    private void Awake()
    {
        if (I != null) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);

        var loaded = SaveSystem.Load();          // <-- load first
        if (loaded != null) player = loaded;
        else if (player.roster.Count == 0) GiveStarterRoster();
    }

    public void Save() => SaveSystem.Save(player);
    private void OnApplicationPause(bool paused) { if (paused) Save(); }
    private void OnApplicationQuit() => Save();


    private void GiveStarterRoster()
    {
        // Hand the player their first two Fables.
        if (database == null || database.all.Count == 0) return;
        player.roster.Add(new OwnedFable(database.all[0].name));
        player.roster.Add(new OwnedFable(database.all[1].name));
        Debug.Log($"Starter roster: {player.roster.Count} Fables, {player.treeTokens} tokens.");
    }
}
