using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject infoPanel;

    [Header("Buttons")]
    public GameObject infoButton; // кнопка "?"
    public GameObject backButton; // кнопка "назад"
void Start()
{
    if (MusicManager.Instance != null)
    {
        MusicManager.Instance.PlayMusic();
    }
}
    public void LoadLevel1()
    {
        FadeMusicAndLoadScene("Level1_MC");
    }

    public void LoadLevel2()
    {
        FadeMusicAndLoadScene("Level2_TF");
    }

    public void LoadLevel3()
    {
        FadeMusicAndLoadScene("Level3_Word");
    }

    public void ShowInfo()
    {
        infoPanel.SetActive(true);
        infoButton.SetActive(false); // Скрыть кнопку Info
        backButton.SetActive(true);  // Показать кнопку Назад
    }

    public void CloseInfo()
    {
        infoPanel.SetActive(false);
        infoButton.SetActive(true);  // Показать кнопку Info обратно
        backButton.SetActive(false); // Скрыть кнопку Назад
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void FadeMusicAndLoadScene(string sceneName)
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.FadeOutAndStop(1.5f);
        }

        // Задержка перед загрузкой сцены (через корутину), чтобы музыка успела затухнуть
        StartCoroutine(LoadSceneWithDelay(sceneName, 1.5f));
    }

    System.Collections.IEnumerator LoadSceneWithDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
public void LoadBonusLevel()
{
    if (MusicManager.Instance != null)
        MusicManager.Instance.FadeOutAndStop(1.5f);

    Invoke(nameof(ActuallyLoadBonus), 1.5f);
}

private void ActuallyLoadBonus()
{
    SceneManager.LoadScene("BonusLevel");
}
}
