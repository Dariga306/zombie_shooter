using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotClick : MonoBehaviour
{
    [SerializeField] private Transform lettersParent;
    [SerializeField] private GameObject letterPrefab;
    [SerializeField] private Transform slotsParent;


    private Button slotButton;

    void Start()
{
    slotButton = GetComponent<Button>();
    slotButton.onClick.AddListener(OnClick);

    if (lettersParent == null)
        lettersParent = GameObject.Find("LettersParent")?.transform;

    if (slotsParent == null)
        slotsParent = GameObject.Find("SlotsParent")?.transform;

    if (slotsParent == null)
        Debug.LogError("❗ [SlotClick] SlotsParent not found in the scene! Make sure the name is exactly 'SlotsParent'");
}

    void OnClick()
{
    TextMeshProUGUI text = transform.Find("SlotText")?.GetComponent<TextMeshProUGUI>();
    if (text != null && !string.IsNullOrEmpty(text.text))
    {
        // создаём букву обратно
        GameObject letter = Instantiate(letterPrefab, lettersParent);
        letter.GetComponentInChildren<TextMeshProUGUI>().text = text.text;

        // очищаем слот
        text.text = "";

        // Запоминаем индекс освободившегося слота
        for (int i = 0; i < slotsParent.childCount; i++)
        {
            if (slotsParent.GetChild(i) == this.transform)
            {
                LetterClick.freeSlotIndex = i;
                break;
            }
        }
    }
}

}
