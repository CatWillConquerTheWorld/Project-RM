using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Exit : MonoBehaviour
{
    public static bool pause = false;
    public GameObject Menu;
    public GameObject SettingsMenu;
    
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        Menu.SetActive(false);
        SettingsMenu.SetActive(false);
    }

   void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(pause){
                Resume();
            } else {
                Pause();
            }
            Debug.Log(pause);
        }
        if(SettingsMenu){
            if(Input.GetKeyDown(KeyCode.Escape)){
                ExitSettings();
            }
        }
    }

    public void Resume(){
        Menu.SetActive(false);
        Time.timeScale = 1f;
        pause = false;
    }

    public void Pause(){
        Menu.SetActive(true);
        Time.timeScale = 0f;
        pause = true;
    }

    public void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Resume();
    }

    public void Quit(){
        Application.Quit();
    }

    public void Settings(){
        Menu.SetActive(false);
        SettingsMenu.SetActive(true);
    }

    public void ExitSettings(){
        Menu.SetActive(true);
        SettingsMenu.SetActive(false);
    }
}
