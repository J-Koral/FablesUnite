using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FlyMote : MonoBehaviour
{
    // Fly from start to end after an optional stagger delay, then self-destruct.
    public void Launch(Sprite sprite, Vector3 start, Vector3 end, float delay, float duration)
    {
        StartCoroutine(Run(sprite, start, end, delay, duration));
    }

    private IEnumerator Run(Sprite sprite, Vector3 start, Vector3 end, float delay, float duration)
    {
        Image img = GetComponent<Image>();
        if (img != null && sprite != null) img.sprite = sprite;   // show the REAL prize
        if (img != null) img.color = Color.white;                 // white so the art shows true

        RectTransform rt = (RectTransform)transform;
        Vector3 scatter = new Vector3(Random.Range(-35f, 35f), Random.Range(-45f, 10f), 0f);
        rt.position    = start + scatter;   // spread the 8 so they don't perfectly overlap
        rt.localScale  = Vector3.one * 0.55f;

        float wait = 0f;                    // wait our turn in the marching line
        while (wait < delay) { wait += Time.unscaledDeltaTime; yield return null; }

        // --- anticipation: pop away from the target, hover, THEN fly in ---
        Vector3 away = (start - end).normalized * 40f;   // outward from the target
        Vector3 popTo = rt.position + away;
        float pd = 0.12f, pe = 0f;
        Vector3 popFrom = rt.position;
        while (pe < pd)
        {
            pe += Time.unscaledDeltaTime;
            float pk = pe / pd;
            rt.position   = Vector3.Lerp(popFrom, popTo, pk);
            rt.localScale = Vector3.one * Mathf.Lerp(0.55f, 0.75f, pk);   // swell slightly
            yield return null;
        }
        yield return new WaitForSecondsRealtime(0.05f);   // brief hover

        Vector3 from = rt.position;
        float t = 0f;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float k = t / duration;
            float ease = k * k * (3f - 2f * k);              // smoothstep for a nicer arc
            rt.position   = Vector3.Lerp(from, end, ease);
            rt.localScale = Vector3.one * Mathf.Lerp(0.55f, 0.05f, k);  // shrinks "into" the target
            yield return null;
        }
        Destroy(gameObject);   // gone once it lands
    }
}
