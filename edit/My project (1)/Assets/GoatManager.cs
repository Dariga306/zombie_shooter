using UnityEngine;

public class GoatManager : MonoBehaviour
{
    private GoatMovement[] goats;

    void Start()
    {
        goats = FindObjectsByType<GoatMovement>(FindObjectsSortMode.None);
        foreach (var goat in goats)
        {
            goat.StopMoving();
        }

        StartGoats();
    }

    public void StartGoats()
    {
        foreach (var goat in goats)
        {
            if (goat.name == "SK_Goat_white (1)")
                goat.StartWalking();
        }
    }

    public void StopGoats()
    {
        foreach (var goat in goats)
        {
            goat.StopMoving();
        }
    }
}
