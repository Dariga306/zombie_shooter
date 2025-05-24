using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // чтобы загружать меню

public class QuizManager : MonoBehaviour
{
    [Header("Quiz UI")]
    public TextMeshProUGUI questionText;
    public Button[] optionButtons;
    public Image questionImage;
    public AudioSource audioSource;
    public GameObject wrongAnswerPanel;
    public TextMeshProUGUI correctAnswerText;

    [Header("Hint Settings")]
    private bool hintUsed = false;
    private int hintUsesLeft = 3; // всего можно использовать 3 раза
    public GameObject hint5050Button; // кнопка подсказки

    [Header("Win Panel")]
    public GameObject winPanel;
    public TextMeshProUGUI finalScoreText;

    [Header("Sounds")]
public AudioClip correctSound;
public AudioClip wrongSound;
private AudioSource musicSource;
public AudioClip WinSound;

    private List<QuizQuestion> questions;
    private int currentIndex = 0;
    private int correctAnswersCount = 0; // Сколько правильных ответов

    void Start()
{
    LoadQuestions();
    DisplayQuestion();

}

    void LoadQuestions()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Questions/Level1Questions");
        questions = new List<QuizQuestion>(JsonHelper.FromJson<QuizQuestion>(jsonFile.text));
    }

    void DisplayQuestion()
{
    if (currentIndex >= questions.Count)
    {
        Debug.Log("Quiz finished!");
        ShowWinPanel(); // Показываем победную панель
        return;
    }

    var q = questions[currentIndex];

    questionText.text = q.question;

    for (int i = 0; i < optionButtons.Length; i++)
    {
        optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = q.options[i];
        int index = i;
        optionButtons[i].onClick.RemoveAllListeners();
        optionButtons[i].onClick.AddListener(() => CheckAnswer(index));
    }

    // Показ изображения
    if (!string.IsNullOrEmpty(q.image))
    {
        questionImage.gameObject.SetActive(true);
        questionImage.sprite = Resources.Load<Sprite>("Images/" + q.image);
    }
    else
    {
        questionImage.gameObject.SetActive(false);
    }

    // Обработка аудио и фоновой музыки
    if (!string.IsNullOrEmpty(q.audio))
    {
        audioSource.clip = Resources.Load<AudioClip>("Audio/" + q.audio);
        audioSource.Play();

        if (q.hasMusic && MusicManager.Instance != null)
        {
            MusicManager.Instance.MuteMusic();
        }
    }
    else
    {
        // Вернуть музыку, если нет аудио
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.UnmuteMusic();
        }
    }
}


    void CheckAnswer(int selectedIndex)
{
    var q = questions[currentIndex];

    foreach (Button btn in optionButtons)
    {
        btn.image.color = Color.white;
        btn.interactable = false;
    }

    if (audioSource.isPlaying)
        audioSource.Stop();

    if (selectedIndex == q.correctIndex)
    {
        optionButtons[selectedIndex].image.color = Color.green;
        correctAnswersCount++;

        // ▶️ Звук правильного ответа
        audioSource.PlayOneShot(correctSound);

        Invoke(nameof(NextQuestion), 1.5f);
    }
    else
    {
        optionButtons[selectedIndex].image.color = Color.red;
        optionButtons[q.correctIndex].image.color = Color.green;
        correctAnswerText.text = "Correct answer: " + q.options[q.correctIndex];

        // 🔴 Звук ошибки
        audioSource.PlayOneShot(wrongSound);

        Invoke(nameof(ShowWrongAnswerPanel), 1f);
    }
}


    void ShowWrongAnswerPanel()
    {
        wrongAnswerPanel.SetActive(true);
    }

    public void UseHint5050()
    {
        if (hintUsed || hintUsesLeft <= 0)
            return;

        var q = questions[currentIndex];

        List<int> wrongOptions = new List<int>();
        for (int i = 0; i < q.options.Length; i++)
        {
            if (i != q.correctIndex)
                wrongOptions.Add(i);
        }

        for (int i = 0; i < 2; i++)
        {
            int randomIndex = Random.Range(0, wrongOptions.Count);
            int buttonIndex = wrongOptions[randomIndex];
            optionButtons[buttonIndex].gameObject.SetActive(false);
            wrongOptions.RemoveAt(randomIndex);
        }

        hintUsed = true;
        hintUsesLeft--;

        if (hintUsesLeft <= 0 && hint5050Button != null)
        {
            Button btn = hint5050Button.GetComponent<Button>();
if (btn != null)
    btn.interactable = false;

Image img = hint5050Button.GetComponent<Image>();
if (img != null)
    img.color = new Color(img.color.r, img.color.g, img.color.b, 0.5f);

        }
    }

    public void NextQuestion()

    {
        wrongAnswerPanel.SetActive(false);

        currentIndex++;

        hintUsed = false;

        foreach (Button btn in optionButtons)
        {
            btn.gameObject.SetActive(true);
            btn.image.color = Color.white;
            btn.interactable = true;
        }

        DisplayQuestion();
        if (MusicManager.Instance != null)
{
    MusicManager.Instance.UnmuteMusic();
}


    }

    void ShowWinPanel()
{
    if (winPanel != null)
    {
        // Сохраняем очки для текущего уровня
        SaveLevelScore();

        winPanel.SetActive(true);
        finalScoreText.text = "You answered correctly: " + correctAnswersCount + " / " + questions.Count;
        audioSource.PlayOneShot(WinSound);
    }
}
void SaveLevelScore()
{
    // Определяем уровень по сцене
    string sceneName = SceneManager.GetActiveScene().name;

    if (sceneName == "Level1_MC")
    {
        PlayerPrefs.SetInt("Level1Score", correctAnswersCount);
    }
    else if (sceneName == "Level2_TF")
    {
        PlayerPrefs.SetInt("Level2Score", correctAnswersCount);
    }
    else if (sceneName == "Level3_Word")
    {
        PlayerPrefs.SetInt("Level3Score", correctAnswersCount);
    }

    PlayerPrefs.Save(); // Обязательно сохраняем
}


    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Переход на главное меню
    }
}
