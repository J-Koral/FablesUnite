using UnityEngine;
using UnityEngine.EventSystems;

public class TreePullButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public TreeController tree;
    public TreePanelUI ui;          // shows the result (Stage 7)
    public float interval = 0.12f;  // time between auto-pulls while holding

    private bool held;
    private int firedThisHold;
    private float timer;

    public void OnPointerDown(PointerEventData e)
    {
        held = true; firedThisHold = 0; timer = 0f;
        DoPull();                    // first pull is instant
    }
    public void OnPointerUp(PointerEventData e) => held = false;

    private void Update()
    {
        if (!held || firedThisHold >= 15) return;
        timer -= Time.unscaledDeltaTime;
        if (timer <= 0f) { DoPull(); timer = interval; }
    }

    private void DoPull()
    {
        var spot = tree.Pull();
        if (spot == null) { held = false; return; }   // out of tokens
        firedThisHold++;
        if (ui != null) ui.ShowResult(spot);
    }
}