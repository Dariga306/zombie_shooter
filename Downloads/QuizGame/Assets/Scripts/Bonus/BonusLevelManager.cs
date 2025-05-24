using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class BonusLevelManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI questionText;
    public Image questionImage;
    public Button[] optionButtons;
    public GameObject winPanel;
    public TextMeshProUGUI finalText;

    private List<QuizQuestion> bonusQuestions;
    private int currentIndex = 0;
    private int correctAnswers = 0;
    public AudioClip correctSound;
public AudioClip wrongSound;
public AudioSource audioSource;
public AudioClip WinSound;

    void Start()
    {
        LoadBonusQuestions();
        DisplayQuestion();
    }

    void LoadBonusQuestions()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Questions/BonusLevelQuestions");

        if (jsonFile != null)
        {
            bonusQuestions = new List<QuizQuestion>(JsonHelper.FromJson<QuizQuestion>(jsonFile.text));
        }
        else
        {
            Debug.LogError("❌ Не удалось загрузить BonusQuestions.json!");
        }
    }

    void DisplayQuestion()
    {
        if (currentIndex >= bonusQuestions.Count)
        {
            ShowWinPanel();
            return;
        }

        QuizQuestion q = bonusQuestions[currentIndex];
        questionText.text = q.question;

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

        // Заполняем кнопки
        for (int i = 0; i < optionButtons.Length; i++)
        {
            optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = q.options[i];
            int index = i; // захватываем индекс
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => CheckAnswer(index));
        }
    }

    void CheckAnswer(int selectedIndex)
{
    QuizQuestion q = bonusQuestions[currentIndex];

    // Подсветка кнопок
    for (int i = 0; i < optionButtons.Length; i++)
    {
        optionButtons[i].interactable = false;
        optionButtons[i].image.color = (i == q.correctIndex) ? Color.green : Color.red;
    }

    // Аудио + учёт правильного ответа
    if (selectedIndex == q.correctIndex)
    {
        correctAnswers++;
        audioSource.PlayOneShot(correctSound);
    }
    else
    {
        audioSource.PlayOneShot(wrongSound);
    }

    // Переход к следующему вопросу
    Invoke(nameof(NextQuestion), 1.5f);
}


void NextQuestion()
{
    currentIndex++;

    // Очистить подсветку
    foreach (var btn in optionButtons)
    {
        btn.image.color = Color.white;
        btn.interactable = true;
    }

    DisplayQuestion();
}


    void ShowWinPanel()
    {
        winPanel.SetActive(true);

        if (correctAnswers == bonusQuestions.Count)
        {
            finalText.text = "You're a true K-POP fan! \nAmazing job recognizing your idols!";
            audioSource.PlayOneShot(WinSound);
        }
        else
        {
            finalText.text = "Not bad, but keep practicing!\nYou'll master your K-POP idol knowledge soon!";
            audioSource.PlayOneShot(WinSound);
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
