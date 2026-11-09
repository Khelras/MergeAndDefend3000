using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    //[SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource BackGround;


    [Header("Audio clip")]
    [Header("EnemySlam")]
    public AudioClip MachineGunsSFX;
    public AudioClip BackGroundMusic;
    public AudioClip LazerSFX;
    public AudioClip SniperSFX;
    public AudioClip CannonSFX;
    public AudioClip EnemyDeathSFX;


    private void Awake()
    {
        // Check if an instance already exists in the scene
        if (Instance != null && Instance != this)
        {
            // Destroy the duplicate GameObject to maintain exactly one instance
            Destroy(gameObject);
            return;
        }

        //  Assign this instance as the global reference
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 100f) / 100f;
        AudioListener.volume = savedVolume;

        BackGround.PlayOneShot(BackGroundMusic);
    }

    public void PlaySFX(AudioClip clip, float pitch)
    {
        SFXSource.pitch = pitch;
        SFXSource.PlayOneShot(clip);
    }

    public void PauseAudio()
    {
        BackGround.Pause();
        SFXSource.Pause();
    }

    public void ResumeAudio()
    {
        BackGround.UnPause();
        SFXSource.UnPause();
    }
}
