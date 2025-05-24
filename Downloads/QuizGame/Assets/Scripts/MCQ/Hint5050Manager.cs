using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Hint5050Manager : MonoBehaviour
{
    public QuizManager quizManager; // Ссылка на основной QuizManager

    public void UseHint()
    {
        if (quizManager != null)
        {
            quizManager.UseHint5050(); // вызываем метод из QuizManager
            Button btn = GetComponent<Button>();
if (btn != null)
    btn.interactable = false;

Image img = GetComponent<Image>();
if (img != null)
    img.color = new Color(img.color.r, img.color.g, img.color.b, 0.5f);

        }
        else
        {
            Debug.LogError("QuizManager not assigned to Hint5050Manager!");
        }
    }
}
