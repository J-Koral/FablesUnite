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

    private Coroutine repeatRoutine;

    public void OnPointerDown(PointerEventData e)
    {
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
        yield return new WaitForSeconds(initialDelay);   // the "hold pause", like a keyboard
        while (true)
        {
            DoPull();
            yield return new WaitForSeconds(repeatInterval);
        }
    }

    private void DoPull()
    {
        if (tree == null) return;
        TreeSpot spot = tree.Pull();
        if (spot == null) return;                        // out of tokens: stop quietly
        if (ui != null) ui.ShowResult(spot);
    }
}
