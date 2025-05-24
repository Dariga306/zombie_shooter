using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryManager : MonoBehaviour
{
    public GameObject victoryPanel;
    public Button restartButton;

    void Start()
    {
        victoryPanel.SetActive(false); // Скрываем панель в начале
        restartButton.onClick.AddListener(RestartGame); // Подключаем обработчик
    }

    public void ShowVictory()
    {
        victoryPanel.SetActive(true); // Показываем панель
        Time.timeScale = 0f; // Останавливаем время
    }

    public void RestartGame()
{
    Time.timeScale = 1f;
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}
}

