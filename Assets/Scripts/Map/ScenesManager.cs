using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public void OpenMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    public void LoadEmptyMap()
    {
        SceneParameters.LoadSavedMap = false;
        SceneManager.LoadScene("MapEditor");
    }

    public void StartGameplay()
    {
        SceneParameters.LoadSavedMap = true;
        SceneManager.LoadScene("Gameplay");
    }

    public void LoadSavedMap()
    {
        SceneParameters.LoadSavedMap = true;
        SceneManager.LoadScene("MapEditor");
    }
    public void Exit()
    {
        Application.Quit();
    }
}
