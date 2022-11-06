using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }
    private void Awake()
    {
        Time.timeScale = 1;
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void StartGame()
    {
        LoadingData.LoadScene("GamePlay");
    }

    public void ShowLeaderboard()
    {
        LoadingData.LoadScene("Leaderboard");
    }
}
