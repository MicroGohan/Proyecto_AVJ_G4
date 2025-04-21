using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Header("--------------------Audio Source---------------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("--------------------Audio Clip---------------")]
    public AudioClip background;
    public AudioClip Attack;
    public AudioClip Death;
    public AudioClip Teleport;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }


}
