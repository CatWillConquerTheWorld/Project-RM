using DG.Tweening;
using UnityEngine;

public class CenterFrame : MonoBehaviour
{
    public static AudioSource myAudio;
    public AudioClip[] audioClips;
    public static bool musicStart = false;

    static float volume;

    void Start()
    {
        volume = 1.0f;
        myAudio = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (!myAudio.isPlaying)
        {
            musicStart = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!musicStart)
        {
            if (collision.CompareTag("Note"))
            {
                MusicStart();
            }
        }
    }

    public void ChangeMusic(int number)
    {
        myAudio.clip = audioClips[number];
    }
    public void MusicStart()
    {
        if (!musicStart)
        {
            myAudio.volume = 1.0f;
            myAudio.Play();
            musicStart = true;
        }
    }

    public static void MusicFadeOut()
    {
        volume = 1.0f;
        DOTween.To(() => volume, x => volume = x, 0f, 0.5f).SetEase(Ease.OutSine).OnUpdate(() => myAudio.volume = volume).OnComplete(() => myAudio.Stop());
    }

    public static void MusicPause()
    {
        myAudio.Pause();
    }

    public static void MusicUnPause()
    {
        myAudio.UnPause();
    }
}
