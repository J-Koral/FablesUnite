using UnityEngine;
using UnityEngine.UI;

public class FeverMeter : MonoBehaviour
{
    public static FeverMeter I;

    [Header("UI")]
    public Image feverBar;              // Filled bar
    public Image treeBackground;        // TreeBackground image to tint

    [Header("Tuning")]
    public int pullsToFever = 20;       // fills over this many pulls
    public int chargedPulls = 3;        // how many guaranteed rares Fever grants
    public Color normalTint = Color.white;
    public Color feverTint  = new Color(1f, 0.75f, 0.55f);

    private int progress;
    private int charge;                 // remaining forced-rare pulls
    public bool IsFever { get; private set; }

    // TreeController asks this each pull: "should this pull be forced rare?"
    public bool ConsumeChargedPull()
    {
        if (!IsFever || charge <= 0) return false;
        charge--;
        if (charge <= 0) EndFever();
        return true;
    }

    private void Awake() { I = this; }

    // call once per pull from TreeController
    public void RegisterPull()
    {
        if (IsFever) return;
        progress++;
        if (feverBar != null) feverBar.fillAmount = Mathf.Clamp01(progress / (float)pullsToFever);
        if (progress >= pullsToFever) EnterFever();
    }

    private void EnterFever()
    {
        IsFever = true;
        charge = chargedPulls;
        if (treeBackground != null) treeBackground.color = feverTint;
        // AudioManager.I?.Play(AudioManager.I.fanfare);   // optional, once Part 17 clips exist
    }

    public void EndFever()
    {
        IsFever = false;
        progress = 0;
        if (feverBar != null) feverBar.fillAmount = 0f;
        if (treeBackground != null) treeBackground.color = normalTint;
    }
}
