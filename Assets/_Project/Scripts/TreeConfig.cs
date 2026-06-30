using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TreeConfig", menuName = "Fables Unite/Tree Config")]
public class TreeConfig : ScriptableObject
{
    public List<TreeSpot> spots = new List<TreeSpot>();
    [Tooltip("Guarantee a Fable by this many pulls without one.")]
    public int fablePity = 50;

    public float TotalWeight()
    {
        float t = 0f;
        foreach (var s in spots) t += Mathf.Max(0f, s.weight);
        return t;
    }
}
