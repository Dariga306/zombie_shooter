using TMPro;
using UnityEngine;

public class KillCounter : MonoBehaviour
{
    public static KillCounter Instance; 
    public TextMeshProUGUI killText;

    private int killCount = 0;
    public VictoryManager victoryManager;
public int totalZombies = 10; // Задай сколько зомби всего в сцене


    private void Awake()
    {
        Instance = this;
    }

    public void AddKill()
{
    killCount++;
    UpdateUI();

    if (killCount >= totalZombies)
    {
        victoryManager.ShowVictory();
    }
}


    private void UpdateUI()
    {
        killText.text = "Zombies Killed: " + killCount;
    }
}
