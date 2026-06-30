using UnityEngine;

public class TreeView : MonoBehaviour
{
    public TreeConfig config;
    public Transform content;        // the Scroll View Content
    public TreeSpotUI spotPrefab;

    private void OnEnable()
    {
        if (config == null || content == null || spotPrefab == null) return;
        Build();
    }

    public void Build()
    {
        foreach (Transform c in content) Destroy(c.gameObject);
        float total = config.TotalWeight();
        foreach (var spot in config.spots)
            Instantiate(spotPrefab, content).Bind(spot, total);
    }
}