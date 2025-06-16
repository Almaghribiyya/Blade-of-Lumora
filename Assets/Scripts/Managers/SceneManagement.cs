using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public static SceneManagement instance;

    private void Awake()
    {
        // Ensure this is a singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Optional if you want this to persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Method to load the next level
    public void LoadLevel()
    {
        SceneManager.LoadScene("Level2");
    }

    // Method to load the main menu
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
