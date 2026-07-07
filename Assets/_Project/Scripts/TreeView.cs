using System.Collections.Generic;
using UnityEngine;

public class TreeView : MonoBehaviour
{
    public TreeConfig config;
    public RectTransform[] spotAnchors;
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
            node.transform.localPosition = Vector3.zero;
            node.Bind(config.spots[i], total, icons);
            nodes.Add(node);
        }
    }

    public void PlayLanding(int spotIndex)
    {
        if (spotIndex >= 0 && spotIndex < nodes.Count && nodes[spotIndex] != null)
            nodes[spotIndex].PlayLanding();
    }

    public Vector3 GetSpotWorldPos(int spotIndex)
    {
        if (spotIndex >= 0 && spotIndex < nodes.Count && nodes[spotIndex] != null)
            return nodes[spotIndex].transform.position;
        return transform.position;
    }

    public Sprite GetSpotSprite(int spotIndex)   // NEW: the picture on that spot
    {
        if (spotIndex >= 0 && spotIndex < nodes.Count && nodes[spotIndex] != null)
            return nodes[spotIndex].IconSprite;
        return null;
    }
}
