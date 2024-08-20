using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject paused;
    public GameObject settings;

    private bool isPaused;
    private bool isSettings;

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        isSettings = false;

        paused.SetActive(false);
        settings.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused) PauseOn();
            else if (isPaused && !isSettings && NoteManager.isMusicStarted) StartCoroutine(CountToResume());
            else if (isPaused && !isSettings) PauseOff();
            else if (isPaused && isSettings) SettingsBackButton();
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
        print("done!");
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
