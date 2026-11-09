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

        Instance = this;

    }

    private void Start()
    {
        BackGround.PlayOneShot(BackGroundMusic);
    }

    public void PlaySFX(AudioClip clip, float pitch)
    {
        SFXSource.pitch = pitch;
        SFXSource.PlayOneShot(clip);
    }
}
