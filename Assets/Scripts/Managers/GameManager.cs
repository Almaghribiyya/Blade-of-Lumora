
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public bool GameActive = true;
    //Objek buat UI gameovernya
    public GameObject gameOverUI;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (gameOverUI.activeInHierarchy)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    //Settings gameover, main menu dan quit masih sama kayak di file mainmenu.cs
    public void gameOver()
    {
        gameOverUI.SetActive(true);
        GameActive = false;
        Time.timeScale = 0f; // Pause seluruh permainan
    }

    public void restart()
    {
        Time.timeScale = 1f; // Kembalikan ke normal
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void quit()
    {
        Application.Quit();
    }   
}





