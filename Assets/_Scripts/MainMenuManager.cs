using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    [SerializeField] private string gameSceneName = "GuitarScene";

    /// <summary>
    /// This function will be called by the Start Game button.
    /// </summary>
    public void StartGame()
    {
        Debug.Log($"Loading scene: {gameSceneName}");
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}