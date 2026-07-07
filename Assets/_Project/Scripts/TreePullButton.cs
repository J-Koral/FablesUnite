using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class TreePullButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public TreeController tree;
    public TreePanelUI ui;

    [Header("Keyboard-style repeat")]
    public float initialDelay = 0.5f;     // pause after the first pull before spamming
    public float repeatInterval = 0.12f;  // spam speed while held
    public int maxPullsPerHold = 15;       // cap per press-and-hold

    private Coroutine repeatRoutine;
    private int pullsThisHold;

    public void OnPointerDown(PointerEventData e)
    {
        pullsThisHold = 0;                               // fresh count each press
        DoPull();                                        // exactly ONE pull on press
        repeatRoutine = StartCoroutine(RepeatAfterDelay());
    }

    public void OnPointerUp(PointerEventData e)
    {
        if (repeatRoutine != null) StopCoroutine(repeatRoutine);
        repeatRoutine = null;

        if (tree != null && tree.pendingMinigames > 0)   // present queued minigames on release
        {
            Debug.Log(tree.pendingMinigames + " minigame(s) to play!");
            tree.pendingMinigames = 0;
        }
    }

    private IEnumerator RepeatAfterDelay()
    {
        yield return new WaitForSeconds(initialDelay);   // the "hold pause"
        while (pullsThisHold < maxPullsPerHold)          // stop at the cap
        {
            DoPull();
            yield return new WaitForSeconds(repeatInterval);
        }
    }

    private void DoPull()
    {
        if (tree == null) return;
        if (pullsThisHold >= maxPullsPerHold) return;    // safety: never exceed the cap
        TreeSpot spot = tree.Pull();
        if (spot == null) return;                        // out of tokens: stop quietly
        pullsThisHold++;                                 // only count successful pulls
        if (ui != null) ui.ShowResult(spot);
    }
}