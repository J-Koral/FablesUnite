using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public enum BattleState { Deploy, Battle, Win, Lose }

public class BattleManager : MonoBehaviour
{
    [Header("References")]
    public WaveSpawner waveSpawner;
    public Lane[] lanes;

    [Header("UI (assign in Stage 8)")]
    public GameObject deployUI;
    public GameObject resultUI;
    public TMP_Text resultText;

    public BattleState CurrentState { get; private set; } = BattleState.Deploy;

    private void Start()
    {
        EnterDeploy();
    }

    private void Update()
    {
        if (CurrentState == BattleState.Battle) CheckEndConditions();
    }

    // Called by the Start Battle button.
    public void StartBattle()
    {
        if (CurrentState != BattleState.Deploy) return;
        CurrentState = BattleState.Battle;
        if (deployUI != null) deployUI.SetActive(false);
        waveSpawner.BeginSpawning();
    }

    private void CheckEndConditions()
    {
        // Lose: any enemy crossed a back line.
        for (int i = 0; i < lanes.Length; i++)
        {
            if (lanes[i].AnyAttackerPastBackLine()) { EndBattle(false); return; }
        }

        // Win: all waves done AND no enemies remain.
        if (waveSpawner.Finished)
        {
            int left = 0;
            for (int i = 0; i < lanes.Length; i++) left += lanes[i].AttackerCount();
            if (left == 0) EndBattle(true);
        }
    }

    private void EndBattle(bool won)
    {
        CurrentState = won ? BattleState.Win : BattleState.Lose;
        Time.timeScale = 1f;
        if (resultUI != null) resultUI.SetActive(true);
        if (resultText != null) resultText.text = won ? "VICTORY" : "DEFEAT";
    }

    private void EnterDeploy()
    {
        CurrentState = BattleState.Deploy;
        Time.timeScale = 1f;
        if (deployUI != null) deployUI.SetActive(true);
        if (resultUI != null) resultUI.SetActive(false);
    }

    // --- Hooked to speed buttons (pass 1, 2, or 4) ---
    public void SetSpeed(float multiplier)
    {
        Time.timeScale = multiplier;
    }

    // --- Hooked to the Restart button ---
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
