using UnityEngine;

public class OpenResultsPanel : MonoBehaviour
{
    public GameObject resultsPanel;

    public void OpenResults()
    {
        if (resultsPanel != null)
        {
            resultsPanel.SetActive(true);
        }
    }
}
