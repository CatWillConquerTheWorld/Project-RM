using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
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
            if(SettingsMenu.activeSelf){
                ExitSettings();
            }
            else if(pause){
                Debug.Log("1");
                Resume();
            } else {
                Pause();
            }
        }
    }

    public void Resume(){
        Menu.SetActive(false);
        Time.timeScale = 1f;
        pause = false;
    }

    public void Pause(){
        pause = true;
        Menu.SetActive(true);
        Time.timeScale = 0f;
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
