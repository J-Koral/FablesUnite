using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FableDatabase", menuName = "Fables Unite/Fable Database")]
public class FableDatabase : ScriptableObject
{
    public List<FableDefinition> all = new List<FableDefinition>();

    public FableDefinition Get(string id)
    {
        foreach (var f in all) if (f != null && f.name == id) return f;
        return null;
    }
}

