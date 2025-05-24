using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ResultsManager : MonoBehaviour
{
    public TextMeshProUGUI level1Text;
    public TextMeshProUGUI level2Text;
    public TextMeshProUGUI level3Text;
    public GameObject bonusButton;

    void OnEnable()
    {
        // Здесь мы загружаем сохраненные результаты
        int level1Score = PlayerPrefs.GetInt("Level1Score", -1);
        int level2Score = PlayerPrefs.GetInt("Level2Score", -1);
        int level3Score = PlayerPrefs.GetInt("Level3Score", -1);

        // Теперь обновляем тексты
        if (level1Score != -1)
            level1Text.text = "Level 1: " + level1Score + " / 10";
        else
            level1Text.text = "Level 1: not completed yet";

        if (level2Score != -1)
            level2Text.text = "Level 2: " + level2Score + " / 10";
        else
            level2Text.text = "Level 2: not completed yet";

        if (level3Score != -1)
            level3Text.text = "Level 3: " + level3Score + " / 10";
        else
            level3Text.text = "Level 3: not completed yet";

        // Бонусная кнопка
        if (level1Score == 10 && level2Score == 10 && level3Score == 10)
        {
            bonusButton.SetActive(true);
        }
        else
        {
            bonusButton.SetActive(false);
        }
    }

    public void PlayBonusLevel()
    {
        SceneManager.LoadScene("BonusLevel"); // Название сцены с бонусом
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
