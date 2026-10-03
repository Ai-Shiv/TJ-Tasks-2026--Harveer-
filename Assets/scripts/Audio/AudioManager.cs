using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Music")]
    [SerializeField] private AudioClip backgroundMusic;

    [Header("SFX")]
    [SerializeField] private AudioClip playerShootSound;
    [SerializeField] private AudioClip enemyShootSound;
    [SerializeField] private AudioClip enemyDeathSound;
    [SerializeField] private AudioClip healthPickupSound;
    [SerializeField] private AudioClip playerDeathSound;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (musicSource != null &&
            backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlayPlayerShoot()
    {
        PlaySFX(playerShootSound);
    }

    public void PlayEnemyShoot()
    {
        PlaySFX(enemyShootSound);
    }

    public void PlayEnemyDeath()
    {
        PlaySFX(enemyDeathSound);
    }

    public void PlayHealthPickup()
    {
        PlaySFX(healthPickupSound);
    }

    public void PlayPlayerDeath()
    {
        PlaySFX(playerDeathSound);
    }

    private void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null &&
            clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}