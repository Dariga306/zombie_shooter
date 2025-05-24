using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LetterClick : MonoBehaviour
{
    private Button button;
    private string letter;
    private static Transform slotsParent;
    private static GameObject[] filledLetters;
    public static int freeSlotIndex = -1;

    [Header("Sound")]
    public AudioClip clickSound;
    private static AudioSource audioSource;

    void Start()
    {
        button = GetComponent<Button>();
        letter = GetComponentInChildren<TextMeshProUGUI>().text;

        // Кэшируем ссылки один раз
        if (slotsParent == null)
        {
            slotsParent = GameObject.Find("SlotsParent").transform;
            filledLetters = new GameObject[slotsParent.childCount];
        }

        if (audioSource == null)
        {
            GameObject soundSource = GameObject.Find("LetterSoundSource");
            if (soundSource != null)
                audioSource = soundSource.GetComponent<AudioSource>();
        }

        button.onClick.AddListener(OnLetterClick);
    }

    void OnLetterClick()
    {
        PlayClickSound();

        if (freeSlotIndex >= 0)
        {
            Transform slot = slotsParent.GetChild(freeSlotIndex);
            TextMeshProUGUI slotText = slot.GetComponentInChildren<TextMeshProUGUI>();
            if (slotText != null && string.IsNullOrEmpty(slotText.text))
            {
                slotText.text = letter;
                freeSlotIndex = -1;
                gameObject.SetActive(false);
                return;
            }
        }

        for (int i = 0; i < slotsParent.childCount; i++)
        {
            TextMeshProUGUI slotText = slotsParent.GetChild(i).GetComponentInChildren<TextMeshProUGUI>();
            if (slotText != null && string.IsNullOrEmpty(slotText.text))
            {
                slotText.text = letter;
                gameObject.SetActive(false);
                break;
            }
        }
    }

    void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    public static void ResetSlots()
    {
        for (int i = 0; i < filledLetters.Length; i++)
        {
            if (filledLetters[i] != null)
                filledLetters[i].SetActive(true);

            GameObject slot = slotsParent.GetChild(i).gameObject;
            TextMeshProUGUI slotText = slot.GetComponentInChildren<TextMeshProUGUI>();
            if (slotText != null)
                slotText.text = "";

            filledLetters[i] = null;
        }
    }
}
