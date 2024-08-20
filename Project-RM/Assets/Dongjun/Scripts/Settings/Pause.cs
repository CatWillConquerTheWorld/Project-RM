using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject paused;
    public GameObject settings;

    private bool isPaused;
    private bool isSettings;
    private bool isExitingPause;

    public static bool isOKToPause;

    public TMP_Text countText;
    // Start is called before the first frame update
    void Start()
    {
        isOKToPause = false;

        isExitingPause = false;
        isPaused = false;
        isSettings = false;

        countText.enabled = false;  
        paused.SetActive(false);
        settings.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isOKToPause)
            {
                if (!isPaused) PauseOn();
                else if (isPaused && !isSettings && NoteManager.isMusicStarted && !isExitingPause) StartCoroutine(CountToResume());
                else if (isPaused && !isSettings) PauseOff();
                else if (isPaused && isSettings) SettingsBackButton();
            }
        }
        //if (isPaused && !isSettings && Input.GetKeyDown(KeyCode.Escape))
        //{
        //    PauseOff();
        //}

        //else if (!isPaused && Input.GetKeyDown(KeyCode.Escape)) 
        //{
        //    PauseOn();
        //}
    }

    void PauseOn()
    {
        isExitingPause = false;
        if (NoteManager.isMusicStarted) CenterFrame.MusicPause();
        isPaused = true;
        isSettings = false;
        settings.SetActive(false);
        paused.SetActive(true);

        Time.timeScale = 0;
    }

    void PauseOff()
    {
        isPaused = false;
        paused.SetActive(false);

        Time.timeScale = 1;
    }

    public void PauseButton()
    {
        PauseOn();
    }

    public void ResumeButton()
    {
        print("Done");
        PauseOff();
    }

    public void SettingsButton()
    {
        isSettings = true;
        paused.SetActive(false);
        settings.SetActive(true);
    }

    public void SettingsBackButton()
    {
        isSettings = false;
        paused.SetActive(true);
        settings.SetActive(false);
    }

    IEnumerator CountToResume()
    {
        isExitingPause = false;
        countText.enabled = true;
        paused.SetActive(false);
        isPaused = false;
        countText.text = "3";
        yield return new WaitForSecondsRealtime(60f / bpmManager.instance.bpm);
        countText.text = "2";
        yield return new WaitForSecondsRealtime(60f / bpmManager.instance.bpm);
        countText.text = "1";
        yield return new WaitForSecondsRealtime(60f / bpmManager.instance.bpm);
        countText.text = "GO!";
        yield return new WaitForSecondsRealtime(60f / bpmManager.instance.bpm);
        countText.enabled = false;
        Time.timeScale = 1;
        CenterFrame.MusicUnPause();
        yield return null;
    }

    public void QuitButton()
    {
# if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
