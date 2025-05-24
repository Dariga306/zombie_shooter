using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class QuizManager_Level2 : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public Button trueButton;
    public Button falseButton;
    public GameObject winPanel;
    public GameObject wrongAnswerPanel;
    public TextMeshProUGUI correctAnswerText;
    public AudioSource audioSource;
    public TextMeshProUGUI finalScoreText;
    public FriendHelpUI friendHelpUI; // подключи из инспектора
    public AudioClip correctSound;
public AudioClip wrongSound;
public AudioClip WinSound;



    private List<QuizQuestion> questions;
    private int currentIndex = 0;
    private int correctCount = 0;

    void Start()
    {
        LoadQuestions();
        DisplayQuestion();
    }

    void LoadQuestions()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Questions/Level2Questions");
        questions = new List<QuizQuestion>(JsonHelper.FromJson<QuizQuestion>(jsonFile.text));
    }

    void DisplayQuestion()
    {
        if (currentIndex >= questions.Count)
        {
            ShowWinPanel();
            return;
        }

        var q = questions[currentIndex];

        questionText.text = q.question;

        trueButton.onClick.RemoveAllListeners();
        falseButton.onClick.RemoveAllListeners();

        trueButton.onClick.AddListener(() => CheckAnswer(0));
        falseButton.onClick.AddListener(() => CheckAnswer(1));

        // Воспроизвести аудио, если есть
        if (!string.IsNullOrEmpty(q.audio))
        {
            AudioClip clip = Resources.Load<AudioClip>("Audio/" + q.audio);
            if (clip != null)
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }

        // Сброс цвета кнопок
        trueButton.image.color = Color.white;
        falseButton.image.color = Color.white;
    }

    void CheckAnswer(int selectedIndex)
    {
        if (friendHelpUI != null)
        friendHelpUI.HideHint();

        var q = questions[currentIndex];

        trueButton.interactable = false;
        falseButton.interactable = false;

        // ⏹️ Остановить песню
        if (audioSource.isPlaying)
            audioSource.Stop();

        // Подсветить правильный/неправильный ответ
        if (selectedIndex == q.correctIndex)
        {
            correctCount++;
             audioSource.PlayOneShot(correctSound);

            if (selectedIndex == 0)
                trueButton.image.color = Color.green;
            else
                falseButton.image.color = Color.green;

            Invoke(nameof(NextQuestion), 1.5f);
        }
        else
        {
             audioSource.PlayOneShot(wrongSound);
            // Выбранная неправильная кнопка — красная
            if (selectedIndex == 0)
                trueButton.image.color = Color.red;
            else
                falseButton.image.color = Color.red;

            // Правильная кнопка — зелёная
            if (q.correctIndex == 0)
                trueButton.image.color = Color.green;
            else
                falseButton.image.color = Color.green;

            correctAnswerText.text = "Correct answer: " + q.explanation;
            Invoke(nameof(ShowWrongAnswerPanel), 1f);
        }
    }

    void ShowWrongAnswerPanel()
    {
        wrongAnswerPanel.SetActive(true);
    }

    public void NextQuestion()
    {
        wrongAnswerPanel.SetActive(false);
        currentIndex++;
        trueButton.interactable = true;
        falseButton.interactable = true;
        DisplayQuestion();
    }

    void ShowWinPanel()
{
    PlayerPrefs.SetInt("Level2Score", correctCount);
    PlayerPrefs.Save();

    if (winPanel != null)
    {
        winPanel.SetActive(true);

        // Показываем финальный результат
        if (finalScoreText != null)
        {
            finalScoreText.text = "You answered correctly: " + correctCount + " / " + questions.Count;
            audioSource.PlayOneShot(WinSound);
        }
    }
}
public QuizQuestion GetCurrentQuestion()
{
    return questions[currentIndex];
}


    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
