using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void OnStartButtonClick()
    {
        SceneManager.LoadScene("GameScene"); 
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
        Debug.Log("Выход из игры"); 
    }
}
