using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TreeSpotUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Visuals")]
    public Image iconImage;   // the prize picture (this object's own Image)
    public Image glow;        // flashes on landing (starts hidden)

    [Header("Hold to reveal odds")]
    public float holdDelay = 0.35f;

    public Sprite IconSprite { get; private set; }   // what this spot shows (used by the fly burst)

    private float pct;
    private string label;
    private Rarity rarity;
    private Coroutine holdRoutine;
    private Coroutine landingRoutine;
    private Vector3 originalScale;
    private bool captured;

    private void Awake()
    {
        originalScale = transform.localScale;   // remember the true size ONCE
        captured = true;
    }

    public void Bind(TreeSpot spot, float totalWeight, TreeIconLibrary icons)
    {
        label  = spot.label;
        rarity = spot.rarity;
        pct    = totalWeight > 0f ? (spot.weight / totalWeight) * 100f : 0f;

        IconSprite = icons.Get(spot.type);
        if (iconImage != null) iconImage.sprite = IconSprite;
        if (glow != null) glow.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData e) => holdRoutine = StartCoroutine(HoldToShow());

    public void OnPointerUp(PointerEventData e)
    {
        if (holdRoutine != null) StopCoroutine(holdRoutine);
        if (OddsPopup.I != null) OddsPopup.I.Hide();
    }

    private IEnumerator HoldToShow()
    {
        yield return new WaitForSeconds(holdDelay);
        if (OddsPopup.I != null)
            OddsPopup.I.Show(transform.position, label, pct, RarityColors.Of(rarity));
    }

    public void PlayLanding()
    {
        if (!captured) { originalScale = transform.localScale; captured = true; }
        if (landingRoutine != null) StopCoroutine(landingRoutine);  // cancel any in-progress punch
        transform.localScale = originalScale;                       // always reset before starting
        landingRoutine = StartCoroutine(LandingRoutine());
    }

    private IEnumerator LandingRoutine()
    {
        if (glow != null)
        {
            glow.color = RarityColors.Of(rarity);
            glow.gameObject.SetActive(true);
        }
        float t = 0f, dur = 0.35f;
        while (t < dur)
        {
            t += Time.unscaledDeltaTime;
            float punch = 1f + 0.25f * Mathf.Sin((t / dur) * Mathf.PI);
            transform.localScale = originalScale * punch;   // ALWAYS from the stored original
            yield return null;
        }
        transform.localScale = originalScale;
        if (glow != null) glow.gameObject.SetActive(false);
        landingRoutine = null;
    }
}
