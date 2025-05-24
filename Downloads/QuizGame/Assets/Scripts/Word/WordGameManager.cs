using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[System.Serializable]
public class WordData
{
    public string word;
}

[System.Serializable]
public class WordList
{
    public WordData[] words;
}

public class WordGameManager : MonoBehaviour
{
    public Transform slotsParent;
    public Transform lettersParent;
    public GameObject slotPrefab;
    public GameObject letterPrefab;
    public TextMeshProUGUI resultText;
    public Button checkButton;
    private int correctAnswers = 0;
    public GameObject winPanel;
public TextMeshProUGUI finalText;
public GameObject wrongAnswerPanel;
public Button hintButton;
private int hintsUsedForCurrentWord = 0;
private int totalHintsAvailable = 30;

public AudioSource audioSource;
public AudioClip correctSound;
public AudioClip wrongSound;
public AudioClip WinSound;



    private string currentWord;
    private List<GameObject> slotList = new List<GameObject>();
    private List<string> wordsList = new List<string>();
    private int currentWordIndex = 0;

    void Start()
    {
        LoadWords();
        PickWord();
        GenerateSlots(currentWord);
        GenerateLetters(currentWord);
        checkButton.onClick.AddListener(CheckAnswer);

        resultText.gameObject.SetActive(false); // скрыть текст в начале
    hintButton.interactable = true;
    }


    void LoadWords()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Questions/Level3Words");
        if (jsonFile != null)
        {
            WordList loadedWords = JsonUtility.FromJson<WordList>("{\"words\":" + jsonFile.text + "}");
            foreach (var w in loadedWords.words)
            {
                wordsList.Add(w.word.ToUpper());
            }
        }
        else
        {
            Debug.LogError("❗ Cannot load Level3Words.json!");
        }
    }

    void PickWord()
    {
        if (currentWordIndex < wordsList.Count)
        {
            currentWord = wordsList[currentWordIndex];
        }
        else
        {
            Debug.Log("🎉 All words completed!");
            // Здесь можно загрузить победную панель
        }
    }

    void GenerateSlots(string word)
    {
        foreach (Transform child in slotsParent)
            Destroy(child.gameObject);

        slotList.Clear();

        for (int i = 0; i < word.Length; i++)
        {
            GameObject slot = Instantiate(slotPrefab, slotsParent);
            slotList.Add(slot);

            // Скрыть текст
            TextMeshProUGUI slotText = slot.GetComponentInChildren<TextMeshProUGUI>();
            if (slotText != null)
                slotText.text = "";
        }
    }

    void GenerateLetters(string word)
    {
        foreach (Transform child in lettersParent)
            Destroy(child.gameObject);

        List<char> chars = new List<char>(word.ToCharArray());
        Shuffle(chars);

        foreach (char c in chars)
        {
            GameObject letter = Instantiate(letterPrefab, lettersParent);
            letter.GetComponentInChildren<TextMeshProUGUI>().text = c.ToString();
        }
    }

    void CheckAnswer()
{
    string assembled = "";

    foreach (var slot in slotList)
    {
        TextMeshProUGUI slotText = slot.GetComponentInChildren<TextMeshProUGUI>();
        if (slotText == null || string.IsNullOrEmpty(slotText.text))
        {
            resultText.gameObject.SetActive(true);
            resultText.text = "Fill all letters!";
            return;
        }

        assembled += slotText.text;
    }

    resultText.gameObject.SetActive(true);

    if (assembled.ToUpper() == currentWord)
    {
        correctAnswers++;
        resultText.text = "Correct!";
        audioSource.PlayOneShot(correctSound);
        Invoke(nameof(LoadNextWord), 1.5f);
    }
    else
    {
        resultText.text = "Incorrect!";
        audioSource.PlayOneShot(wrongSound);
        Invoke(nameof(ShowWrongAnswerOptions), 1.5f); // вместо Restart
    }
}

void ShowWrongAnswerOptions()
{
    wrongAnswerPanel.SetActive(true);
}

    public void LoadNextWord()
{
    wrongAnswerPanel.SetActive(false);
    currentWordIndex++;
    if (currentWordIndex >= wordsList.Count)
    {
        // ВСЕ слова пройдены
        ShowWinPanel();
    }
    else
    {
        PickWord();
        GenerateSlots(currentWord);
        GenerateLetters(currentWord);
        resultText.gameObject.SetActive(false);
        hintsUsedForCurrentWord = 0;
hintButton.interactable = totalHintsAvailable > 0;

    }
}

void ShowWinPanel()
{
    winPanel.SetActive(true);
    finalText.text = "Correct answers: " + correctAnswers + " / " + wordsList.Count;
    audioSource.PlayOneShot(WinSound);

    // Сохраняем в PlayerPrefs для Results сцены
    PlayerPrefs.SetInt("Level3Score", correctAnswers);
    PlayerPrefs.SetInt("Level3Max", wordsList.Count);
    PlayerPrefs.Save();
}

    public void RestartCurrentWord()
    {
        wrongAnswerPanel.SetActive(false);
        GenerateSlots(currentWord);
        GenerateLetters(currentWord);
        resultText.gameObject.SetActive(false); // скрыть текст
        hintsUsedForCurrentWord = 0;
hintButton.interactable = totalHintsAvailable > 0;

    }

    void Shuffle(List<char> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            char temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void BackToMenu()
{
    SceneManager.LoadScene("MainMenu"); // Убедись, что сцена называется именно так
}
public void UseHint()
{
    if (hintsUsedForCurrentWord >= 3 || totalHintsAvailable <= 0)
        return;

    // Находим первую пустую ячейку
    for (int i = 0; i < currentWord.Length; i++)
    {
        var slotText = slotList[i].GetComponentInChildren<TextMeshProUGUI>();

        if (string.IsNullOrEmpty(slotText.text))
        {
            // Вставляем правильную букву
            string correctLetter = currentWord[i].ToString();
            slotText.text = correctLetter;
            hintsUsedForCurrentWord++;
            totalHintsAvailable--;

            // Ищем и удаляем соответствующую букву из букв снизу
            bool letterFound = false;
            foreach (Transform letterObj in lettersParent)
            {
                var letterText = letterObj.GetComponentInChildren<TextMeshProUGUI>();
                if (letterText != null && letterText.text == correctLetter)
                {
                    Destroy(letterObj.gameObject); // удаляем букву
                    letterFound = true;
                    break; // остановить поиск после удаления
                }
            }

            if (!letterFound)
            {
                Debug.LogWarning("Couldn't find matching letter to remove.");
            }

            break; // ⬅️ ОБЯЗАТЕЛЬНО выходим из цикла сразу после первой вставки!!
        }
    }

    if (hintsUsedForCurrentWord >= 3 || totalHintsAvailable <= 0)
    {
        hintButton.interactable = false;
    }
}




}
