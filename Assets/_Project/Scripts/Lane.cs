using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
    [Header("Lane Geometry (drag the child markers here)")]
    public Transform spawnPoint;          // right end: where enemies appear
    public Transform backLine;            // left end: the fail line
    public Transform[] defenderSlots;     // placement spots, filled left-to-right

    // Every unit currently in this lane registers itself here.
    private readonly List<Unit> units = new List<Unit>();

    public void Register(Unit u)
    {
        if (!units.Contains(u)) units.Add(u);
    }

    public void Unregister(Unit u)
    {
        units.Remove(u);
    }

    // Nearest living enemy of the given unit, measured along the lane (X axis).
    public Unit GetNearestEnemy(Unit self)
    {
        Unit best = null;
        float bestDist = float.MaxValue;
        for (int i = 0; i < units.Count; i++)
        {
            Unit u = units[i];
            if (u == null || !u.IsAlive || u.team == self.team) continue;
            float d = Mathf.Abs(u.transform.position.x - self.transform.position.x);
            if (d < bestDist) { bestDist = d; best = u; }
        }
        return best;
    }

    // Nearest friendly unit directly ahead (closer to the back line), used to keep spacing.
    public Unit GetNearestAllyAhead(Unit self, float spacing)
    {
        Unit best = null;
        float bestDist = float.MaxValue;
        for (int i = 0; i < units.Count; i++)
        {
            Unit u = units[i];
            if (u == null || !u.IsAlive || u == self || u.team != self.team) continue;
            float dx = self.transform.position.x - u.transform.position.x; // >0 means u is ahead
            if (dx > 0f && dx < spacing && dx < bestDist) { bestDist = dx; best = u; }
        }
        return best;
    }

    // Has any attacker crossed our back line? (Lose condition.)
    public bool AnyAttackerPastBackLine()
    {
        float lineX = backLine.position.x;
        for (int i = 0; i < units.Count; i++)
        {
            Unit u = units[i];
            if (u != null && u.IsAlive && u.team == Team.Attacker && u.transform.position.x <= lineX)
                return true;
        }
        return false;
    }

    public int AttackerCount()
    {
        int count = 0;
        for (int i = 0; i < units.Count; i++)
        {
            Unit u = units[i];
            if (u != null && u.IsAlive && u.team == Team.Attacker) count++;
        }
        return count;
    }

    // Next empty defender slot, or null if the lane is full.
    public Transform GetNextFreeSlot()
    {
        int defenders = 0;
        for (int i = 0; i < units.Count; i++)
        {
            Unit u = units[i];
            if (u != null && u.IsAlive && u.team == Team.Defender) defenders++;
        }
        if (defenderSlots == null || defenders >= defenderSlots.Length) return null;
        return defenderSlots[defenders];
    }
}
