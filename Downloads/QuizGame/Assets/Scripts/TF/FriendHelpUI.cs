using UnityEngine;
using TMPro;
using System.Collections;

public class FriendHelpUI : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI helpText;
    private QuizManager_Level2 quizManager; // ссылка на викторину
    private int usesLeft = 3;
    private bool isVisible = false;

    private void Start()
    {
        quizManager = FindObjectOfType<QuizManager_Level2>();
    }

    public void ShowHint()
    {
        if (usesLeft <= 0 || isVisible) return;

        var currentQuestion = quizManager.GetCurrentQuestion();
        helpText.text = GenerateHint(currentQuestion);

        usesLeft--;

        gameObject.SetActive(true);
        StartCoroutine(DelayedFadeIn());
    }

    IEnumerator DelayedFadeIn()
    {
        yield return null;
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        isVisible = true;
        canvasGroup.alpha = 0;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        float duration = 0.5f;
        float t = 0;

        while (t < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1;
    }

    public void HideHint()
    {
        if (isVisible)
            StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float duration = 0.4f;
        float t = 0;

        while (t < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        gameObject.SetActive(false);
        isVisible = false;
    }

    private string GenerateHint(QuizQuestion question)
    {
        // Если правильный ответ TRUE (index 0)
        if (question.correctIndex == 0)
        {
            return "I feel like... it's TRUE!";
        }
        else
        {
            return "Hmm... I think it's FALSE!";
        }
    }
}
