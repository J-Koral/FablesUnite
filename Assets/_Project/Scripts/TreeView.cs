using System.Collections.Generic;
using UnityEngine;

public class TreeView : MonoBehaviour
{
    public TreeConfig config;
    public RectTransform[] spotAnchors;   // the Anchor_0..N you placed on the tree
    public TreeSpotUI spotPrefab;
    public TreeIconLibrary icons;

    private readonly List<TreeSpotUI> nodes = new List<TreeSpotUI>();

    private void Start() => Build();

    public void Build()
    {
        foreach (var n in nodes) if (n != null) Destroy(n.gameObject);
        nodes.Clear();

        float total = config.TotalWeight();
        int count = Mathf.Min(config.spots.Count, spotAnchors.Length);
        for (int i = 0; i < count; i++)
        {
            TreeSpotUI node = Instantiate(spotPrefab, spotAnchors[i]);
            node.transform.localPosition = Vector3.zero;   // sit exactly on the anchor
            node.Bind(config.spots[i], total, icons);
            nodes.Add(node);
        }
    }

    // Light up the spot a pull landed on.
    public void PlayLanding(int spotIndex)
    {
        if (spotIndex >= 0 && spotIndex < nodes.Count && nodes[spotIndex] != null)
            nodes[spotIndex].PlayLanding();
    }

    // Where a spot's node is on screen (used as the fly-from point).
    public Vector3 GetSpotWorldPos(int spotIndex)
    {
        if (spotIndex >= 0 && spotIndex < nodes.Count && nodes[spotIndex] != null)
            return nodes[spotIndex].transform.position;
        return transform.position;
    }
}