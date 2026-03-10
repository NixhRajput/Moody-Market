using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources (assign in inspector)")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Music")]
    public AudioClip bgMusic;

    [Header("Sound Effects")]
    public AudioClip harvestSound;
    public AudioClip gainSound;
    public AudioClip gameStartSound;
    public AudioClip buttonBeep;

    private bool musicOn = true;
    private bool soundOn = true;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        AudioSource[] sources = GetComponents<AudioSource>();
        if (sources.Length >= 2)
        {
            musicSource = sources[0];
            sfxSource = sources[1];
        }
        Debug.Log($"[Audio] AudioManager initialized, Instance set: {Instance != null}");
    }

    void Start()
    {
        if (bgMusic != null)
        {
            musicSource.clip = bgMusic;
            musicSource.loop = true;
            musicSource.Play();
            Debug.Log("[Audio] BG Music started");
        }
    }

    public void PlayHarvest() => PlaySFX(harvestSound);
    public void PlayGain() => PlaySFX(gainSound);
    public void PlayGameStart() => PlaySFX(gameStartSound);
    public void PlayButtonBeep() => PlaySFX(buttonBeep);

    void PlaySFX(AudioClip clip)
    {
        if (clip == null || !soundOn) return;
        sfxSource.PlayOneShot(clip, 1f);
        Debug.Log($"[Audio] SFX: {clip.name}");
    }

    public void SetMusic(bool on)
    {
        musicOn = on;
        if (musicOn) musicSource.Play();
        else musicSource.Stop();
    }

    public void SetSound(bool on) => soundOn = on;
    public bool IsMusicOn() => musicOn;
    public bool IsSoundOn() => soundOn;
}