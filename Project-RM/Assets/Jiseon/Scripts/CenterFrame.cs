using DG.Tweening;
using System.ComponentModel;
using UnityEngine;

public class CenterFrame : MonoBehaviour
{
    public static AudioSource myAudio;
    public AudioClip[] audioClips;
    public static bool musicStart = false;

    public static GameObject container;
    public static NoteManager noteManager;

    static float volume;
    public bool music_change;

    void Start()
    {
        volume = 1.0f;
        myAudio = GetComponent<AudioSource>();

        container = GameObject.Find("Note");
        noteManager = GameObject.Find("Note").GetComponent<NoteManager>();
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
        music_change = true;
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
        for (int i = 0; i < container.transform.childCount; i++)
        {
            if (container.transform.GetChild(i).gameObject.activeSelf)
            {
                if (container.transform.GetChild(i).gameObject.name == "Note(Clone)" || container.transform.GetChild(i).gameObject.name == "LONGNOTE(Clone)")
                {
                    container.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
        LoadBMS.currentTime = -1000000000d;
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
