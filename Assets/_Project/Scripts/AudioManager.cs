using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager I;

    [Header("Source")]
    public AudioSource source;

    [Header("Clips")]
    public AudioClip pull;
    public AudioClip starUp;
    public AudioClip evolve;
    public AudioClip feed;

    private void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);
        if (source == null) source = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip, float pitch = 1f, float volume = 1f)
    {
        if (clip == null || source == null) return;
        source.pitch = pitch;
        source.PlayOneShot(clip, volume);
    }
}
