using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

public class ReconDisplay : MonoBehaviour
{
    public WaveSpawner waveSpawner;
    public TMP_Text[] laneLabels; // one label per lane, index matches lane

    private void Start()
    {
        int laneCount = laneLabels.Length;
        var counts = new Dictionary<Element, int>[laneCount];
        for (int i = 0; i < laneCount; i++) counts[i] = new Dictionary<Element, int>();

        foreach (Wave wave in waveSpawner.waves)
            foreach (EnemySpawn s in wave.enemies)
            {
                if (s.laneIndex < 0 || s.laneIndex >= laneCount) continue;
                var d = counts[s.laneIndex];
                if (!d.ContainsKey(s.element)) d[s.element] = 0;
                d[s.element]++;
            }

        for (int i = 0; i < laneCount; i++)
        {
            var sb = new StringBuilder("Lane " + (i + 1) + ":  ");
            int total = 0;
            foreach (var kvp in counts[i]) { sb.Append(kvp.Value + "x " + kvp.Key + "   "); total += kvp.Value; }
            if (total == 0) sb.Append("(clear)");
            laneLabels[i].text = sb.ToString();
        }
    }
}
