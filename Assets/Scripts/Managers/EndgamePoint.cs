using UnityEngine;

public class EndgamePoint : MonoBehaviour
{
    [SerializeField] private bool isEndGame; // True if this point should trigger the endgame
    [SerializeField] private GameObject endgameScreen; // Reference to the Endgame UI Canvas

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isEndGame)
            {
                ShowEndgameScreen(); // Show the endgame UI
            }
            else
            {
                SceneManagement.instance.LoadLevel(); // Load the next level
            }
        }
    }

    private void ShowEndgameScreen()
    {
        if (endgameScreen != null)
        {
            endgameScreen.SetActive(true); // Activate the endgame screen
            Time.timeScale = 0f; // Pause the game
        }
        else
        {
            Debug.LogWarning("EndgameScreen is not assigned in the inspector.");
        }
    }

    // Button action to return to main menu
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // Resume the game before transitioning
        SceneManagement.instance.LoadMainMenu();
    }

    // Button action to quit the game
    public void QuitGame()
    {
        Application.Quit();
    }
}
