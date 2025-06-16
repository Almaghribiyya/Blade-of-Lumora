using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //Scene manager buat ngeloadnya
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    //Masuk ke Menu Settings
    public void GoToSettingMenu()
    {
        SceneManager.LoadScene("SettingsMenu");
    }

    //Masuk ke Scene MainMenu
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    //Quit game function (cuman bisa ditest saat dibuild diluar unity editor)
    public void QuitGame()
    {
        Application.Quit();
    }
}
