using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawn
{
    [Tooltip("Which lane: 0 = Lane 1, 1 = Lane 2, 2 = Lane 3.")]
    public int laneIndex;
    public Element element = Element.Water;
}

[System.Serializable]
public class Wave
{
    public List<EnemySpawn> enemies = new List<EnemySpawn>();
    [Tooltip("Seconds between each enemy within this wave.")]
    public float gapBetweenSpawns = 0.5f;
    [Tooltip("Seconds to wait after this wave before the next.")]
    public float delayAfterWave = 5f;
}

public class WaveSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject enemyPrefab;
    public Lane[] lanes;   // size 3, in order

    [Header("Waves")]
    public List<Wave> waves = new List<Wave>();

    public bool Finished { get; private set; }

    public void BeginSpawning()
    {
        Finished = false;
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        foreach (Wave wave in waves)
        {
            foreach (EnemySpawn spawn in wave.enemies)
            {
                SpawnOne(spawn);
                yield return new WaitForSeconds(wave.gapBetweenSpawns);
            }
            yield return new WaitForSeconds(wave.delayAfterWave);
        }
        Finished = true;
    }

    private void SpawnOne(EnemySpawn spawn)
    {
        if (spawn.laneIndex < 0 || spawn.laneIndex >= lanes.Length) return;
        Lane lane = lanes[spawn.laneIndex];

        GameObject go = Instantiate(enemyPrefab, lane.spawnPoint.position, Quaternion.identity);
        Unit unit = go.GetComponent<Unit>();
        unit.team = Team.Attacker;
        unit.element = spawn.element;
        unit.AssignLane(lane);
    }

    //  private void Start() { BeginSpawning(); }   // TEMPORARY — delete after this test
}
