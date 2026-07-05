using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader I { get; private set; }

    [Tooltip("The CanvasGroup on the full-screen black image used for fading.")]
    public CanvasGroup fader;
    public float fadeSeconds = 0.3f;

    private bool isLoading;

    private void Awake()
    {
        if (I != null) { Destroy(gameObject); return; }   // duplicate self-destructs
        I = this;
        DontDestroyOnLoad(gameObject);
        if (fader != null) { fader.alpha = 0f; fader.blocksRaycasts = false; }
    }

    // Call this from any nav button.
    public void Load(string sceneName)
    {
        if (isLoading) return;
        StartCoroutine(LoadRoutine(sceneName));
    }

    private IEnumerator LoadRoutine(string sceneName)
    {
        isLoading = true;
        yield return Fade(1f);                          // fade to black
        yield return SceneManager.LoadSceneAsync(sceneName);
        yield return Fade(0f);                          // fade back in
        isLoading = false;
    }

    private IEnumerator Fade(float target)
    {
        if (fader == null) yield break;
        fader.blocksRaycasts = true;                    // block taps mid-load
        float start = fader.alpha;
        float t = 0f;
        while (t < fadeSeconds)
        {
            t += Time.unscaledDeltaTime;                // unscaled: works even if paused
            fader.alpha = Mathf.Lerp(start, target, t / fadeSeconds);
            yield return null;
        }
        fader.alpha = target;
        fader.blocksRaycasts = target > 0.5f;
    }
}
