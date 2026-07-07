using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TreeSpotUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Visuals")]
    public Image iconImage;   // the prize picture (usually this object's own Image)
    public Image glow;        // sits behind the icon, flashes on landing (starts hidden)

    [Header("Hold to reveal odds")]
    public float holdDelay = 0.35f;

    private float pct;
    private string label;
    private Rarity rarity;
    private Coroutine holdRoutine;

    public void Bind(TreeSpot spot, float totalWeight, TreeIconLibrary icons)
    {
        label  = spot.label;
        rarity = spot.rarity;
        pct    = totalWeight > 0f ? (spot.weight / totalWeight) * 100f : 0f;
        if (iconImage != null) iconImage.sprite = icons.Get(spot.type);
        if (glow != null) glow.gameObject.SetActive(false);
    }

    // Press and hold this prize to see its chance.
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

    // Called when a pull lands on this spot: glow in rarity color + a little scale punch.
    public void PlayLanding() => StartCoroutine(LandingRoutine());

    private IEnumerator LandingRoutine()
    {
        if (glow != null)
        {
            glow.color = RarityColors.Of(rarity);
            glow.gameObject.SetActive(true);
        }
        Vector3 baseScale = transform.localScale;
        float t = 0f, dur = 0.35f;
        while (t < dur)
        {
            t += Time.unscaledDeltaTime;
            float punch = 1f + 0.25f * Mathf.Sin((t / dur) * Mathf.PI); // grow then settle
            transform.localScale = baseScale * punch;
            yield return null;
        }
        transform.localScale = baseScale;
        if (glow != null) glow.gameObject.SetActive(false);
    }
}