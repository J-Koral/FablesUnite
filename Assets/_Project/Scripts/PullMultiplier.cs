using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PullMultiplier : MonoBehaviour
{
    public static PullMultiplier I;

    [Header("Tiers")]
    public int[] tiers = { 1, 10 };       // extend to { 1, 5, 10 } later
    private int tierIndex = 0;
    public int Current => tiers[tierIndex];

    [Header("Toggle button")]
    public TMP_Text multiplierLabel;      // the "1x" text on Btn_Multiplier

    [Header("Pull button reaction")]
    public RectTransform pullButton;      // Btn_Pull rect
    public GameObject pullGlow;           // PullGlow child
    public TMP_Text tokenLabel;           // Btn_Pull's TokenLabel
    public Vector3 baseScale = Vector3.one;
    public Vector3 bigScale  = new Vector3(1.15f, 1.15f, 1f);

    private void Awake() { I = this; }
    private void Start() { Apply(); }

    // hook this to Btn_Multiplier's OnClick
    public void Cycle()
    {
        tierIndex = (tierIndex + 1) % tiers.Length;
        Apply();
    }

    private void Apply()
    {
        if (multiplierLabel != null) multiplierLabel.text = Current + "x";

        bool high = Current >= 10;
        if (pullButton != null) pullButton.localScale = high ? bigScale : baseScale;
        if (pullGlow  != null)  pullGlow.SetActive(high);

        if (tokenLabel != null) tokenLabel.text = "x" + Current;   // shows the cost weight
    }
}
